using BlazingStory.Components;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StoryServerApp.Components.Pages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var serverUrl = builder.Configuration[WebHostDefaults.ServerUrlsKey]?.Split(';').FirstOrDefault() ?? "http://localhost:5000";
builder.Services.TryAddScoped(sp => new HttpClient { BaseAddress = new Uri(serverUrl) });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
#if (!NoHttps)
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
#endif
}

#if (!NoHttps)
app.UseHttpsRedirection();

#endif
#if (Framework == "net8.0")
app.UseStaticFiles();
#else
app.MapStaticAssets();
#endif
app.UseRouting();
app.UseAntiforgery();

app.MapRazorComponents<BlazingStoryServerComponent<IndexPage, IFramePage>>()
    .AddInteractiveServerRenderMode();

app.Run();
