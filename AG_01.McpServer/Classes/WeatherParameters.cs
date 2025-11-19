using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AG_01.McpServer.Classes
{
    public class CurrentWeatherParameters
    {
        [JsonPropertyName("city")]
        [Description("The city name to get weather for")]
        public string City { get; set; }

        [JsonPropertyName("country")]
        [Description("The country code (optional)")]
        public string Country { get; set; }
    }

    public class ForecastParameters
    {
        [JsonPropertyName("city")]
        [Description("The city name to get forecast for")]
        public string City { get; set; }

        [JsonPropertyName("days")]
        [Description("Number of days to forecast (default 3)")]
        public int Days { get; set; } = 3;
    }
}
