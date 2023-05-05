// This JavaScript file is loaded automatically before the Blazor runtime starts.
// See alos the "JavaScript initializers" section: https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/startup#javascript-initializers

export const beforeStart = () => {

    // Expose the "BlazingStory.isOnLine()" function to the window object.
    // This function is used by the "UrlParameterKit.GetUpdateToken()" method to check if the browser is online.
    (window as any).BlazingStory = { isOnLine: () => navigator.onLine }
}