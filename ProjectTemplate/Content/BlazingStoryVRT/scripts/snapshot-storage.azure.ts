// Azure Blob Storage adapter for the VRT baseline sync.
//
// Requirements:
//   - Azure CLI installed and logged in (`az login`), and the signed-in user
//     holds the "Storage Blob Data Contributor" role on the storage account
//     (being subscription Owner/Contributor is not enough)
//   - vrt.config.ts declares:
//       storageAccount: process.env.VRT_STORAGE_ACCOUNT ?? "<account name>",
//       storageContainer: process.env.VRT_STORAGE_CONTAINER ?? "<container name>",

import { AzureCliCredential } from "@azure/identity";
import { BlobServiceClient } from "@azure/storage-blob";
import { join } from "node:path";
import { vrtConfig } from "../vrt.config.ts";
import { runPool, snapshotsDir, type FileListing, type StorageAdapter } from "./snapshot-sync.ts";

// No network traffic happens until the first actual request.
const container = new BlobServiceClient(
  `https://${vrtConfig.storageAccount}.blob.core.windows.net`,
  new AzureCliCredential(),
).getContainerClient(vrtConfig.storageContainer);

export const storageAdapter: StorageAdapter = {
  remoteLabel: `${vrtConfig.storageAccount}/${vrtConfig.storageContainer}`,

  async listRemote(): Promise<FileListing> {
    const listing: FileListing = new Map();
    for await (const blob of container.listBlobsFlat()) {
      // Only flat *.png names can be snapshot baselines; ignore anything
      // else that may have been dropped into the container.
      if (!blob.name.endsWith(".png") || blob.name.includes("/")) continue;
      const md5 = blob.properties.contentMD5;
      listing.set(blob.name, md5 ? Buffer.from(md5).toString("base64") : undefined);
    }
    return listing;
  },

  async download(names: readonly string[]): Promise<void> {
    await runPool(names, async (name) => {
      await container.getBlobClient(name).downloadToFile(join(snapshotsDir, name));
    });
  },

  async upload(local: FileListing, names: readonly string[]): Promise<void> {
    await runPool(names, async (name) => {
      const md5 = local.get(name);
      await container.getBlockBlobClient(name).uploadFile(join(snapshotsDir, name), {
        blobHTTPHeaders: {
          blobContentType: "image/png",
          // Without this header the blob carries no Content-MD5 and every
          // later diff-mode sync would consider the file "changed".
          blobContentMD5: md5 !== undefined ? Buffer.from(md5, "base64") : undefined,
        },
      });
    });
  },

  async deleteRemote(names: readonly string[]): Promise<void> {
    await runPool(names, async (name) => {
      await container.getBlobClient(name).delete();
    });
  },

  errorHint(error: unknown): string {
    const message = error instanceof Error ? error.message : String(error);
    const statusCode = (error as { statusCode?: number })?.statusCode;
    if (error instanceof Error && error.name === "CredentialUnavailableError") {
      return "Hint: run `az login` first (the Azure CLI login is the credential source).";
    }
    if (statusCode === 403) {
      return "Hint: the signed-in user needs the \"Storage Blob Data Contributor\" role " +
        `on storage account "${vrtConfig.storageAccount}" (being subscription Owner/Contributor is not enough). ` +
        "Note it can take a minute or two after assignment to propagate.";
    }
    if (statusCode === 404) {
      return `Hint: container "${vrtConfig.storageContainer}" was not found on ` +
        `storage account "${vrtConfig.storageAccount}" — check vrt.config.ts.`;
    }
    return `Hint: check the storage settings in vrt.config.ts and your network connection. (${message})`;
  },
};
