const ensureAxeRuntimeModuleLoaded = async () => {
    const path = "../../js/axe.min.js";
    await import(path);
};
export const run = async () => {
    await ensureAxeRuntimeModuleLoaded();
    axe.reset();
    axe.configure({
        rules: [
            {
                id: "region",
                enabled: false,
            }
        ]
    });
    const include = document.querySelectorAll(".preview-story-area");
    const result = await axe.run({ include, exclude: [] });
    const flattenTarget = (results) => {
        return results.map(result => {
            return {
                ...result,
                nodes: result.nodes.map(node => ({
                    ...node,
                    target: node.target.flat()
                }))
            };
        });
    };
    const resultText = JSON.stringify({
        violations: flattenTarget(result.violations),
        passes: flattenTarget(result.passes),
        incomplete: flattenTarget(result.incomplete)
    });
    const blob = new Blob([resultText], { type: 'text/plain' });
    return URL.createObjectURL(blob);
};
