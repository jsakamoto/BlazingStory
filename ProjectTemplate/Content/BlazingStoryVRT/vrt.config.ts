// Project-specific values, collected in one place so the mechanism code
// (playwright.config.ts, scripts/) stays free of values a user of this
// project may need to edit.
//
// Declarative values only — no environment detection here. (Resolving the
// env-var overrides is the one exception, so that every consumer sees the
// same resolved values.)
export const vrtConfig = {
  //#if (SnapshotsStorage == "aws")
  // AWS S3 bucket holding the shared VRT baseline screenshots.
  // Not secrets: authentication happens through the AWS CLI credentials
  // (`aws configure`).
  storageRegion: process.env.VRT_STORAGE_REGION ?? "AWS_S3_REGION_NAME",
  storageBucket: process.env.VRT_STORAGE_BUCKET ?? "AWS_S3_BUCKET_NAME",

  //#endif
  //#if (SnapshotsStorage == "azure")
  // Azure Blob Storage holding the shared VRT baseline screenshots.
  // Not secrets: authentication happens through the Azure CLI login
  // (`az login` + the "Storage Blob Data Contributor" role).
  storageAccount: process.env.VRT_STORAGE_ACCOUNT ?? "AZURE_STORAGE_ACCOUNT_NAME",
  storageContainer: process.env.VRT_STORAGE_CONTAINER ?? "AZURE_STORAGE_CONTAINER_NAME",

  //#endif
  //#if (SnapshotsStorage == "gcp")
  // Google Cloud Storage bucket holding the shared VRT baseline screenshots.
  // Not a secret: authentication happens through Application Default
  // Credentials (`gcloud auth application-default login`)
  storageBucket: process.env.VRT_STORAGE_BUCKET ?? "GOOGLE_CLOUD_STORAGE_BUCKET_NAME",

  //#endif
  // Where the Blazing Story app under test is running. This is the plain
  // value as seen from the host; rewriting the hostname when the tests run
  // inside a container is playwright.config.ts's responsibility.
  baseURL: "BASE_URL",
};
