// Google Cloud Storage adapter for the VRT baseline sync.
//
// Requirements:
//   - Application Default Credentials available
//     (`gcloud auth application-default login`) with the
//     "Storage Object Admin" role (roles/storage.objectAdmin) on the bucket
//   - vrt.config.ts declares (replacing the Azure fields):
//       storageBucket: process.env.VRT_STORAGE_BUCKET ?? "<bucket name>",
//
// GCS computes and stores each object's md5Hash server-side and returns it
// in listings, so the MD5 diff sync works without any caveats.

import { Storage } from "@google-cloud/storage";
import { join } from "node:path";
import { vrtConfig } from "../vrt.config.ts";
import { runPool, snapshotsDir, type FileListing, type StorageAdapter } from "./snapshot-sync.ts";

// No network traffic happens until the first actual request.
const bucket = new Storage().bucket(vrtConfig.storageBucket);

export const storageAdapter: StorageAdapter = {
  remoteLabel: `gs://${vrtConfig.storageBucket}`,

  async listRemote(): Promise<FileListing> {
    const listing: FileListing = new Map();
    const [files] = await bucket.getFiles();
    for (const file of files) {
      // Only flat *.png names can be snapshot baselines; ignore anything
      // else that may have been dropped into the bucket.
      if (!file.name.endsWith(".png") || file.name.includes("/")) continue;
      // md5Hash is computed server-side on upload (base64).
      listing.set(file.name, file.metadata.md5Hash ?? undefined);
    }
    return listing;
  },

  async download(names: readonly string[]): Promise<void> {
    await runPool(names, async (name) => {
      await bucket.file(name).download({ destination: join(snapshotsDir, name) });
    });
  },

  async upload(local: FileListing, names: readonly string[]): Promise<void> {
    await runPool(names, async (name) => {
      await bucket.upload(join(snapshotsDir, name), {
        destination: name,
        contentType: "image/png",
        // Verify the payload against its MD5 end to end.
        validation: "md5",
      });
    });
  },

  async deleteRemote(names: readonly string[]): Promise<void> {
    await runPool(names, async (name) => {
      await bucket.file(name).delete();
    });
  },

  errorHint(error: unknown): string {
    const message = error instanceof Error ? error.message : String(error);
    const statusCode = (error as { code?: number })?.code;
    if (message.includes("Could not load the default credentials")) {
      return "Hint: run `gcloud auth application-default login` first (Application Default Credentials are the credential source).";
    }
    if (statusCode === 403) {
      return `Hint: the signed-in account needs the "Storage Object Admin" role (roles/storage.objectAdmin) on bucket "${vrtConfig.storageBucket}".`;
    }
    if (statusCode === 404) {
      return `Hint: bucket "${vrtConfig.storageBucket}" was not found — check vrt.config.ts.`;
    }
    return `Hint: check the storage settings in vrt.config.ts and your network connection. (${message})`;
  },
};
