import type * as AxeModule from "@blazingstory/types/axe";
declare global { var axe: typeof AxeModule; }

const ensureAxeRuntimeModuleLoaded = async () => {
    const path = "../../js/axe.min.js";
    await import(path);
}

export const run = async () => {

    await ensureAxeRuntimeModuleLoaded();
    axe.reset();
    axe.configure({
        rules: [
            {
                id: "region",
                enabled: false,
            }]
    });
    const include = document.querySelectorAll(".preview-story-area");
    const result = await axe.run({ include, exclude: [] });

    return JSON.stringify({
        violations: result.violations,
        passes: result.passes,
        incomplete: result.incomplete
    });
}
