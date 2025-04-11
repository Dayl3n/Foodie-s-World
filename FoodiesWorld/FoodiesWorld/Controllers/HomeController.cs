using System.Diagnostics;
using FoodiesWorld.Data;
using FoodiesWorld.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
namespace FoodiesWorld.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User>? _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetEvents()
        {
            var userId = _userManager.GetUserId(User);
            var visits = await _context.Visit
                .Include(v => v.Restaurant)
                .Where(v => v.UserId == userId)
                .ToListAsync();

            var events = visits.Select(v => new {
                title = v.Restaurant.Name,
                start = v.Date.ToString("yyyy-MM-ddTHH:mm:ss") // zak³adam, ¿e masz DateTime Date
            });

            return Json(events);
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
