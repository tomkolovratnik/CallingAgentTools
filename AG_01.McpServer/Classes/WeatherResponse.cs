using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.Json.Serialization;

namespace AG_01.McpServer.Classes
{
    public class WeatherResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("main")]
        public MainWeather Main { get; set; }

        [JsonPropertyName("weather")]
        public WeatherInfo[] Weather { get; set; }

        [JsonPropertyName("wind")]
        public WindInfo Wind { get; set; }
    }
}
