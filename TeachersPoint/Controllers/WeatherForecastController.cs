using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Xml.Linq;
using TeachersPoint.BusinessLayer.Implementation;
using TeachersPoint.BusinessLayer.Interface;
using TeachersPoint.Core.RequestDto;
using Microsoft.Extensions.Configuration;
using System.Data;
using Npgsql;

namespace TeachersPoint.WebApisService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMongoCollection<MongoRequestDto> _myDataCollection;
        private readonly IConfiguration _configuration; 


        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ITestService _testservice;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITestService testservice, IMongoCollection<MongoRequestDto> myDataCollection, IConfiguration conf)
        {
            _logger = logger;
            _testservice = testservice;
            _myDataCollection = myDataCollection;
            _configuration = conf;
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

        [HttpGet]
        [Route("GetByRollNo")]
        public async Task<IActionResult> GetByRollNo(int rollNo)
        {
            var data = await _myDataCollection.Find(d => d.rollNo == rollNo).FirstOrDefaultAsync();

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        //[HttpGet]
        //[Route("GetFromSQL")]
        //public JsonResult GetFromSQL(string s)
        //{
        //    DataTable dt = new DataTable();
        //    string query = @"select * from mastertable";
        //    string sqlDataSource = _configuration.GetConnectionString("TeachersPointDB");
        //    NpgsqlDataReader myreader;
        //    using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
        //    {
        //        myCon.Open();
        //        using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
        //        {
        //            myreader = myCommand.ExecuteReader();
        //            dt.Load(myreader);

        //            myreader.Close();
        //            myCon.Close();
        //        }
        //    }

        //    return new JsonResult(dt);
        //}
        [HttpGet]
        [Route("GetFromSQL")]
        public IActionResult GetFromSQL()
        {
            // Connection string from configuration
            string connectionString = _configuration.GetConnectionString("TeachersPointDB");

            // SQL query
            string query = "SELECT * FROM mastertable;";

            try
            {
                // Create a DataTable to store the results
                DataTable dataTable = new DataTable();

                // Use using blocks to ensure proper disposal of resources
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Use NpgsqlDataAdapter to execute the query and fill the DataTable
                    using (var adapter = new NpgsqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dataTable);
                    }
                }

                // Return the DataTable as a JSON result
                return Ok(new JsonResult(dataTable));
            }
            catch (Exception ex)
            {
                // Log the exception using a proper logging framework
                _logger.LogError($"Error in GetFromSQL: {ex.Message}", ex);

                // Return an error response with appropriate status code
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}