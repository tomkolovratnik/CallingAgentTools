using System;
using System.Text.Json.Serialization;

namespace AG_01.McpServer.Classes
{
    public class ForecastResponse
    {
        [JsonPropertyName("city")]
        public CityInfo City { get; set; }

        [JsonPropertyName("list")]
        public ForecastItem[] List { get; set; }
    }
}
