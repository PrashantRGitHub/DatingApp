using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.Controllers
{   
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        //private static readonly string[] Summaries = new[]
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DataContext _dataContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    //var rng = new Random();
        //    //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    //{
        //    //    Date = DateTime.Now.AddDays(index),
        //    //    TemperatureC = rng.Next(-20, 55),
        //    //    Summary = Summaries[rng.Next(Summaries.Length)]
        //    //})
        //    //.ToArray();
        //}

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var weatherForeCastDb = await _dataContext.WeatherForcasts.ToListAsync();

            return Ok(weatherForeCastDb);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var weatherForeCastDb = await _dataContext.WeatherForcasts.FirstOrDefaultAsync(x =>x.ID.Equals(id));

            return Ok(weatherForeCastDb);
        }
    }
}
