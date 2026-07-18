// Playwright globalSetup, run before every test run (but not for bare test
// listing / Test Explorer discovery).
//#if (SnapshotsStorage != "none")
//
//   1. Download baselines missing locally from a storage service
//      (current platform only, never deletes or overwrites). Kept as a
//      missing-only sync that runs every time — an "only when the folder is
//      empty" check would let a partially-populated folder slip through, and
//      Playwright would then silently create a fresh local baseline for any
//      story whose team baseline was never downloaded.
//      Skip with VRT_SKIP_CLOUD=1 (e.g. for offline work).
//   2. Regenerate tests/stories.json from the running app (gen-stories.ts).
//#else
// This setup regenerates tests/stories.json from the running app (gen-stories.ts).
//#endif

import type { FullConfig } from "@playwright/test";
import generateStoryIndex from "./gen-stories.ts";
//#if (SnapshotsStorage != "none")
import { pullMissingBaselines } from "./snapshot-sync.ts";
import { storageAdapter } from "./snapshot-storage.ts";
//#endif

export default async function globalSetup(config: FullConfig): Promise<void> {
//#if (SnapshotsStorage != "none")
  if (process.env.VRT_SKIP_CLOUD === "1") {
    console.log("[vrt-cloud] VRT_SKIP_CLOUD=1 — skipping baseline download");
  } else {
    try {
      await pullMissingBaselines(storageAdapter);
    } catch (error) {
      const message = error instanceof Error ? error.message : String(error);
      throw new Error(
        `Downloading VRT baselines from ${storageAdapter.remoteLabel} failed: ${message}\n` +
        `${storageAdapter.errorHint(error)}\n` +
        "(Set VRT_SKIP_CLOUD=1 to run tests without the cloud sync.)",
      );
    }
  }
//#endif
  await generateStoryIndex(config);
}
