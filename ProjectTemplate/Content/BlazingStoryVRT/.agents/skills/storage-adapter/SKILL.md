---
name: storage-adapter
description: Implement a snapshot-storage adapter so a new storage service (Dropbox, MinIO, WebDAV, any S3-compatible, …) can hold the VRT baselines. Use when the user asks to support / implement / a new snapshot storage backend, or to switch the sync to a service that has no scripts/snapshot-storage.<provider>.ts yet.
---

# Implementing a storage adapter

An adapter is one file, `scripts/snapshot-storage.<provider>.ts`, exporting a single
`storageAdapter: StorageAdapter`. Everything else (CLI, diff planning, globalSetup,
`--prune`, prompts) is provider-agnostic and must not be touched.

Read before writing code:

- `scripts/snapshot-sync.ts` — the `StorageAdapter` interface (the contract) and helpers
- `scripts/snapshot-storage.custom.ts` — the empty template to start from
- One working example: `scripts/snapshot-storage.azure.ts` / `.aws.ts` / `.gcs.ts`
- AGENTS.md § "Switching the cloud provider"

## Ask the user first

1. Which service / SDK (prefer an official npm SDK; note its package name)
2. Auth model — prefer CLI-login / ambient credentials. Secrets must NOT go into
   `vrt.config.ts` (it is committed); if unavoidable, use env vars only
3. Whether to just add the variant file, or also activate it now

## Contract invariants (what the common code relies on)

- `listRemote()` returns name → content-MD5 **base64**, or `undefined` when unknown.
  `undefined` makes the diff sync treat the file as always-changed — acceptable as a
  fallback, wrong as the normal case. Skip non-flat / non-`*.png` names.
- `upload()` must ensure a *later* `listRemote()` can return the uploaded file's MD5.
  Each example solves this differently — pick the matching strategy:
  - Azure: sets the `Content-MD5` blob header explicitly on upload
  - S3: relies on ETag = MD5 (single-part PUT, non-KMS only; normalize quoted hex →
    base64, treat multipart/KMS ETags as `undefined`) — caveat documented in its header
  - GCS: server computes `md5Hash`; nothing to do
- `download()` writes to `join(snapshotsDir, name)`; the directory already exists.
- Wrap per-file transfers in `runPool` (both are exported by snapshot-sync.ts).
- `errorHint(error)` maps each likely failure (no credentials / permission denied /
  bucket not found) to ONE actionable line. Follow the examples' style.
- Module-level client construction is fine; it must not perform network I/O at import.
- Erasable TypeScript only (no enum/namespace — Node runs these files via type
  stripping) and explicit `.ts` extensions on local imports.

## File conventions

- Header comment (keep it terse, mirror the existing adapters): activation `cp` line,
  Requirements (SDK package, auth prerequisites, the exact `vrt.config.ts` fields as a
  sample), provider-specific caveats if any.
- Connection values: only the adapter reads `vrt.config.ts`. Field pattern:
  `storageBucket: process.env.VRT_STORAGE_BUCKET ?? "<name>"` (env override first).
- Comments in English, short; code style matches the existing adapters.

## Type-checking a variant

Variants are excluded from `npx tsc --noEmit` (tsconfig excludes
`scripts/snapshot-storage.*.ts`) because inactive SDKs are not installed. To check the
new adapter without activating it: install its SDK, `cp` it to a temporary name like
`scripts/tmp-adapter-check.ts`, run `npx tsc --noEmit`, then delete the copy (and the
SDK, if not activating).

## Activation (only when the user wants to switch)

Follow AGENTS.md § "Switching the cloud provider": `cp` over
`scripts/snapshot-storage.ts`, rewrite the storage fields in `vrt.config.ts` (keep
`baseURL`), `npm rm` the old SDK / `npm i -D` the new one, `npx tsc --noEmit`.

## Verification

1. `npm run snapshots:push -- --dry-run` — plan lists the local baselines
2. `npm run snapshots:push` — then push again: expect `0 to upload, N up to date`
   (proves the MD5 round-trip; if files stay "changed", `listRemote` MD5s are wrong)
3. Delete one local PNG → `npm run snapshots:pull` restores exactly that file
4. Update AGENTS.md: the "verified against real stores" sentence and, if the variant
   list is enumerated, the Structure section
