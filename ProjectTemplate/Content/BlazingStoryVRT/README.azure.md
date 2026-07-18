# Blazing Story VRT

Visual Regression Testing (VRT) for a [Blazing Story](https://github.com/jsakamoto/BlazingStory) application, built on Playwright's [`toHaveScreenshot()`](https://playwright.dev/docs/test-snapshots). Each story in the running app automatically becomes one test with one screenshot, and the baseline screenshots are shared through Azure Blob Storage.

## Requirements

- Node.js 24 or later
- Azure CLI (`az`) installed and logged in (`az login`). The signed-in user needs the **"Storage Blob Data Contributor"** role on the storage account (subscription Owner/Contributor alone is not enough)

## Getting started

### 1. Configure `vrt.config.ts`

Set the URL of the Blazing Story app under test and the storage connection values:

```typescript
export const vrtConfig = {
  storageAccount: process.env.VRT_STORAGE_ACCOUNT ?? "<storage account name>",
  storageContainer: process.env.VRT_STORAGE_CONTAINER ?? "<blob container name>",
  baseURL: "https://localhost:7117",
};
```

### 2. Install dependencies

```sh
npm install
```

### 3. Run the VRT

The Blazing Story app must be running at the configured URL whenever the tests run.

On the first run no baseline exists yet, so capture the baselines:

```sh
npm test -- --update-snapshots
```

Once the baselines look good, share them through the cloud storage:

```sh
npm run snapshots:push
```

From then on, a plain run compares the current screenshots against the baselines:

```sh
npm test
```

## How it works

- Every test run reads the story index from the running app and generates one test per story. The story list is materialized into `tests/stories.json`; to refresh it without running any tests (e.g. after adding stories), run `npm run stories:gen`.
- Baselines missing locally are downloaded from the cloud storage automatically before every test run, which is convenient on CI or a fresh clone. You can also download them explicitly with `npm run snapshots:pull`.
- After updating baselines (`npm test -- --update-snapshots`), upload them again with `npm run snapshots:push`.

## Tuning

Screenshot diff sensitivity (`maxDiffPixelRatio`), viewport size, and other details are configured in `playwright.config.ts`. For the underlying machinery, see the Playwright snapshot testing docs: <https://playwright.dev/docs/test-snapshots>
