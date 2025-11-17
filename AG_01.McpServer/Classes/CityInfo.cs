using System;
using System.Text.Json.Serialization;

namespace AG_01.McpServer.Classes
{
    public class CityInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
