
using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;
using System.ComponentModel;
public class Program
{
    public async static Task Main(string[] args)
    {
        var result = await AG_01.ToolsCalling.Call().ConfigureAwait(false);
        Console.WriteLine(result);
    }
}

