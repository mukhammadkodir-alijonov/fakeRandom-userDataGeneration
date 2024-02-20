using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using userDataGeneration.Models;
using userDataGeneration.Services;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private List<Data> query;


    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public IActionResult PartialTable(int itemsCount = 1, int page = 1, string locale = "ru", float errors = 0, int seed = 0)
    {
        DataFaker faker = new DataFaker(seed + page, locale, itemsCount, errors);
        if (page == 1)
            query = faker.Get(20);
        if (page > 1)
            query = faker.Get(10);
        faker.ErrorsGenerator(ref query);
        if (query.Count() == 0) return StatusCode(204);// 204 := "No Content"
        return PartialView(query);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult ExportUsers()
    {
        var users = GetUsers(); // Replace this with your method to get users
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var usersSection = new List<IDictionary<string, string>>();
        foreach (var user in users)
        {
            usersSection.Add(new Dictionary<string, string>
        {
            {"Id", user.Id.ToString()},
            {"Name", user.Name},
            {"Email", user.Address},
            {"Phone", user.Telephone}
        });
        }

        config.GetSection("Users").Value = JsonConvert.SerializeObject(usersSection);

        return RedirectToAction("Index"); // Redirect to the index page after saving the users
    }
    private List<Data> GetUsers(int itemsCount = 20, string locale = "ru", float errors = 0, int seed = 0)
    {
        DataFaker faker = new DataFaker(seed, locale, itemsCount, errors);
        var users = faker.Get(itemsCount);
        faker.ErrorsGenerator(ref users);
        return users;
    }

}