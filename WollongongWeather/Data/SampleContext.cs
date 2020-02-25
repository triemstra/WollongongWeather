using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TempApp.Models;

namespace TempApp.Data
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options) : base(options)
        {
        }

        public DbSet<Temperature> Temperatures { get; set; }

        // In production this method should take the following parameters:
        // baseUrl, City, Units and user key
        //
        public async Task<Temperature> GetTemperature()
        {
            HttpClient client = new HttpClient();

            MyService weatherAPI = new MyService(client);

            var pageContent = await weatherAPI.GetPage();

            var myJObject = JObject.Parse(pageContent);

            var temp = myJObject["main"].Value<string>("temp").ToString();

            Temperature temperature = new Temperature();

            temperature.Id = 5;

            temperature.Temp = temp;

            temperature.City = "wollongong";

            return temperature;
        }
    }

    public interface IMyService
    {
        Task<string> GetPage();
    }

    public class MyService : IMyService
    {
        private readonly HttpClient _client;

        public MyService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetPage()
        {
            // Hard coded for demo purposes
            // In production this method should take the following parameters:
            // baseUrl, City, Units and user key
            // to build the URI
            //
            var request = new HttpRequestMessage(HttpMethod.Get, "http://api.openweathermap.org/data/2.5/weather?q=Wollongong,au&units=metric&APPID=0394a2b26c1acd382abd634a43a0ea48");

            var response = await _client.SendAsync(request);

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
