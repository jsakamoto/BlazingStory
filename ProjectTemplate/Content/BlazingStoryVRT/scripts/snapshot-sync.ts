// Provider-agnostic core of the VRT baseline sync: local listing, MD5 diff
// planning, name filtering, the transfer pool, and the CLI / globalSetup
// entry points.
//
// This file is NOT swapped when switching cloud providers — only
// scripts/snapshot-storage.ts (the StorageAdapter implementation) is.
// See "Switching the cloud provider" in AGENTS.md.

import { createHash } from "node:crypto";
import { existsSync, mkdirSync, readdirSync, readFileSync, rmSync } from "node:fs";
import { join } from "node:path";
import { createInterface } from "node:readline/promises";
import { parseArgs } from "node:util";

export const snapshotsDir = join(import.meta.dirname, "..", "tests", "vrt.spec.ts-snapshots");

// Playwright appends the platform to every snapshot file name
// (e.g. "…--default-linux.png"), so one cloud container can hold baselines
// for several platforms side by side. Most operations only act on the
// current platform's set.
const platformSuffix = `-${process.platform}.png`;

const TRANSFER_CONCURRENCY = 8;

// Both listings map file name → content MD5 (base64), the key the diff mode
// compares. A remote file whose MD5 is unknown maps to undefined and is
// always treated as "changed".
export type FileListing = Map<string, string | undefined>;

// What a provider adapter (scripts/snapshot-storage.ts) must implement.
export interface StorageAdapter {
  /** Shown in messages, e.g. "demovrt01storage/demo-vrt01-snapshots". */
  remoteLabel: string;
  /** List the remote *.png files: name → content MD5 (base64, or undefined if unknown). */
  listRemote(): Promise<FileListing>;
  /** Download the named remote files into snapshotsDir (the dir already exists). */
  download(names: readonly string[]): Promise<void>;
  /** Upload the named files from snapshotsDir; `local` carries their MD5s (base64). */
  upload(local: FileListing, names: readonly string[]): Promise<void>;
  /** Delete the named remote files. */
  deleteRemote(names: readonly string[]): Promise<void>;
  /** One actionable line for the auth/connection errors this provider is likely to hit. */
  errorHint(error: unknown): string;
}

export function listLocal(): FileListing {
  const listing: FileListing = new Map();
  if (!existsSync(snapshotsDir)) return listing;
  for (const entry of readdirSync(snapshotsDir, { withFileTypes: true })) {
    if (!entry.isFile() || !entry.name.endsWith(".png")) continue;
    const md5 = createHash("md5").update(readFileSync(join(snapshotsDir, entry.name))).digest("base64");
    listing.set(entry.name, md5);
  }
  return listing;
}

export interface NameFilter {
  glob?: string;
  platformOnly?: boolean;
}

// Supports "*" and "?" wildcards. (path.matchesGlob is still experimental in
// Node 24 and emits a warning on every use, hence this tiny substitute.)
function globToRegExp(glob: string): RegExp {
  const escaped = glob
    .replace(/[.+^${}()|[\]\\]/g, "\\$&")
    .replace(/\*/g, ".*")
    .replace(/\?/g, ".");
  return new RegExp(`^${escaped}$`);
}

export function filterNames(listing: FileListing, filter: NameFilter): FileListing {
  const globRegExp = filter.glob ? globToRegExp(filter.glob) : undefined;
  return new Map([...listing].filter(([name]) =>
    (!filter.platformOnly || name.endsWith(platformSuffix)) &&
    (!globRegExp || globRegExp.test(name))
  ));
}

export type SyncMode = "diff" | "force" | "missing";

export interface SyncPlan {
  transfer: { name: string; reason: "missing" | "changed" | "forced" }[];
  upToDate: number;
  // In pruneScope but not in source — deleted from the destination when
  // --prune is given.
  orphans: string[];
}

export function planSync(
  source: FileListing,
  dest: FileListing,
  mode: SyncMode,
  pruneScope: FileListing,
): SyncPlan {
  const plan: SyncPlan = { transfer: [], upToDate: 0, orphans: [] };
  for (const [name, sourceMd5] of source) {
    if (!dest.has(name)) {
      plan.transfer.push({ name, reason: "missing" });
    } else if (mode === "force") {
      plan.transfer.push({ name, reason: "forced" });
    } else if (mode === "diff" && (sourceMd5 === undefined || sourceMd5 !== dest.get(name))) {
      plan.transfer.push({ name, reason: "changed" });
    } else {
      plan.upToDate++;
    }
  }
  plan.orphans = [...pruneScope.keys()].filter((name) => !source.has(name));
  return plan;
}

// Small worker pool the adapters use for their per-file transfers.
export async function runPool<T>(items: readonly T[], fn: (item: T) => Promise<void>): Promise<void> {
  const queue = [...items];
  const workers = Array.from({ length: Math.min(TRANSFER_CONCURRENCY, queue.length) }, async () => {
    for (let item = queue.shift(); item !== undefined; item = queue.shift()) {
      await fn(item);
    }
  });
  await Promise.all(workers);
}

