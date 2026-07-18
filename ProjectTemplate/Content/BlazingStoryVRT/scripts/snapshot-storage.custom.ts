// Template for a custom storage adapter: implement the members below and any
// storage service can hold the VRT baselines.
//
// Put your connection values in vrt.config.ts (import { vrtConfig } from
// "../vrt.config.ts"). Helpers in snapshot-sync.ts: runPool (parallel
// transfers), snapshotsDir (the local baseline dir). Working examples:
// snapshot-storage.{azure,aws,gcs}.ts.

import type { FileListing, StorageAdapter } from "./snapshot-sync.ts";

export const storageAdapter: StorageAdapter = {
  // Shown in messages, e.g. "myservice://my-bucket".
  remoteLabel: "TODO",

  // Return remote file name → content MD5 (base64). undefined MD5 = always
  // treated as "changed". Skip anything that isn't a flat *.png name.
  async listRemote(): Promise<FileListing> {
    throw new Error("listRemote: not implemented");
  },

  // Write each named remote file to join(snapshotsDir, name).
  async download(names: readonly string[]): Promise<void> {
    throw new Error("download: not implemented");
  },

  // Upload each named file from join(snapshotsDir, name). `local` maps
  // name → MD5 (base64); make sure listRemote can return that MD5 later
  // (store it as metadata if the service doesn't compute one itself).
  async upload(local: FileListing, names: readonly string[]): Promise<void> {
    throw new Error("upload: not implemented");
  },

  // Delete the named remote files (used by --prune).
  async deleteRemote(names: readonly string[]): Promise<void> {
    throw new Error("deleteRemote: not implemented");
  },

  // One actionable line per likely failure: not logged in, missing
  // permission, wrong bucket/container name, …
  errorHint(error: unknown): string {
    return "Hint: TODO";
  },
};
