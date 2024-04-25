using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders.Composite;

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
        public IActionResult NewData(string num, double latitude, double longitude)
        {
            HttpClient client = new HttpClient();
            using(StreamWriter fs = new("/home/contents.txt",true))
            {
                fs.WriteLine(num + $": long:{longitude}; lat:{latitude}");
            }

            return Ok();
        }
    }
}
