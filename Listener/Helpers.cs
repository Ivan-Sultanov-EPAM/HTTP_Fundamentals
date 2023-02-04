using System.Net;

namespace Listener;

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
                response.StatusCode = 307;
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
                response.Cookies.Add(new Cookie("MyNameCookie", "Ivan"));
                break;
            default:
                response.StatusCode = 200;
                break;
        }
    }

    public static string ParseRequest(this string path) => path.Trim('/');

    public static string GetMyName(this HttpListenerRequest request)
    {
        var path = request.RawUrl?.ParseRequest();

        return path == "GetMyName" ? "Ivan" : path;
    }
}