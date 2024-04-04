using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace BlazingStory.Internals.Services;

internal class WebAssets
{
    private readonly IServiceProvider _Services;

    private IFileProvider? _FileProvider;

    public WebAssets(IServiceProvider services)
    {
        this._Services = services;
    }

    public ValueTask<string> GetStringAsync(string path, string queryString = "")
    {
        if (!OperatingSystem.IsBrowser()) return this.GetStringOnServerAsync(path);
        return this.GetStringOnWebAssemblyAsync(path, queryString);
    }

    private async ValueTask<string> GetStringOnWebAssemblyAsync(string path, string queryString)
    {
        var httpClient = this._Services.GetRequiredService<HttpClient>();
        return await httpClient.GetStringAsync(path + "?" + queryString.TrimStart('?'));
    }

    private async ValueTask<string> GetStringOnServerAsync(string path)
    {
        var fileProvider = this.GetFileProvider();
        var fileInfo = fileProvider.GetFileInfo(path);
        using var fileStream = fileInfo.CreateReadStream();
        using var fileReader = new StreamReader(fileStream);
        return await fileReader.ReadToEndAsync();
    }

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
