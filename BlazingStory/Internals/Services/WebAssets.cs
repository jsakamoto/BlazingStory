using System.Diagnostics.CodeAnalysis;
using BlazingStory.Internals.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Services;

internal class WebAssets
{
    private readonly IServiceProvider _Services;

    private IFileProvider? _FileProvider;

    public WebAssets(IServiceProvider services)
    {
        this._Services = services;
    }

    public ValueTask<string> GetStringAsync(string path)
    {
        if (!OperatingSystem.IsBrowser()) return this.GetStringOnServerAsync(path);
        return this.GetStringOnWebAssemblyAsync(path);
    }

    private async ValueTask<string> GetStringOnWebAssemblyAsync(string path)
    {
        var httpClient = this._Services.GetRequiredService<HttpClient>();
        var jsRuntime = this._Services.GetRequiredService<IJSRuntime>();
        var updateToken = UriParameterKit.GetUpdateToken(jsRuntime);
        return await httpClient.GetStringAsync(path + updateToken);
    }

    private async ValueTask<string> GetStringOnServerAsync(string path)
    {
        var fileProvider = this.GetFileProvider();
        var fileInfo = fileProvider.GetFileInfo(path);
        using var fileStream = fileInfo.CreateReadStream();
        using var fileReader = new StreamReader(fileStream);
        return await fileReader.ReadToEndAsync();
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    [UnconditionalSuppressMessage("Trimming", "IL2075")]
    private IFileProvider GetFileProvider()
    {
        if (this._FileProvider == null)
        {
            var globalServices = this._Services.GetRequiredService<GlobalServiceProvider>();
            var hostingAbstractions = AppDomain.CurrentDomain.GetAssemblies().First(asm => asm.GetName().Name == "Microsoft.AspNetCore.Hosting.Abstractions");
            var typeOfIWebHostEnv = hostingAbstractions.GetType("Microsoft.AspNetCore.Hosting.IWebHostEnvironment");
            var webHostEnv = globalServices.GetService(typeOfIWebHostEnv!);
            var propOfWebRootFileProvider = typeOfIWebHostEnv?.GetProperty("WebRootFileProvider");
            this._FileProvider = propOfWebRootFileProvider?.GetValue(webHostEnv, null) as IFileProvider;
            if (this._FileProvider == null) throw new InvalidOperationException();
        }
        return this._FileProvider;
    }
}
