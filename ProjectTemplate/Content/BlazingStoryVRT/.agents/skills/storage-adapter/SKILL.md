---
name: storage-adapter
description: Implement scripts/snapshot-storage.ts so a storage service (Dropbox, MinIO, WebDAV, any S3-compatible, …) can hold the VRT baseline screenshots. Use when the user asks to implement, set up, or switch the snapshot storage backend of this VRT project.
---

# Implementing a storage adapter

This project shares its VRT baseline screenshots (`tests/vrt.spec.ts-snapshots/`) through
a storage service instead of committing them to git. Which service that is depends
entirely on one file, `scripts/snapshot-storage.ts`, which ships here as an empty
template exporting a single `storageAdapter: StorageAdapter`. Implement its members and
any service can hold the baselines. Everything else (the CLIs, diff planning,
globalSetup, `--prune`, prompts) is provider-agnostic and must not be touched.

Read before writing code:

- `scripts/snapshot-storage.ts`: the empty template, one comment per member
- `scripts/snapshot-sync.ts`: the `StorageAdapter` interface (the contract) plus the
  helpers the adapter uses, `runPool` (parallel per-file transfers) and `snapshotsDir`
  (the local baseline directory)

## How the pieces fit

- `scripts/global-setup.ts` (Playwright's `globalSetup`) downloads the baselines missing
  locally before every test run: current platform only, never overwrites or deletes.
  `VRT_SKIP_CLOUD=1` skips it, and any other failure aborts the run with `errorHint()`.
- `npm run snapshots:pull` / `npm run snapshots:push` are the explicit syncs. They
  default to a diff sync by content MD5, print the plan, and ask for confirmation on an
  interactive terminal. Options: `--force`, `--missing`, `--filter <glob>`,
  `--all-platforms` (pull only), `--prune`, `--dry-run`, `--yes`.
- Baseline file names carry a platform suffix (`<story-id>-<platform>.png`), so one
  remote store holds the linux / win32 / darwin sets side by side. The common code does
  that filtering; the adapter only ever deals with plain flat file names.

## Ask the user first

1. Which service, and which SDK (prefer an official npm SDK; note its package name)
2. The auth model. Prefer a CLI login or ambient credentials. Secrets must NOT go into
   `vrt.config.ts` (it is committed); if unavoidable, read them from env vars only.

## Contract invariants (what the common code relies on)

- `listRemote()` returns name → content MD5 in **base64**, or `undefined` when unknown.
  `undefined` makes the diff sync treat that file as always changed: acceptable as a
  fallback, wrong as the normal case. Skip any name that is not a flat `*.png`.
- `upload()` must ensure a *later* `listRemote()` can return the uploaded file's MD5.
  Pick whichever mechanism the service offers:
  - it computes and returns a content hash itself (Google Cloud Storage does this with
    `md5Hash`): nothing to do
  - it has a content-MD5 header or property settable on upload (Azure Blob Storage's
    `Content-MD5`): set it explicitly, otherwise every later sync sees the file as changed
  - it returns an entity tag that equals the MD5 only under conditions (an S3 ETag does,
    for single-part uploads on non-KMS buckets): normalize it (quoted hex to base64) and
    map every value you cannot trust to `undefined`, then document the caveat in the
    file's header comment
  - none of the above: store the MD5 yourself as user-defined metadata next to the file
    and read it back in `listRemote()`
- `download()` writes to `join(snapshotsDir, name)`; the directory already exists.
- Wrap per-file transfers in `runPool` (exported by `snapshot-sync.ts` together with
  `snapshotsDir`).
- `errorHint(error)` maps each likely failure (no credentials, permission denied,
  bucket or container not found) to ONE actionable line, and falls back to a generic
  "check vrt.config.ts and your network connection" line carrying the message.
- Module-level client construction is fine, but it must not perform network I/O at
  import time.
- Erasable TypeScript only (no `enum`, no `namespace`: Node runs these files through
  type stripping) and explicit `.ts` extensions on local imports.

## File conventions

- Start the file with a terse header comment: the SDK package, the auth prerequisites,
  the exact `vrt.config.ts` fields the adapter reads, and any caveat such as the ETag
  one above.
- Connection values live in `vrt.config.ts` and only the adapter reads them. Follow the
  existing field pattern, env override first:
  `storageBucket: process.env.VRT_STORAGE_BUCKET ?? "<bucket name>"`. Leave `baseURL`
  alone; it belongs to the test run, not the storage.
- Comments in English and short. Never use em dashes or en dashes anywhere in this
  project, comments and strings alike; it is the project owner's style preference.
  Restructure the sentence with a colon, parentheses, or a full stop instead.

## Wiring it up

1. `npm i -D <sdk package>`
2. Add the connection fields to `vrt.config.ts`
3. `npx tsc --noEmit` (`scripts/snapshot-storage.ts` is part of the type check)

## Verification

1. `npm run snapshots:push -- --dry-run`: the plan lists the local baselines
2. `npm run snapshots:push`, then push again: expect `0 to upload, N up to date`. This
   proves the MD5 round-trip; if the files stay "changed", the MD5s returned by
   `listRemote()` are wrong.
3. Delete one local PNG, then `npm run snapshots:pull` restores exactly that file.
