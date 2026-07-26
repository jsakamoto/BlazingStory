# Blazing Story VRT

Visual Regression Testing (VRT) for a [Blazing Story](https://github.com/jsakamoto/BlazingStory) application, built on Playwright's [`toHaveScreenshot()`](https://playwright.dev/docs/test-snapshots). Each story in the running app automatically becomes one test with one screenshot, and the baseline screenshots are kept on the local disk only.

> [!TIP]
> The full guide, from creating this project to running it on CI/CD, is at
> <https://blazingstory.github.io/docs/visual-regression-testing>

> [!NOTE]
> In practice, team-level VRT usually stores the baseline screenshots in a shared cloud storage, so that every developer and every CI run compares against the same baselines. This local-only setup is an option for trying VRT out first.

## Running in a Dev Container (recommended)

Screenshots can vary slightly between machines and operating systems, so for stable, reproducible snapshots we recommend always running the VRT in the same container. For that purpose, this project ships a Dev Container configuration file (`.devcontainer/devcontainer.json`). If you use VS Code, just "Reopen in Container" for this folder, and you can run the VRT in a consistent container environment right away.

## Requirements

- Node.js 24 or later

## Configuration (`vrt.config.ts`)

`vrt.config.ts` collects everything you may need to edit: the URL of the Blazing Story app under test.

```typescript
export const vrtConfig = {
  baseURL: process.env.VRT_BASE_URL ?? "<app URL, e.g. http://localhost:5000>",
};
```

This is a required option when this project is created, so the file is already filled in with the value you gave, and the VRT is ready to run as-is. Edit this one file whenever it is wrong (for example, if you typed a placeholder at creation time) or changes later.

The `VRT_BASE_URL` environment variable, when set, takes precedence over the value written in the file, which is handy on CI and when one machine needs a different app URL. It can also be written into a `.env` file at the project root: `vrt.config.ts` loads it automatically, and `.gitignore` keeps it out of the repository. A variable already set in the shell still wins over `.env`.

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

From then on, a plain run compares the current screenshots against the baselines:

```sh
npm test
```

## How it works

- Every test run reads the story index from the running app and generates one test per story. The story list is materialized into `tests/stories.json`; to refresh it without running any tests (e.g. after adding stories), run `npm run stories:gen`.
- The baselines live in `tests/vrt.spec.ts-snapshots/` on this machine only. To accept an intentional UI change as the new baseline, run `npm run snapshots:update` again. Each file is named `<story id>-<platform>.png`, so a screenshot is only ever compared against a baseline captured on the same platform.
- Every run writes an HTML report to `playwright-report/index.html`. When pixels differ, open it in a browser to compare the baseline, the new screenshot, and the diff. `npm run test:open` runs the tests and opens it in one step.

## Tuning

Screenshot diff sensitivity (`maxDiffPixelRatio`), viewport size, and other details are configured in `playwright.config.ts`. For the underlying machinery, see the Playwright snapshot testing docs: <https://playwright.dev/docs/test-snapshots>
