using System.Net;

namespace Listener;

internal class ListenerApp
{
    public static async Task Listen(HttpListener listener)
    {
        while (true)
        {
            var context = await listener.GetContextAsync();

            var request = context.Request;
            var response = context.Response;

            var responseString = request.GetMyName();

            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;

            response.SetStatusCode(request);

            var output = response.OutputStream;

            await output.WriteAsync(buffer, 0, buffer.Length);

            output.Close();
        }
    }
}