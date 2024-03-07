using Microsoft.Extensions.Configuration;

class Program
{
    const string PROMPT = "command:> ";
    static string? FlightAPI_Address = null;

    public static async Task Main()
    {
        AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

        var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("app.config", optional: false);

        IConfiguration config = builder.Build();

        FlightAPI_Address = config.GetSection("FlightApi_Address").Get<string>();

        bool isQuit = false;

        using (var client = new HttpClient() { BaseAddress = new Uri(FlightAPI_Address) })
        {
            while (!isQuit)
            {
                Console.Write(PROMPT);

                var cmd = Console.ReadLine();

                switch (cmd.ToLower())
                {
                    case "?":
                        Console.WriteLine("Load - load flights from extern API.");
                        Console.WriteLine("Quit - quit client.");
                        Console.WriteLine("?    - help.");
                        break;

                    case "quit":
                        isQuit = true;
                        Console.WriteLine("End.");
                        break;
                    case "load":
                        Console.Write("Filter [empty]: ");
                        var filter = Console.ReadLine();

                        Console.Write("Order [empty]: ");
                        var sortOrder = Console.ReadLine();

                        string query = string.Empty;

                        if (!string.IsNullOrEmpty(filter))
                        {
                            query = $"Filter={filter}";
                        }
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            if (query.Length > 0)
                                query += "&";

                            query += $"Order={sortOrder}";
                        }

                        if (query.Length > 0)
                            query = $"?{query}";

                        var text = $"{FlightAPI_Address}{query}";

                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Get,
                            RequestUri = new Uri(text)
                        };

                        try
                        {
                            var response = await client.SendAsync(request).ConfigureAwait(false);
                            response.EnsureSuccessStatusCode();

                            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            Console.WriteLine(responseBody);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                        }
                        break;

                    default:
                        Console.WriteLine("Bad command format. Use ? for help.");
                        break;
                }
            }
        }
    }

    private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
    {
        Console.WriteLine(e.ExceptionObject.ToString());
    }
}