// The missing-only download used by global-setup.ts before every test run:
// current platform only, no deletion, no confirmation.
export async function pullMissingBaselines(adapter: StorageAdapter): Promise<void> {
  const remote = filterNames(await adapter.listRemote(), { platformOnly: true });
  const plan = planSync(remote, listLocal(), "missing", new Map());
  if (plan.transfer.length === 0) {
    console.log(`[vrt-cloud] baselines up to date (${plan.upToDate} present, nothing to download)`);
    return;
  }
  mkdirSync(snapshotsDir, { recursive: true });
  await adapter.download(plan.transfer.map((t) => t.name));
  console.log(`[vrt-cloud] downloaded ${plan.transfer.length} missing baseline(s) from ${adapter.remoteLabel}`);
}

const USAGE = (direction: "pull" | "push") => `Usage: npm run snapshots:${direction} [-- <options>]

Sync VRT baselines (tests/vrt.spec.ts-snapshots/) ${direction === "pull" ? "down from" : "up to"} the cloud storage.
Default mode transfers only missing and changed (by MD5) files.

Options:
  --force           transfer everything, overwriting the destination
  --missing         transfer only files absent from the destination
  --filter <glob>   only file names matching the glob ("*" and "?" supported)
  ${direction === "pull"
    ? "--all-platforms  also pull baselines of other platforms (default: current platform only)"
    : "--prune           delete remote files (current platform only) that no longer exist locally"}${direction === "pull"
    ? "\n  --prune           delete local files (current platform only) that no longer exist remotely"
    : ""}
  --dry-run         show the plan without transferring anything
  --yes             skip the confirmation prompt
  --help            show this help`;

// Entry point shared by the two thin CLIs (snapshots-pull.ts / snapshots-push.ts).
export async function runSyncCli(direction: "pull" | "push", adapter: StorageAdapter): Promise<void> {
  const { values } = parseArgs({
    options: {
      force: { type: "boolean", default: false },
      missing: { type: "boolean", default: false },
      prune: { type: "boolean", default: false },
      filter: { type: "string" },
      "all-platforms": { type: "boolean", default: false },
      "dry-run": { type: "boolean", default: false },
      yes: { type: "boolean", default: false },
      help: { type: "boolean", default: false },
    },
  });
  if (values.help) {
    console.log(USAGE(direction));
    return;
  }
  if (values.force && values.missing) {
    console.error("--force and --missing are mutually exclusive.");
    process.exitCode = 1;
    return;
  }
  const mode: SyncMode = values.force ? "force" : values.missing ? "missing" : "diff";

  try {
    const remote = await adapter.listRemote();
    const local = listLocal();

    // Pull acts on the current platform's baselines unless --all-platforms;
    // push offers everything that exists locally. Pruning is always scoped
    // to the current platform so a push from e.g. linux can never delete
    // the -win32/-darwin sets.
    const sourceFilter: NameFilter = {
      glob: values.filter,
      platformOnly: direction === "pull" && !values["all-platforms"],
    };
    const pruneFilter: NameFilter = { glob: values.filter, platformOnly: true };
    const source = filterNames(direction === "pull" ? remote : local, sourceFilter);
    const dest = direction === "pull" ? local : remote;
    const pruneScope = values.prune ? filterNames(dest, pruneFilter) : new Map<string, undefined>();

    const plan = planSync(source, dest, mode, pruneScope);

    const marks = { missing: "+", changed: "~", forced: "!" } as const;
    for (const { name, reason } of plan.transfer) console.log(`  ${marks[reason]} ${name} (${reason})`);
    for (const name of plan.orphans) console.log(`  - ${name} (orphan, will be deleted)`);
    const counts = (["missing", "changed", "forced"] as const)
      .map((reason) => [plan.transfer.filter((t) => t.reason === reason).length, reason] as const)
      .filter(([count]) => count > 0)
      .map(([count, reason]) => `${count} ${reason}`)
      .join(", ");
    console.log(`${direction} plan (${mode} mode): ` +
      `${plan.transfer.length} to ${direction === "pull" ? "download" : "upload"}` +
      (counts ? ` (${counts})` : "") +
      `, ${plan.upToDate} up to date` +
      (values.prune ? `, ${plan.orphans.length} orphan(s) to delete` : ""));

    if (values["dry-run"]) return;
    if (plan.transfer.length === 0 && plan.orphans.length === 0) return;

    if (!values.yes && process.stdin.isTTY && process.stdout.isTTY) {
      const readline = createInterface({ input: process.stdin, output: process.stdout });
      const answer = await readline.question("Proceed? [y/N] ");
      readline.close();
      if (!/^y(es)?$/i.test(answer.trim())) {
        console.log("Aborted.");
        return;
      }
    }

    const names = plan.transfer.map((t) => t.name);
    if (direction === "pull") {
      mkdirSync(snapshotsDir, { recursive: true });
      await adapter.download(names);
      for (const name of plan.orphans) rmSync(join(snapshotsDir, name));
    } else {
      await adapter.upload(local, names);
      await adapter.deleteRemote(plan.orphans);
    }
    console.log(`Done: ${names.length} ${direction === "pull" ? "downloaded" : "uploaded"}` +
      (plan.orphans.length > 0 ? `, ${plan.orphans.length} deleted` : "") + ".");
  } catch (error) {
    console.error(`snapshots:${direction} failed: ${error instanceof Error ? error.message : String(error)}`);
    console.error(adapter.errorHint(error));
    process.exitCode = 1;
  }
}
