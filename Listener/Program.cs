using System.Net;

namespace Listener;

public class Program
{
    private static readonly HttpListener Listener = new();

    private static async Task Main()
    {
        Listener.Prefixes.Add("http://localhost:8888/");
        Listener.Start();
        Console.WriteLine("Listening...");
        await ListenerApp.Listen(Listener);
        Listener.Stop();
    }
}