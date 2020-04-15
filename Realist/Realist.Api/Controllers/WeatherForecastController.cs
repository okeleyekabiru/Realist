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
using Realist.Data.Extensions;
using Realist.Data.Infrastructure;
using Realist.Data.Model;

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
        private readonly IUserInfo _userInfoContext;
        private readonly IBot _botContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IDeviceAuth device,IUserInfo userInfoContext,IBot botContext)
        {
            _logger = logger;
            _device = device;
            _userInfoContext = userInfoContext;
            _botContext = botContext;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var m = GetDeviceCurrentLocation.GetCoordinates("nigeria");


            var result = await RealistExtension.VerifyDevice(_device, _userInfoContext, "ghana", _botContext);
            if (!result.Succeeded) return BadRequest();

            return Ok();



        }
    }
}
