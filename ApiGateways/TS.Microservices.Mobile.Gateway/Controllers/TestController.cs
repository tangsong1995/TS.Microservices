using Microsoft.AspNetCore.Mvc;

namespace TS.Microservices.Mobile.Gateway.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult TestOcelot()
        {
            return Content("TestOcelot TS.Microservices.Mobile.Gateway");
        }
    }
}
