// Values a user of this project may need to edit, collected in one place so
// the mechanism code (playwright.config.ts, scripts/) stays free of them.
// Declarative only: no environment detection here, except resolving the
// env-var overrides so that every consumer sees the same values.

// Every entry point reaches this file, so loading the optional .env here is
// what makes it apply to both `npm test` and the snapshots CLIs. Variables
// already set in the shell win; .env only fills in what is missing. The file
// is git-ignored, which is why secrets belong there and never in this file.
try {
  process.loadEnvFile();
} catch {
  // No .env file, which is the normal case.
}

export const vrtConfig = {
  //#if (SnapshotsStorage == "aws")
  // AWS S3 bucket holding the shared VRT baseline screenshots. Not secrets:
  // authentication goes through the AWS CLI credentials (`aws configure`).
  storageRegion: process.env.VRT_STORAGE_REGION ?? "AWS_S3_REGION_NAME",
  storageBucket: process.env.VRT_STORAGE_BUCKET ?? "AWS_S3_BUCKET_NAME",

  //#endif
  //#if (SnapshotsStorage == "azure")
  // Azure Blob Storage holding the shared VRT baseline screenshots. Not
  // secrets: authentication goes through the Azure CLI login (`az login`
  // plus the "Storage Blob Data Contributor" role).
  storageAccount: process.env.VRT_STORAGE_ACCOUNT ?? "AZURE_STORAGE_ACCOUNT_NAME",
  storageContainer: process.env.VRT_STORAGE_CONTAINER ?? "AZURE_STORAGE_CONTAINER_NAME",

  //#endif
  //#if (SnapshotsStorage == "gcp")
  // Google Cloud Storage bucket holding the shared VRT baseline screenshots.
  // Not a secret: authentication goes through Application Default Credentials
  // (`gcloud auth application-default login`).
  storageBucket: process.env.VRT_STORAGE_BUCKET ?? "GOOGLE_CLOUD_STORAGE_BUCKET_NAME",

  //#endif
  // Where the Blazing Story app under test is running, as seen from the host.
  // Rewriting the hostname for in-container runs is playwright.config.ts's job.
  baseURL: process.env.VRT_BASE_URL ?? "STORY_APP_BASE_URL",
};
