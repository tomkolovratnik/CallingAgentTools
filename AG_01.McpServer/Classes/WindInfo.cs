using System;
using System.Text.Json.Serialization;

namespace AG_01.McpServer.Classes
{
    public class WindInfo
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
    }
}
