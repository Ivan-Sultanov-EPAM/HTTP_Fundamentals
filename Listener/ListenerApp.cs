using System.Net;

namespace Listener;

class ListenerApp
{
    static readonly HttpListener _listener = new();

    static async Task Main()
    {
        _listener.Prefixes.Add("http://localhost:8888/");
        _listener.Start();
        Console.WriteLine("Listening...");
        await Listen();
        _listener.Stop();
    }

    static async Task Listen()
    {
        while (true)
        {
            var context = await _listener.GetContextAsync();
            var request = context.Request;
            var response = context.Response;

            var responseString = GetMyName(request);

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;

            response.SetStatusCode(request);

            var output = response.OutputStream;

            await output.WriteAsync(buffer, 0, buffer.Length);

            output.Close();
        }
    }

    private static string GetMyName(HttpListenerRequest request)
    {
        var path = request.RawUrl?.ParseRequest();

        if (path == "GetMyName")
        {
            return "Ivan";
        }

        return path;
    }
}

public static class Helpers
{
    public static void SetStatusCode(this HttpListenerResponse response, HttpListenerRequest request)
    {
        switch (request.RawUrl?.ParseRequest())
        {
            case "Information":
                response.StatusCode = 200;
                break;
            case "Success":
                response.StatusCode = 200;
                break;
            case "Redirection":
                response.StatusCode = 300;
                break;
            case "ClientError":
                response.StatusCode = 400;
                break;
            case "ServerError":
                response.StatusCode = 500;
                break;
            case "MyNameByHeader":
                response.StatusCode = 200;
                response.Headers.Add("MyNameHeader: Ivan");
                break;
            case "MyNameByCookies":
                response.StatusCode = 200;
                response.Cookies.Add(new Cookie("MyName", "Ivan"));
                break;
            default:
                response.StatusCode = 200;
                break;
        }
    }

    public static string ParseRequest(this string path) => path.Trim('/');
}