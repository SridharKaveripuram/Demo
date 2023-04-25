using Microsoft.AspNetCore.Mvc;

namespace SignalClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.WebSocketUrl = _configuration.GetValue<string>("WebSocketHostUrl");
            return View();
        }
    }
}