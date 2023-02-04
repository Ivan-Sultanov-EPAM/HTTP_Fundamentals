namespace Client;

public class Program
{
    private static async Task Main()
    {
        using HttpClient client = new();
        await ClientApp.Act(client);
    }
}