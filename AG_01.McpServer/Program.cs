using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;


namespace AG_01.McpServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateEmptyApplicationBuilder(settings: null);

            var env = builder.Environment;

            //builder.Configuration
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            builder.Configuration.AddUserSecrets<Program>(optional: true);

            builder.Configuration
                .AddEnvironmentVariables()        // All environment variables
                .AddCommandLine(args);            // Allow overrides from command line

            builder.Services
                .AddMcpServer()
                .WithStdioServerTransport()
                .WithToolsFromAssembly();

            var app = builder.Build();

            // Načtení API_KEY z konfigurace a předání do WeatherMcpServer
            var apiKey = app.Services.GetRequiredService<IConfiguration>()["OPENWEATHER_API_KEY"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("OPENWEATHER_API_KEY není nastavena v user secrets nebo konfiguraci");
            }
            WeatherMcpServer.API_KEY = apiKey;

            await app.RunAsync();
        }
    }
}
