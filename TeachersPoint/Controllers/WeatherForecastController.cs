using Microsoft.AspNetCore.Mvc;
using TeachersPoint.BusinessLayer.Implementation;
using TeachersPoint.BusinessLayer.Interface;
using TeachersPoint.Core.RequestDto;

namespace TeachersPoint.WebApisService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ITestService _testservice;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITestService testservice)
        {
            _logger = logger;
            _testservice = testservice;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("TestController")]
        public string TestMethodCall()
        {

            return _testservice.ThisIsTestServiceMethodCalling("TestParams");



        
        }


        [HttpPost]
        [Route("RegisterNewUser")]
        public int RegisterNewUser(UserDto user)
        {

            return _testservice.RegisterNewUser(user);

        }

        [HttpPost]
        [Route("LogIn")]
        public string LogIn(UserDto user)
        {

            return _testservice.LogIn(user);
        }
    }
}