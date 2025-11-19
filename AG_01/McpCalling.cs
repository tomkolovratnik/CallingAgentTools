using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using OpenAI;
using System;
using System.ClientModel;

namespace AG_01
{
    internal static class McpCalling
    {
        public async static Task<AgentRunResponse> Call( )
        {
            // Vytvoření OpenAI klienta s Ollama endpointem
            var clientOptions = new OpenAIClientOptions()
            {
                Endpoint = new Uri("http://localhost:11434/v1/")
            };

            var client = new OpenAIClient(
                new ApiKeyCredential("not-needed"), // Ollama nevyžaduje API klíč
                clientOptions
            );


            //var chatClient = client.GetChatClient("mistral:7b-instruct");
            var chatClient = client.GetChatClient("qwen2.5:3b");
            await using var mcpClient = await McpClient.CreateAsync(new StdioClientTransport(new()
            {
                Name = "MCPWeatherServer",
                Command = "dotnet",
                Arguments = ["run", "--project", @"..\..\..\..\AG_01.McpServer\AG_01.McpServer.csproj"],
            })).ConfigureAwait(true);

            // Retrieve the list of available tools 
            var mcpTools = await mcpClient.ListToolsAsync().ConfigureAwait(false);

            
            var agent = chatClient.CreateAIAgent(
                name: "LocalAgent",
                instructions: "You are a helpful assistant.",
                tools: [.. mcpTools.Cast<AITool>()]
            );

           return await agent.RunAsync("What is the weather like in Prague?").ConfigureAwait(false);

             
        }

        
    }
}
