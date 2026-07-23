# Accessibility Report

Blazing Story includes an optional Accessibility Report panel that scans the current story with axe-core.

## Enable it

Set `EnableAccessibilityReport="true"` on `<BlazingStoryApp>`.

```razor
<BlazingStoryApp Assemblies="[typeof(App).Assembly]" EnableAccessibilityReport="true" OnInitialize="OnInitialize">
</BlazingStoryApp>
```

## What it covers

The built-in report runs axe-core against the preview story area and shows:

- accessibility violations that need fixing
- items that pass
- inconclusive items that may need manual review

## Custom rules

If you want to add or change accessibility rules, create a custom addon with your own preview decorator and register it from `OnInitialize`.

The built-in Accessibility Report currently uses axe-core rules configured in its preview decorator, so a custom addon is the right extension point for project-specific rules or checks.

## Notes

- Automated checks are useful, but they do not replace manual keyboard and screen reader testing.
- The panel is intentionally opt-in so apps can choose whether to expose it.
