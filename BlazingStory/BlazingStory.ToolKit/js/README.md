# Why this directory exists

This directory is a **phantom type-resolution stub** for the TypeScript compiler.

## Background

`markdown-page.ts` imports `prism.js` from the `BlazingStory.ToolKit` project, which is
served at runtime via ASP.NET Core's Static Web Assets mechanism:

```
/_content/BlazingStory.ToolKit/js/prism.js
```

Because `markdown-page.js` is itself served from `/_content/BlazingStory/js/`, the
correct runtime path is expressible as a **relative URL**:

```ts
import { Prism } from "../../BlazingStory.ToolKit/js/prism.js";
```

This relative URL resolves correctly in both root (`/`) and subpath (`/foo/`) deployments,
because the browser resolves it relative to the importing module's own URL.

## The problem

TypeScript resolves relative import paths against the **file system**, so it looks for:

```
BlazingStory/BlazingStory.ToolKit/js/prism.d.ts
```

The actual `.d.ts` lives in `BlazingStory.ToolKit/wwwroot/js/`, not here.
TypeScript's `paths` mapping does not apply to relative imports, so it cannot bridge the gap.

## The solution

`prism.d.ts` in this directory is a one-line stub that re-exports all types from the real
declaration file:

```ts
export * from "../../../BlazingStory.ToolKit/wwwroot/js/prism";
```

This gives TypeScript a file to find at the path it expects, with no effect on the
emitted JavaScript — the import statement in the output is unchanged.

**Do not delete this directory.** Removing it breaks TypeScript type checking for
`markdown-page.ts` (error TS2792).
