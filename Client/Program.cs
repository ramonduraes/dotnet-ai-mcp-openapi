using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol; 

// Initialize MCP client
var mcpClient = await McpClientFactory.CreateAsync(
    new SseClientTransport(
        new SseClientTransportOptions
        {
            Name = "MCP",
            Endpoint = new Uri("http://localhost:5000/mcp")
        }
    )
);

// Call the "ping" tool
var pingResponse = await mcpClient.CallToolAsync(
    "ping",
    new Dictionary<string, object?> { ["name"] = "Ramon" }
);

// Extract result
var responseText = pingResponse.Content
    .OfType<TextContentBlock>()
    .Select(tb => tb.Text)
    .FirstOrDefault() 
    ?? "no response";

// print result
Console.WriteLine(responseText);