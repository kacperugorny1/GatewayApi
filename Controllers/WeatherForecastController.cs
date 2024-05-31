using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders.Composite;
using System.Text;
using Newtonsoft.Json;

namespace HttpOnlyTest.Controllers
{
    [ApiController]
    [Route("")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Get")]
        public string Get()
        {
            using(StreamReader fs = new("/home/contents.txt"))
            {
                return fs.ReadToEnd();
            }
        }
        [HttpPost("NewData")]
        public async Task<IActionResult> NewDataAsync(string num, double latitude, double longitude, double temp, double humi, double press, string code)
        {
            HttpClient client = new HttpClient();
            string url = "https://weatherapiproject.azurewebsites.net/api";

            var payload = new
            {
                nr_tel = num,
                kod = code,
                bateria = 100,
                temp = temp,
                press = press,
                humi = humi,
                lat = latitude,
                longi = longitude
            };
            string jsonPayload = JsonConvert.SerializeObject(payload);
            StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Send the POST request
            HttpResponseMessage response = await client.PostAsync(url, content);

            // Read the response content
            string responseContent = await response.Content.ReadAsStringAsync();

            using (StreamWriter fs = new("/home/contents.txt", true))
            {
                fs.WriteLine(num + $": long:{longitude}; lat:{latitude}; temp:{temp}; humi:{humi}; press:{press}; code:{code}, responseCode:{response.StatusCode}");
            }

            return Ok();
        }
    }
}
