using AG_01.McpServer.Classes;
using ModelContextProtocol.Server;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AG_01.McpServer
{
    [McpServerToolType()]
    public class WeatherMcpServer
    {
        public static readonly HttpClient httpClient = new();
        private const string BASE_URL = "https://api.openweathermap.org/data/2.5";

        [McpServerTool, Description("Získá aktuální počasí pro zadané město.")]
       
        static async Task<string> GetCurrentWeather(int requestId, JsonElement arguments, CancellationToken ct)
        {
            var city = arguments.GetProperty("city").GetString();
            var country = arguments.TryGetProperty("country", out var countryProp) ? countryProp.GetString() : "";

            var location = string.IsNullOrEmpty(country) ? city : $"{city},{country}";
            var url = $"{BASE_URL}/weather?q={location}&appid={API_KEY}&units=metric";

            var response = await httpClient.GetStringAsync(url, ct);
            ct.ThrowIfCancellationRequested();
            var weatherData = JsonSerializer.Deserialize<WeatherResponse>(response);

            var result = $"Počasí v {weatherData.Name}:\n" +
                        $"🌡️ Teplota: {weatherData.Main.Temp}°C (pocitově {weatherData.Main.FeelsLike}°C)\n" +
                        $"☁️ Popis: {weatherData.Weather[0].Description}\n" +
                        $"💧 Vlhkost: {weatherData.Main.Humidity}%\n" +
                        $"💨 Vítr: {weatherData.Wind.Speed} m/s";

            return result;
        }

        public static async Task<string> GetWeatherForecast(int requestId, JsonElement arguments, CancellationToken ct)
        {
            var city = arguments.GetProperty("city").GetString();
            var days = arguments.TryGetProperty("days", out var daysProp) ? daysProp.GetInt32() : 3;

            var url = $"{BASE_URL}/forecast?q={city}&appid={API_KEY}&units=metric&cnt={days * 8}"; // 8 forecasts per day

            var response = await httpClient.GetStringAsync(url, ct);
            ct.ThrowIfCancellationRequested();
            var forecastData = JsonSerializer.Deserialize<ForecastResponse>(response);

            var result = $"Předpověď počasí pro {forecastData.City.Name} ({days} dny):\n\n";

            var dailyForecasts = forecastData.List
                .GroupBy(f => DateTime.Parse(f.DtTxt).Date)
                .Take(days);

            foreach (var dayGroup in dailyForecasts)
            {
                var dayForecast = dayGroup.First();
                result += $"📅 {dayGroup.Key:dd.MM.yyyy}\n" +
                         $"🌡️ {dayForecast.Main.Temp}°C, {dayForecast.Weather[0].Description}\n" +
                         $"💧 Vlhkost: {dayForecast.Main.Humidity}%\n\n";
            }

            return result;
        }
    }
}
