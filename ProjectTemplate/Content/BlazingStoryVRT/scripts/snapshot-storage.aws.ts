// AWS S3 adapter for the VRT baseline sync.
//
// Requirements:
//   - AWS credentials available through the default provider chain
//     (`aws configure`, `aws sso login`, env vars, …) with s3:ListBucket,
//     s3:GetObject, s3:PutObject and s3:DeleteObject on the bucket
//   - storageBucket and storageRegion declared in vrt.config.ts
//
// Caveat on the MD5 diff: the diff sync compares the ETag from ListObjectsV2,
// which equals the content MD5 only for single-part uploads without SSE-KMS.
// This script always uploads single-part, so SSE-S3 (the default encryption)
// is fine, but a bucket defaulting to SSE-KMS makes every file look changed.
// Use SSE-S3 buckets.

import {
  DeleteObjectCommand,
  GetObjectCommand,
  ListObjectsV2Command,
  PutObjectCommand,
  S3Client,
} from "@aws-sdk/client-s3";
import { readFileSync, writeFileSync } from "node:fs";
import { join } from "node:path";
import { vrtConfig } from "../vrt.config.ts";
import { runPool, snapshotsDir, type FileListing, type StorageAdapter } from "./snapshot-sync.ts";

// No network traffic happens until the first actual request.
const client = new S3Client({ region: vrtConfig.storageRegion });
const bucket = vrtConfig.storageBucket;

export const storageAdapter: StorageAdapter = {
  remoteLabel: `s3://${bucket}`,

  async listRemote(): Promise<FileListing> {
    const listing: FileListing = new Map();
    let continuationToken: string | undefined;
    do {
      const page = await client.send(new ListObjectsV2Command({
        Bucket: bucket,
        ContinuationToken: continuationToken,
      }));
      for (const object of page.Contents ?? []) {
        const name = object.Key;
        // Only flat *.png names can be snapshot baselines; ignore anything
        // else that may have been dropped into the bucket.
        if (!name || !name.endsWith(".png") || name.includes("/")) continue;
        // The ETag is the content MD5 (hex, quoted) only for single-part
        // non-KMS uploads, which is what this adapter's own pushes produce.
        // Anything else (multipart, SSE-KMS) maps to undefined and is always
        // treated as "changed".
        const etag = object.ETag?.replaceAll('"', "");
        listing.set(name, etag && /^[0-9a-f]{32}$/.test(etag)
          ? Buffer.from(etag, "hex").toString("base64")
          : undefined);
      }
      continuationToken = page.IsTruncated ? page.NextContinuationToken : undefined;
    } while (continuationToken !== undefined);
    return listing;
  },

  async download(names: readonly string[]): Promise<void> {
    await runPool(names, async (name) => {
      const response = await client.send(new GetObjectCommand({ Bucket: bucket, Key: name }));
      if (response.Body === undefined) throw new Error(`Empty response body for "${name}"`);
      writeFileSync(join(snapshotsDir, name), await response.Body.transformToByteArray());
    });
  },

  async upload(local: FileListing, names: readonly string[]): Promise<void> {
    await runPool(names, async (name) => {
      await client.send(new PutObjectCommand({
        Bucket: bucket,
        Key: name,
        Body: readFileSync(join(snapshotsDir, name)),
        ContentType: "image/png",
        // Makes S3 verify the payload, and (single-part, non-KMS) yields the
        // ETag equal to this MD5 that the diff sync relies on.
        ContentMD5: local.get(name),
      }));
    });
  },

  async deleteRemote(names: readonly string[]): Promise<void> {
    await runPool(names, async (name) => {
      await client.send(new DeleteObjectCommand({ Bucket: bucket, Key: name }));
    });
  },

  errorHint(error: unknown): string {
    const message = error instanceof Error ? error.message : String(error);
    const name = error instanceof Error ? error.name : "";
    const statusCode = (error as { $metadata?: { httpStatusCode?: number } })?.$metadata?.httpStatusCode;
    if (name === "CredentialsProviderError") {
      return "Hint: no AWS credentials found. Run `aws configure` or `aws sso login` first.";
    }
    if (name === "AccessDenied" || statusCode === 403) {
      return `Hint: the AWS identity needs s3:ListBucket/GetObject/PutObject/DeleteObject on bucket "${bucket}".`;
    }
    if (name === "NoSuchBucket" || statusCode === 404) {
      return `Hint: bucket "${bucket}" was not found (region "${vrtConfig.storageRegion}"). Check vrt.config.ts.`;
    }
    return `Hint: check the storage settings in vrt.config.ts and your network connection. (${message})`;
  },
};
