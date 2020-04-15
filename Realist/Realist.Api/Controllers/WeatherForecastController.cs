using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.FileSystemChange;
using Plugins;
using Plugins.DeviceAuthentication;

namespace Realist.Api.Controllers
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
        private readonly IDeviceAuth _device;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IDeviceAuth device)
        {
            _logger = logger;
            _device = device;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
           
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
          var model =  DeviceAddress.GetMacAddress().GetAddressBytes();
          var convert = Convert.ToBase64String(model);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
