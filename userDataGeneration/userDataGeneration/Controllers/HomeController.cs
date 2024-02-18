using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using userDataGeneration.Models;
using userDataGeneration.Services;

namespace userDataGeneration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;

        public HomeController(ILogger<HomeController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        public IActionResult Index(string region = "en", double errorRate = 0, int seed = 20, int page = 20)
        {
            // Validate the input parameters
            if (string.IsNullOrEmpty(region)) return BadRequest("Region is required");
            if (errorRate < 0 || errorRate > 10) return BadRequest("Error rate must be between 0 and 10");
            if (seed < 0) return BadRequest("Seed must be non-negative");
            if (page < 1) return BadRequest("Page must be positive");

            // Combine the seed and the page number to get a unique seed for each page
            var pageSeed = seed + page;

            // Generate the user data for the given parameters
            var users = _userService.GenerateUsers(region, errorRate, pageSeed);

            return View(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
