# Blazing Story VRT

Visual Regression Testing (VRT) for a [Blazing Story](https://github.com/jsakamoto/BlazingStory) application, built on Playwright's [`toHaveScreenshot()`](https://playwright.dev/docs/test-snapshots). Each story in the running app automatically becomes one test with one screenshot, and the baseline screenshots are shared through a storage service of your own choice.

## Running in a Dev Container (recommended)

Screenshots can vary slightly between machines and operating systems, so for stable, reproducible snapshots we recommend always running the VRT in the same container. For that purpose, this project ships a Dev Container configuration file (`.devcontainer/devcontainer.json`). If you use VS Code, just "Reopen in Container" for this folder, and you can run the VRT in a consistent container environment right away.

## Requirements

- Node.js 24 or later

## Configuration (`vrt.config.ts`)

`vrt.config.ts` collects everything you may need to edit: the URL of the Blazing Story app under test, and the storage connection values.

```typescript
export const vrtConfig = {
  // ... whatever connection values your storage adapter reads (see step 1 below)
  baseURL: process.env.VRT_BASE_URL ?? "<app URL, e.g. http://localhost:5000>",
};
```

The app URL is a required option when this project is created, so it is already filled in with the value you gave. Edit this one file whenever it is wrong (for example, if you typed a placeholder at creation time) or changes later. The connection values are up to your storage adapter, so they are the part you add yourself in step 1.

The app URL already shows the pattern to copy: read the environment variable first (`VRT_BASE_URL` here), and fall back to the literal in the file. Do the same for every connection value your adapter needs. Those variables can be written into a `.env` file at the project root: `vrt.config.ts` loads it automatically, and `.gitignore` keeps it out of the repository. A variable already set in the shell still wins over `.env`. Anything secret, such as a password or an access key, belongs in `.env` and must never be written into `vrt.config.ts`, which is committed.

## Getting started

### 1. Implement the storage adapter

`scripts/snapshot-storage.ts` ships as an empty template. Implement its members (each one is described by a comment in the file) so the sync can list, download, upload, and delete the baseline files on your storage service. Putting the connection values in `vrt.config.ts` rather than hardcoding them in the adapter is recommended, because it keeps everything a user of this project has to edit in one file.

If you picked an AI coding agent when you created this project, you don't have to write this by hand. A **storage-adapter skill** was installed for that agent, under `.claude/skills/` for Claude Code, or `.agents/skills/` for GitHub Copilot, OpenAI Codex, Cursor, and other agents. Just tell your agent which service you want to use (Dropbox, MinIO, WebDAV, any S3-compatible storage, and so on) and ask it to implement the storage adapter; it will follow the skill to fill in `scripts/snapshot-storage.ts` and add the matching connection values to `vrt.config.ts`.

### 2. Install dependencies

```sh
npm install
```

### 3. Run the VRT

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
- Baselines missing locally are downloaded from the storage automatically before every test run, which is convenient on CI or a fresh clone. You can also download them explicitly with `npm run snapshots:pull`.
- After updating baselines (`npm run snapshots:update`), upload them again with `npm run snapshots:push`.

## Tuning

Screenshot diff sensitivity (`maxDiffPixelRatio`), viewport size, and other details are configured in `playwright.config.ts`. For the underlying machinery, see the Playwright snapshot testing docs: <https://playwright.dev/docs/test-snapshots>
