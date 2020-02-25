using Microsoft.AspNetCore.Mvc;
using TempApp.Data;
using TempApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace TempApp.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly SampleContext _context;

        public WeatherController(SampleContext context)
        {
            _context = context;
        }

        //[HttpGet]
        ////[Route("[action]")]
        //public ActionResult<IEnumerable<Temperature>> Get() =>
        //    _context.Temperatures.ToList();

        [HttpGet("{city}")]
        public async Task<ActionResult<Temperature>> GetByCity(string city)
        {
            // In production this method should take the following parameters
            // baseUrl, City, Units and user key
            //
            _context.GetTemperature();

            Temperature result = null;

            List <Temperature> tempList = _context.Temperatures.ToList();

            if (city != null && tempList.Count() > 0)
            {
                result = tempList.SingleOrDefault(s => s.City.ToLower() == city.ToLower());
            }

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }
    }
}
