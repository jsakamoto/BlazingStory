# Blazing Story VRT

Visual Regression Testing (VRT) for a [Blazing Story](https://github.com/jsakamoto/BlazingStory) application, built on Playwright's [`toHaveScreenshot()`](https://playwright.dev/docs/test-snapshots). Each story in the running app automatically becomes one test with one screenshot, and the baseline screenshots are shared through a storage service of your own choice.

## Requirements

- Node.js 24 or later

## Getting started

### 1. Implement the storage adapter

`scripts/snapshot-storage.ts` ships as an empty template. Implement its members (each one is described by a comment in the file) so the sync can list, download, upload, and delete the baseline files on your storage service. Putting the connection values in `vrt.config.ts` rather than hardcoding them in the adapter is recommended, because it keeps everything a user of this project has to edit in one file.

### 2. Configure `vrt.config.ts`

Set the URL of the Blazing Story app under test and the storage connection values:

```typescript
export const vrtConfig = {
  // ... whatever connection values your storage adapter reads (see step 1)
  baseURL: "https://localhost:7117",
};
```

### 3. Install dependencies

```sh
npm install
```

### 4. Run the VRT

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
