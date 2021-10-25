﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace TS.Microservices.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HcTestController : ControllerBase
    {
        [HttpGet]
        public IActionResult SetReady([FromQuery] bool ready)
        {
            Startup.Ready = ready;
            if (!ready)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(60000);
                    Startup.Ready = true;
                });
            }
            return Content($"{Environment.MachineName} : Ready={Startup.Ready}");
        }
        [HttpGet]
        public IActionResult SetLive([FromQuery] bool live)
        {
            Startup.Live = live;
            return Content($"{Environment.MachineName} : Live={Startup.Live}");
        }


        [HttpGet]
        public IActionResult Exit([FromServices] IHostApplicationLifetime application)
        {
            Task.Run(async () =>
            {
                await Task.Delay(3000);
                application.StopApplication();
            });
            return Content($"{Environment.MachineName} : Stopping");
        }
    }
}
