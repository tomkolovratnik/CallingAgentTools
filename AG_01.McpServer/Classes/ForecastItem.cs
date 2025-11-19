using System;
using System.Text.Json.Serialization;

namespace AG_01.McpServer.Classes
{
    public class ForecastItem
    {
        [JsonPropertyName("dt_txt")]
        public string DtTxt { get; set; }

        [JsonPropertyName("main")]
        public MainWeather Main { get; set; }

        [JsonPropertyName("weather")]
        public WeatherInfo[] Weather { get; set; }
    }
}
