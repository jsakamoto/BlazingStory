---
$attribute: CustomPage("Guides/MCP Integration")
---

## MCP Integration

This Blazing Story app exposes a **Model Context Protocol (MCP)** server, allowing AI assistants and other MCP clients to query the UI catalog programmatically.

### How it works

The MCP server runs alongside the Blazing Story web app. When a client connects, it can discover components, read their parameters and stories, and search documentation pages — all through structured tool calls.

### Available tools

| Tool | Description |
|------|-------------|
| `getComponents` | List all components in the catalog with name and summary |
| `getComponentParameters` | Get the configurable parameters for a component |
| `getComponentStories` | Get usage examples with code snippets |
| `getCustomPages` | List all custom and markdown documentation pages |
| `getCustomPageContent` | Retrieve the full rendered content of a page |
| `searchCustomPages` | Search across all pages by keyword with relevance ranking |

### Connecting a client

Point your MCP client at the server endpoint:

```
http://localhost:5277/mcp/blazingstory
```

The server uses HTTP transport with the Streamable HTTP protocol.

### Setup in your project

Add the MCP server NuGet package and wire it up in `Program.cs`:

```csharp
builder.Services.AddBlazingStoryMcpServer();

var app = builder.Build();
app.MapBlazingStoryMcp();
```

That's it — the MCP endpoint is now available at `/mcp/blazingstory`.

### Configuring the HTTP transport

Every tool above is a read-only lookup against the catalog, so the server runs **stateless**. It answers each request on its own and keeps no session between calls. Clients that still use the older `initialize` handshake also work without a session id.

`AddBlazingStoryMcpServer()` returns an `IMcpServerBuilder`. Call `WithHttpTransport()` on it to change the HTTP transport options. For example, if a client needs a real session, turn on the hybrid session mode like this.

```csharp
using ModelContextProtocol.AspNetCore;

builder.Services.AddBlazingStoryMcpServer()
    .WithHttpTransport(options =>
        options.SessionMode = HttpServerSessionMode.StatefulForInitializeClients);
```
