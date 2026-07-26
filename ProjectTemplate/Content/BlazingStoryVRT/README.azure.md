# Blazing Story VRT

Visual Regression Testing (VRT) for a [Blazing Story](https://github.com/jsakamoto/BlazingStory) application, built on Playwright's [`toHaveScreenshot()`](https://playwright.dev/docs/test-snapshots). Each story in the running app automatically becomes one test with one screenshot, and the baseline screenshots are shared through Azure Blob Storage.

## Running in a Dev Container (recommended)

Screenshots can vary slightly between machines and operating systems, so for stable, reproducible snapshots we recommend always running the VRT in the same container. For that purpose, this project ships a Dev Container configuration file (`.devcontainer/devcontainer.json`). If you use VS Code, just "Reopen in Container" for this folder, and you can run the VRT in a consistent container environment right away. This Dev Container also comes with the Azure CLI (`az`) pre-installed.

## Requirements

- Node.js 24 or later
- Azure CLI (`az`) installed and logged in (`az login`). The signed-in user needs the **"Storage Blob Data Contributor"** role on the storage account (subscription Owner/Contributor alone is not enough)

## Configuration (`vrt.config.ts`)

`vrt.config.ts` collects everything you may need to edit: the URL of the Blazing Story app under test, and the storage connection values.

```typescript
export const vrtConfig = {
  storageAccount: process.env.VRT_STORAGE_ACCOUNT ?? "<storage account name>",
  storageContainer: process.env.VRT_STORAGE_CONTAINER ?? "<blob container name>",
  baseURL: process.env.VRT_BASE_URL ?? "<app URL, e.g. http://localhost:5000>",
};
```

These are required options when this project is created, so the file is already filled in with the values you gave, and the VRT is ready to run as-is. Edit this one file whenever any of them is wrong (for example, if you typed a placeholder at creation time) or changes later.

The `VRT_STORAGE_ACCOUNT`, `VRT_STORAGE_CONTAINER`, and `VRT_BASE_URL` environment variables, when set, take precedence over the values written in the file, which is handy on CI and when one machine needs a different app URL. They can also be written into a `.env` file at the project root: `vrt.config.ts` loads it automatically, and `.gitignore` keeps it out of the repository. A variable already set in the shell still wins over `.env`.

## Getting started

### 1. Install dependencies

```sh
npm install
```

### 2. Run the VRT

The Blazing Story app must be running at the configured URL whenever the tests run.

On the first run no baseline exists yet, so capture the baselines:

```sh
npm run snapshots:update
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
- After updating baselines (`npm run snapshots:update`), upload them again with `npm run snapshots:push`.

## Tuning

Screenshot diff sensitivity (`maxDiffPixelRatio`), viewport size, and other details are configured in `playwright.config.ts`. For the underlying machinery, see the Playwright snapshot testing docs: <https://playwright.dev/docs/test-snapshots>
