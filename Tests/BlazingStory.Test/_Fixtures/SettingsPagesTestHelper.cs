using BlazingStory.Addons.Internals;
using BlazingStory.Configurations;
using BlazingStory.Internals.Pages.Layouts;
using BlazingStory.Internals.Pages.Settings;
using BlazingStory.Internals.Services.Command;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace BlazingStory.Test._Fixtures;

internal static class SettingsPagesTestHelper
{
    /// <summary>
    /// Creates a bUnit context whose addons and commands are configured in the same way as <c>BlazingStoryApp</c> and <c>MainLayout</c> do.
    /// </summary>
    /// <param name="onInitialize">A callback to register an app's own addons, like the <c>OnInitialize</c> parameter of <c>BlazingStoryApp</c>.</param>
    /// <param name="savedCommandStates">The JSON text of the command states saved in the local storage.</param>
    internal static async ValueTask<BunitContext> CreateContextAsync(Action<IBlazingStoryBuilder>? onInitialize = null, string? savedCommandStates = null)
    {
        var addonStore = new AddonStore();
        addonStore.Register<SettingsPagesAddon>();
        onInitialize?.Invoke(new BlazingStoryBuilder(new ServiceCollection(), addonStore, new()));

        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        if (savedCommandStates is not null)
        {
            ctx.JSInterop.Setup<string?>("localStorage.getItem", "CommandService.Commands").SetResult(savedCommandStates);
        }
        ctx.Services.AddSingleton<IJSRuntime>(new BunitJSRuntimeWrapper(ctx.JSInterop.JSRuntime));
        ctx.Services.AddHotKeys2();
        ctx.Services.AddScoped<CommandService>();
        ctx.Services.AddScoped<AddonManager>();

        ctx.Services.GetRequiredService<AddonManager>().Initialize(addonStore);
        await MainLayout.ConfigureCommandsAsync(ctx.Services);

        ctx.RenderTree.Add<CascadingValue<IServiceProvider>>(p => p.Add(p => p.Value, ctx.Services));
        return ctx;
    }
}
