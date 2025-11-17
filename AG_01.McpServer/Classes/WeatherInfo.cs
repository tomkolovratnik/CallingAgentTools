using System;
using System.Text.Json.Serialization;

namespace AG_01.McpServer.Classes
{
    public class WeatherInfo
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
