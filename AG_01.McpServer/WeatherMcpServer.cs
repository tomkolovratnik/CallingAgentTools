using AG_01.McpServer.Classes;
using ModelContextProtocol.Server;
using System;
using System.ComponentModel;
using System.Text.Json;

namespace AG_01.McpServer
{
    [McpServerToolType()]
    public class WeatherMcpServer
    {
        private const string BASE_URL = "https://api.openweathermap.org/data/2.5";

        public static readonly HttpClient httpClient = new();

        [McpServerTool, Description("Gets the current weather for the specified city.")]
        public static async Task<string> GetCurrentWeather([Description("Weather request parameters")] CurrentWeatherParameters parameters, CancellationToken ct)
        {
            var location = string.IsNullOrEmpty(parameters.Country) ? parameters.City : $"{parameters.City},{parameters.Country}";
            var url = $"{BASE_URL}/weather?q={location}&appid={API_KEY}&units=metric";

            var response = await httpClient.GetStringAsync(url, ct).ConfigureAwait(false);
            ct.ThrowIfCancellationRequested();
            var weatherData = JsonSerializer.Deserialize<WeatherResponse>(response);

            var result = $"Počasí v {weatherData.Name}:\n" +
                        $"🌡️ Teplota: {weatherData.Main.Temp}°C (pocitově {weatherData.Main.FeelsLike}°C)\n" +
                        $"☁️ Popis: {weatherData.Weather[0].Description}\n" +
                        $"💧 Vlhkost: {weatherData.Main.Humidity}%\n" +
                        $"💨 Vítr: {weatherData.Wind.Speed} m/s";

            return result;
        }

        [McpServerTool, Description("Gets the forecast weather for the specified city.")]
        public static async Task<string> GetWeatherForecast([Description("Forecast request parameters")] ForecastParameters parameters, CancellationToken ct)
        {
            var url = $"{BASE_URL}/forecast?q={parameters.City}&appid={API_KEY}&units=metric&cnt={parameters.Days * 8}"; // 8 forecasts per day

            var response = await httpClient.GetStringAsync(url, ct).ConfigureAwait(false);
            ct.ThrowIfCancellationRequested();
            var forecastData = JsonSerializer.Deserialize<ForecastResponse>(response);

            var result = $"Předpověď počasí pro {forecastData.City.Name} ({parameters.Days} dny):\n\n";

            var dailyForecasts = forecastData.List
                .GroupBy(f => DateTime.Parse(f.DtTxt).Date)
                .Take(parameters.Days);

            foreach (var dayGroup in dailyForecasts)
            {
                var dayForecast = dayGroup.First();
                result += $"📅 {dayGroup.Key:dd.MM.yyyy}\n" +
                         $"🌡️ {dayForecast.Main.Temp}°C, {dayForecast.Weather[0].Description}\n" +
                         $"💧 Vlhkost: {dayForecast.Main.Humidity}%\n\n";
            }

            return result;
        }

        public static string? API_KEY { get; set; }
    }
}
