namespace Client;

internal class ClientApp
{
    public static async Task Act(HttpClient client)
    {
        var requests = new List<string>
        {
            "http://localhost:8888/GetMyName/",
            "http://localhost:8888/Information/",
            "http://localhost:8888/Success/",
            "http://localhost:8888/Redirection/",
            "http://localhost:8888/ClientError/",
            "http://localhost:8888/ServerError/",
            "http://localhost:8888/MyNameByHeader/",
            "http://localhost:8888/MyNameByCookies/"
        };

        foreach (var request in requests)
        {
            Console.WriteLine("Press any key to proceed...");
            Console.WriteLine();
            Console.ReadKey(true);
            var response = await client.GetAsync(request);

            Console.WriteLine($"Request: {request}");
            Console.WriteLine("Response:");
            Print(response);

            Console.WriteLine();
        }

        static void Print(HttpResponseMessage response)
        {
            var text = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine($"  Content: {text}");
            Console.WriteLine($"  Status Code: {response.StatusCode}");

            var myNameHeader = GetHeaderValue(response, "MyNameHeader");

            var myNameCookie = GetCookieValue(response, "MyNameCookie");

            if (!string.IsNullOrEmpty(myNameHeader))
            {
                Console.WriteLine($"Headers: {myNameHeader}");
            }

            if (!string.IsNullOrEmpty(myNameCookie))
            {
                Console.WriteLine($"Cookie: {myNameCookie}");
            }
        }

        static string GetHeaderValue(HttpResponseMessage message, string headerName)
        {
            var result = message.Headers
                .FirstOrDefault(h => h.Key.Equals(headerName))
                .Value?
                .FirstOrDefault();

            return result ?? "";
        }

        static string GetCookieValue(HttpResponseMessage message, string cookieName)
        {
            var result = message.Headers
                .FirstOrDefault(h => h.Key.Equals("Set-Cookie"))
                .Value?
                .FirstOrDefault(s => s.Contains(cookieName));

            return result != null ? result.Split('=')[1] : "";
        }
    }
}