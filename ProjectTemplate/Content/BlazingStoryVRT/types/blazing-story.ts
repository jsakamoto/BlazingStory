// Type-only module: Blazing Story's story index shapes and the ambient
// declaration of the BlazingStory browser global. No runtime code.

// The story index Blazing Story exposes (equivalent to Storybook's index.json).
export interface StoryIndexEntry {
  id: string;
  title: string;
  name: string;
  type: "story" | "docs";
}

export interface StoryIndex {
  v: number;
  entries: Record<string, StoryIndexEntry>;
}

// The global variable Blazing Story exposes inside the browser, referenced
// from page.evaluate / waitForFunction callbacks without an import.
declare global {
  const BlazingStory: {
    getStoryIndex(): Promise<StoryIndex>;
    readyView(): Promise<void>;
  };
}
