using BlazingStory.Components;
#if (McpServer)
using BlazingStory.McpServer;
#endif
using Microsoft.Extensions.DependencyInjection.Extensions;
using StoryServerApp.1.Components.Pages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
#if (McpServer)
builder.Services.AddBlazingStoryMcpServer();
#endif

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

#if (Framework == "net9.0")
app.UseStatusCodePagesWithReExecute("/not-found");
#endif
#if (Framework == "net10.0")
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
#endif
#if (!NoHttps)
app.UseHttpsRedirection();

#endif
#if (McpServer)
app.MapBlazingStoryMcp();
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
