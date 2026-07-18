import { test, expect } from "@playwright/test";
import { readFileSync } from "node:fs";
import { join } from "node:path";
import type { StoryIndexEntry } from "../types/blazing-story.js";

// The story list is generated ahead of time by scripts/gen-stories.ts (run
// from Playwright's globalSetup), which drives a browser to read
// BlazingStory.getStoryIndex() and writes tests/stories.json.
//
// It's read synchronously here so each story can be registered as its own
// test() at collection time, which is what makes the individual stories show
// up in the VS Code Test Explorer. (The index can't be fetched here directly:
// it requires a running browser, but test collection is synchronous.)
function loadStories(): StoryIndexEntry[] {
  try {
    const json = readFileSync(join(import.meta.dirname, "stories.json"), "utf8");
    return JSON.parse(json) as StoryIndexEntry[];
  } catch {
    // stories.json hasn't been generated yet — globalSetup writes it on
    // the first test run (the app must be running).
    return [];
  }
}

const stories = loadStories().filter((e) => e.type === "story");

for (const story of stories) {
  test(`${story.title} - ${story.name}`, async ({ page }) => {
    await page.goto(
      `/iframe.html?id=${encodeURIComponent(story.id)}&viewMode=story`
    );

    await page.waitForFunction(() => typeof BlazingStory !== "undefined");
    await page.evaluate(() => BlazingStory.readyView());
    const root = page.locator(".preview-story-area");
    await expect(root).toHaveScreenshot(`${story.id}.png`);
  });
}
