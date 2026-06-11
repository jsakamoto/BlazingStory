# Performance Panel Feasibility for Blazor WebAssembly

**Related issue:** [#91 - [Feature Request] Performance Panel](https://github.com/jsakamoto/BlazingStory/issues/91)

## Overview

This document evaluates the feasibility of implementing a performance monitoring panel for Blazing Story, inspired by `@github-ui/storybook-addon-performance-panel`. For each metric category that the addon covers, this document assesses:

1. **Meaningfulness** — Does this metric make sense in the context of Blazor WebAssembly components?
2. **Measurability** — Can this metric be collected in a Blazor WebAssembly environment, and if so, how?

---

## Metric Categories

### 1. Frame Timing

**Metrics:** FPS, Frame Time, Dropped Frames, Frame Jitter, Frame Stability

**Collection method:** `requestAnimationFrame` loop

| | Assessment |
|---|---|
| **Meaningful?** | ✅ Yes |
| **Measurable?** | ✅ Yes — fully applicable |

**Details:**

Frame timing metrics are browser-level and agnostic to the front-end framework. Because Blazor WebAssembly runs its rendering on the browser's main thread (for most .NET configurations), Blazor component re-renders directly cause frame-rate drops that `requestAnimationFrame`-based monitoring will capture faithfully.

`requestAnimationFrame` is a standard Web API accessible via Blazor's JavaScript interop. No changes to Blazor components are required — a monitoring script attached to the preview iframe will suffice.

**Key note:** In .NET 8+ experimental WASM multithreading, component rendering can be scheduled off the main thread, which may cause rAF-based FPS metrics to show fewer drops while the total work remains the same. This is an edge case that applies to a small fraction of configurations today.

---

### 2. Input Responsiveness

**Metrics:** Input Latency, Paint Time, INP (Interaction to Next Paint), FID (First Input Delay), Last Interaction, Slowest Interaction

**Collection method:** Event Timing API (`PerformanceObserver`), double-RAF paint technique

| | Assessment |
|---|---|
| **Meaningful?** | ✅ Yes — highly relevant |
| **Measurable?** | ⚠️ Partially — measurable but requires interpretation |

**Details:**

These metrics are meaningful because the browser records them before any framework processes the event. The `PerformanceObserver` with `event` entry type captures total interaction duration end-to-end, which — for Blazor — includes the following pipeline:

```
Browser event → JS dispatch → WASM boundary crossing → .NET event handler
  → Blazor diff computation → JS interop for DOM patches → Browser paint
```

The total duration is directly comparable to the INP/FID thresholds defined by Web Vitals. Standard collection techniques therefore work unmodified.

**Key nuance:** The "processing time" breakdown reported by the Event Timing API encompasses WASM execution time. Blazor's WASM startup costs and JIT warm-up (for .NET 9 WASM JIT) can inflate the first few interactions. For subsequent interactions, processing time reflects the true cost of the event handler plus Blazor's diffing algorithm.

FID and INP are fully measurable via the standard `PerformanceObserver` API.

---

### 3. Main Thread Health

**Metrics:** Long Tasks, Total Blocking Time (TBT), Thrashing, DOM Churn

**Collection method:** Long Tasks API (`PerformanceObserver`), `MutationObserver`, property getter instrumentation

| | Assessment |
|---|---|
| **Meaningful?** | ✅ Yes |
| **Measurable?** | ✅ Yes — fully applicable |

**Details:**

- **Long Tasks & TBT:** Blazor WebAssembly executes synchronously on the main thread during renders and event handling. These operations will appear as Long Tasks (>50 ms) when components are heavy. Measuring Long Tasks via `PerformanceObserver` requires no framework-specific changes and accurately attributes blocking time to Blazor operations.

- **DOM Churn:** Blazor's virtual DOM diffing algorithm emits batched DOM mutations. `MutationObserver` captures these mutations without any special support. A high DOM churn rate in Blazor often indicates unnecessary re-renders triggered by excessive `StateHasChanged()` calls.

- **Thrashing (forced sync layout):** Property getter instrumentation to detect `offsetWidth`/`getBoundingClientRect()` calls after style writes works at the browser JS layer and therefore detects thrashing caused by Blazor JavaScript interop calls.

---

### 4. Long Animation Frames (LoAF)

**Metrics:** LoAF Count, Blocking Duration, P95 Duration, Script Attribution

**Collection method:** Long Animation Frames API (`PerformanceObserver`, Chrome 123+)

| | Assessment |
|---|---|
| **Meaningful?** | ✅ Yes |
| **Measurable?** | ✅ Yes (Chrome/Edge 123+) |

**Details:**

LoAF is a browser-level API that is framework-agnostic. It captures any animation frame exceeding 50 ms, regardless of what caused it.

The **Script Attribution** feature is particularly valuable for Blazor because it identifies which script caused a long frame. Blazor WASM operations are typically attributed to `dotnet.js`, `blazor.webassembly.js`, or dynamically loaded WASM modules. This allows developers to distinguish Blazor-caused long frames from those caused by third-party scripts.

---

### 5. Element Timing

**Metrics:** Element Count, Largest Render Time, Individual Element Render Times

**Collection method:** `elementtiming` attribute + `PerformanceObserver`

| | Assessment |
|---|---|
| **Meaningful?** | ✅ Yes |
| **Measurable?** | ✅ Yes — fully applicable |

**Details:**

Adding `elementtiming="identifier"` to HTML elements in Razor component markup is straightforward. Blazor does not interfere with custom HTML attributes. A monitoring script in the iframe will observe these entries via `PerformanceObserver` with the `element` entry type, just as it would for any HTML page.

This is particularly useful for measuring how long Blazor components take to render their key visual elements (e.g., images, data grids) after initial mount.

---

### 6. Layout Stability

**Metrics:** CLS (Cumulative Layout Shift), Forced Reflows, Style Writes

**Collection method:** Layout Instability API, `MutationObserver`, property getter instrumentation

| | Assessment |
|---|---|
| **Meaningful?** | ✅ Yes |
| **Measurable?** | ✅ Yes — fully applicable |

**Details:**

- **CLS:** Blazor components that load data asynchronously and update the DOM can cause layout shifts. The Layout Instability API (`PerformanceObserver` with `layout-shift` entry type) captures these at the browser level, independently of Blazor.

- **Forced Reflows:** When Blazor components call JavaScript interop functions that read layout properties (e.g., `getBoundingClientRect()`) after style changes, the browser is forced to perform synchronous layout recalculation. Property getter instrumentation in the monitoring script will detect these patterns.

- **Style Writes:** `MutationObserver` on `style` attribute mutations works on Blazor-generated DOM.

---

### 7. React Performance → Blazor Component Lifecycle Performance

**Metrics (React):** Mount Count/Duration, Slow Updates, P95 Duration, Render Cascades

**Collection method:** React Profiler API (`<Profiler>` component)

| | Assessment |
|---|---|
| **Meaningful?** | ✅ Yes — Blazor has an equivalent concept |
| **Measurable?** | ⚠️ Yes, but requires Blazor-specific implementation |

**Details:**

The React Profiler API has **no equivalent in Blazor**. React's `<Profiler>` component wraps a subtree and records commit-phase timing; Blazor does not expose a comparable profiling hook via a public API.

However, the **underlying metrics are equally meaningful** for Blazor components:

| React Metric | Blazor Equivalent | How to Measure |
|---|---|---|
| Mount Duration | `OnInitialized` / `OnInitializedAsync` duration | Instrument via `Stopwatch` in a base class or wrapper component |
| Slow Updates | `OnParametersSet` / render duration > 16 ms | Same Stopwatch approach |
| P95 Duration | P95 of all `SetParametersAsync` call durations | Collect via .NET-side instrumentation |
| Render Cascades | Cascading `StateHasChanged()` calls | Count `ShouldRender()` + `BuildRenderTree()` invocations |

**Implementation path:** Blazor supports custom component base classes (`ComponentBase` subclass). A `ProfiledComponentBase` that wraps lifecycle methods in `Stopwatch` measurements and reports them via JS interop to the monitoring panel is technically feasible. Alternatively, Blazor's `RenderTreeBuilder` or middleware-style render delegate wrapping could be explored.

This is the metric category that requires the most Blazor-specific development effort.

---

### 8. Memory & Resources

**Metrics:** Heap Usage, Memory Delta, GC Pressure, Compositor Layers

**Collection method:** `performance.memory` (Chrome-only), Chrome DevTools Protocol

| | Assessment |
|---|---|
| **Meaningful?** | ⚠️ Partially — requires supplementation |
| **Measurable?** | ⚠️ Partially — requires dual-layer monitoring |

**Details:**

This is the metric category with the most important Blazor-specific difference:

**JS Heap (`performance.memory`):** This Chrome-only API reports the JavaScript heap. For a Blazor WebAssembly app, **the .NET managed heap lives inside the WebAssembly linear memory, not the JS heap**. This means `performance.memory` will show a relatively small and stable JS heap while the actual memory consumed by Blazor objects is invisible to it.

To get a complete memory picture for Blazor:

| Layer | API | What It Shows |
|---|---|---|
| JS Heap | `performance.memory.usedJSHeapSize` | JS objects (Blazor interop proxies, etc.) |
| WASM Linear Memory (total) | `WebAssembly.Memory.buffer.byteLength` | Total pages allocated to WASM runtime |
| .NET Managed Heap | `GC.GetTotalMemory()` (C# code) | Actual .NET object allocations |
| .NET GC Info | `GC.GetGCMemoryInfo()` (C# code) | GC generation sizes, fragmentation, pause info |

For a practical implementation, `GC.GetTotalMemory()` called periodically via JS interop and reported back to the panel would give the most actionable metric. The JS interop call overhead is small enough to not distort measurements significantly.

**GC Pressure:** The .NET GC running in WASM does compact memory. `GC.GetGCMemoryInfo().PauseTimePercentage` and `GC.CollectionCount()` are available from .NET code and can be exposed via interop. GC pauses in Blazor WASM are particularly impactful because they block the main thread.

**Compositor Layers:** This is a pure browser metric and works identically to any other web app.

---

## Summary Table

| Metric Category | Meaningful? | Measurable? | Notes |
|---|---|---|---|
| Frame Timing (FPS, Frame Time, etc.) | ✅ Yes | ✅ Yes | Standard browser APIs, no changes needed |
| Input Responsiveness (INP, FID, etc.) | ✅ Yes | ✅ Yes | Event Timing API works; WASM processing time is included in INP |
| Main Thread Health (Long Tasks, TBT) | ✅ Yes | ✅ Yes | Blazor WASM ops appear as Long Tasks |
| Long Animation Frames (LoAF) | ✅ Yes | ✅ Yes (Chrome 123+) | Script attribution identifies Blazor-caused frames |
| Element Timing | ✅ Yes | ✅ Yes | `elementtiming` attribute works in Razor markup |
| Layout Stability (CLS, Forced Reflows) | ✅ Yes | ✅ Yes | Standard APIs apply to Blazor DOM |
| Component Lifecycle Performance | ✅ Yes | ⚠️ Custom impl. needed | No React Profiler equivalent; requires `ComponentBase` instrumentation |
| Memory & Resources | ⚠️ Partial | ⚠️ Dual-layer needed | JS heap ≠ .NET heap; `GC.*` APIs needed for true picture |

---

## Blazor-Specific Additional Metrics

Beyond the metrics in `@github-ui/storybook-addon-performance-panel`, a Blazing Story performance panel could uniquely offer:

| Metric | Source | Value |
|---|---|---|
| JS→WASM interop call count/duration | Instrumented interop wrapper | Identifies interop overhead |
| .NET GC pause duration | `GC.GetGCMemoryInfo()` | Identifies GC-caused frame drops |
| `StateHasChanged()` call frequency | Custom `ComponentBase` | Identifies unnecessary re-render triggers |
| Component mount/update durations | Custom `ComponentBase` + `Stopwatch` | Direct equivalent to React Profiler |
| Render batch size (components per commit) | Blazor render batch tracking | Identifies cascading render chains |

---

## Implementation Feasibility Conclusion

**Overall verdict: Feasible**, with the following considerations:

1. **Browser-level metrics** (Frame Timing, Input Responsiveness, Main Thread Health, LoAF, Element Timing, Layout Stability) can be collected using exactly the same browser APIs as `@github-ui/storybook-addon-performance-panel`. A JavaScript monitoring script injected into the preview iframe will work without any Blazor-specific changes.

2. **Component lifecycle metrics** (equivalent to React Performance section) require a Blazor-specific implementation. A `ProfiledComponentBase` class that instruments lifecycle methods and reports timing via JS interop is technically straightforward and does not require modifying Blazor internals.

3. **Memory metrics** require dual-layer monitoring: the standard `performance.memory` API for the JS heap, supplemented by `GC.GetTotalMemory()` called from .NET via a periodic timer and reported via JS interop.

4. The **JS/WASM boundary** is a source of overhead not present in pure JavaScript frameworks. Tracking interop call counts and durations is unique to Blazor and would provide significant diagnostic value that goes beyond what the React-based addon can offer.
