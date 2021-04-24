using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Deliveggie.Backend.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "deliveggie backend api";
        }
    }
}
