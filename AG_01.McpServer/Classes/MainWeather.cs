using System;
using System.Text.Json.Serialization;

namespace AG_01.McpServer.Classes
{
    public class MainWeather
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }
}
