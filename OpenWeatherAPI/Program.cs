using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

public class Program
{
    static async Task<int> Main(string[] args)
    {
        var builder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient();
                services.AddTransient<IMyService, MyService>();
            }).UseConsoleLifetime();

        var host = builder.Build();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            try
            {
                var myService = services.GetRequiredService<IMyService>();

                var pageContent = await myService.GetPage();

                var myJObject = JObject.Parse(pageContent);

                var temp = myJObject["main"].Value<string>("temp").ToString();

                Console.WriteLine("Temperature:" + temp);

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "Error accessing OpenWeather API");
            }
        }

        return 0;
    }

    public interface IMyService
    {
        Task<string> GetPage();
    }

    public class MyService : IMyService
    {
        private readonly IHttpClientFactory _clientFactory;

        public MyService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<string> GetPage()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "http://api.openweathermap.org/data/2.5/weather?q=Wollongong,au&APPID=0394a2b26c1acd382abd634a43a0ea48");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return $"StatusCode: {response.StatusCode}";
            }
        }
    }
}
