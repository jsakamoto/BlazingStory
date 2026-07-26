import { defineConfig } from "@playwright/test";
import { existsSync } from "node:fs";
import { vrtConfig } from "./vrt.config.ts";

const baseURL = vrtConfig.baseURL;

export default defineConfig({
  testDir: "./tests",
  //#if (SnapshotsStorage != "none")
  // Before every test run: download the baselines missing locally, then
  // regenerate tests/stories.json from the running app so the per-story test
  // registration always reflects the current story set. (Bare test listing /
  // Test Explorer discovery skips it and reads the existing file.)
  //#else
  // Before every test run: regenerate tests/stories.json from the running app
  // so the per-story test registration always reflects the current story set.
  // (Bare test listing / Test Explorer discovery skips it and reads the
  // existing file.)
  //#endif
  globalSetup: "./scripts/global-setup.ts",
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: 0,
  workers: 4,
  reporter: [["html", { open: "never" }]],
  use: {
    baseURL: existsSync("/.dockerenv") || existsSync("/run/.containerenv") ?
      baseURL.replace("localhost", "host.docker.internal") :
      baseURL,
    ignoreHTTPSErrors: true,
    viewport: { width: 900, height: 600 },
  },
  expect: {
    toHaveScreenshot: {
      maxDiffPixelRatio: 0.001,
    },
  },
});
