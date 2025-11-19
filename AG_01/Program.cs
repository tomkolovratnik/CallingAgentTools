
using Microsoft.Extensions.Configuration;

public class Program
{
    public async static Task Main(string[] args)
    {

        //Tool calling
        var result = await AG_01.ToolsCalling.Call().ConfigureAwait(false);
        Console.WriteLine(result);

        //MCP Server calling
        var mcpResult = await AG_01.McpCalling.Call().ConfigureAwait(false);
        Console.WriteLine(mcpResult);
    }
}

//Result from Tool calling:
//It looks like it's currently cloudy in Prague, with a high temperature of 15 degrees Celsius. Make sure to pack accordingly, and enjoy the trip!

//Result from MCP Server calling:
//The weather in Prague is as follows:
//-Temperature: 2°C(feels like - 1.19°C)
//- Clouds: Few clouds
//- Humidity: 71 %
//-Wind: 3.13 m / s ?????