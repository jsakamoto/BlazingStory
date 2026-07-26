// Story index generator: drives a browser to read Blazing Story's story index
// (window.BlazingStory.getStoryIndex()) and writes tests/stories.json.
//
// Called from global-setup.ts (Playwright's globalSetup), so every test run
// refreshes the index before test files are collected. The VRT spec
// (tests/vrt.spec.ts) then reads that JSON *synchronously* at collection time
// to register one test() per story, which is what lists the stories
// individually in the VS Code Test Explorer.
import { chromium, type FullConfig } from "@playwright/test";
import { writeFileSync } from "node:fs";
import { join } from "node:path";

export default async function generateStoryIndex(config: FullConfig): Promise<void> {
  const baseURL = config.projects[0].use.baseURL;

  const browser = await chromium.launch();
  try {
    const page = await browser.newPage({ ignoreHTTPSErrors: true });
    await page.goto(new URL("/", baseURL).href);
    await page.waitForFunction(() => typeof BlazingStory !== "undefined");
    const index = await page.evaluate(() => BlazingStory.getStoryIndex());

    const stories = Object.values(index.entries)
      .filter((e) => e.type === "story")
      .map(({ id, title, name, type }) => ({ id, title, name, type }));

    const out = join(import.meta.dirname, "..", "tests", "stories.json");
    writeFileSync(out, JSON.stringify(stories, null, 2) + "\n");
    console.log(`Wrote ${stories.length} stories to ${out}`);
  } finally {
    await browser.close();
  }
}
