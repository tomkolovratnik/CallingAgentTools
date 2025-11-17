using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using OpenAI;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AG_01
{
    internal static class McpCalling
    {
        public async static Task<AgentRunResponse> Call()
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


            var chatClient = client.GetChatClient("mistral:7b-instruct");

            await using var mcpClient = await McpClient.CreateAsync(new StdioClientTransport(new()
            {
                Name = "MCPServer",
                Command = "npx",
                Arguments = ["-y", "--verbose", "@modelcontextprotocol/server-github"],
            }));

            var agent = chatClient.CreateAIAgent(
                name: "LocalAgent",
                instructions: "You are a helpful assistant.",
                tools: [AIFunctionFactory.Create(GetWeather)]
            );


            return await agent.RunAsync("What is the weather like in Prague?").ConfigureAwait(false);
        }
        [Description("Get the weather for a given location.")]
        static string GetWeather([Description("The location to get the weather for.")] string location)
        => $"The weather in {location} is cloudy with a high of 15°C.";
    }
}
