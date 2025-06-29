using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<PingService>();

//MCP Tools
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();
builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

//OpenAPI
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "MCP API V1");
    options.RoutePrefix = "swagger";
});

//Endpoints HTTP
app.MapGet("/ping/{name}", (string name, PingService svc) => svc.CreatePing(name))
   .WithName("Ping")
   .WithSummary("Get a personalized greeting")
   .WithDescription("Return a greeting for the given name. Required: name (string).")
   .WithTags("Greeting")
   .WithOpenApi();

//MCP Endpoints
app.MapMcp("/mcp")
   .WithOpenApi();

app.Run();


//MCP Tools
[McpServerToolType]
public class PingTool
{
    private readonly PingService _pingService;
    public PingTool(PingService pingService)
    {
        _pingService = pingService;
    }

    [McpServerTool(Name = "ping"), Description("Return a greeting for the given name. Required: name (string).")]
    public string ping([Description("Person's name")] string name)
    {
        return _pingService.CreatePing(name);
    }
}

//Services
public class PingService
{
    public string CreatePing(string name)
    {
        return $"Hello, {name}!";
    }
}
