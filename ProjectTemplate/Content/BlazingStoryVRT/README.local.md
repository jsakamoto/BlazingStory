# Blazing Story VRT

Visual Regression Testing (VRT) for a [Blazing Story](https://github.com/jsakamoto/BlazingStory) application, built on Playwright's [`toHaveScreenshot()`](https://playwright.dev/docs/test-snapshots). Each story in the running app automatically becomes one test with one screenshot, and the baseline screenshots are kept on the local disk only.

> [!NOTE]
> In practice, team-level VRT usually stores the baseline screenshots in a shared cloud storage, so that every developer and every CI run compares against the same baselines. This local-only setup is an option for trying VRT out first.

## Requirements

- Node.js 24 or later

## Getting started

### 1. Configure `vrt.config.ts`

Set the URL of the Blazing Story app under test:

```typescript
export const vrtConfig = {
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
npm run snapshots:update
```

From then on, a plain run compares the current screenshots against the baselines:

```sh
npm test
```

## How it works

- Every test run reads the story index from the running app and generates one test per story. The story list is materialized into `tests/stories.json`; to refresh it without running any tests (e.g. after adding stories), run `npm run stories:gen`.
- The baselines live in `tests/vrt.spec.ts-snapshots/` on this machine only. To accept an intentional UI change as the new baseline, run `npm run snapshots:update` again.

## Tuning

Screenshot diff sensitivity (`maxDiffPixelRatio`), viewport size, and other details are configured in `playwright.config.ts`. For the underlying machinery, see the Playwright snapshot testing docs: <https://playwright.dev/docs/test-snapshots>
