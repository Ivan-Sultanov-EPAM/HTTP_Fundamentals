namespace Client;

class ClientApp
{
    private static HttpClient _client;

    static async Task Main()
    {
        _client = new HttpClient();

        await Act();
    }

    static async Task Act()
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

        while (true)
        {
            foreach (var request in requests)
            {
                var response = await _client.GetAsync(request);
                Console.WriteLine(request);
                Print(response);

                Console.ReadKey();
                Console.WriteLine();
            }
        }

        static void Print(HttpResponseMessage response)
        {
            var text = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine($"Content: {text}");
            Console.WriteLine($"Status Code: {response.StatusCode}");

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

            Console.WriteLine();
        }

        static string GetHeaderValue(HttpResponseMessage message, string cookieName)
        {
            var result = message.Headers
                .Where(h => h.Key.Equals(cookieName))
                .Select(p => p.Value)
                .FirstOrDefault();

            if (result != null)
            {
                return string.Join(" ", result);
            }

            return "";
        }

        static string GetCookieValue(HttpResponseMessage message, string name)
        {
            var result = message.Headers
                .Where(h => h.Key.Equals("Set-Cookie"))
                .Select(p => p.Value)
                .FirstOrDefault(v => v.Any(s => s.Contains("MyNameCookie")))
                ?.FirstOrDefault();

            if (result != null)
            {
                return result.Split('=')[1];
            }

            return "";
        }
    }
}