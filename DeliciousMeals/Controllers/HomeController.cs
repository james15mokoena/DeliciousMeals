using DeliciousMeals.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DeliciousMeals.Data;

namespace DeliciousMeals.Controllers
{
    public class HomeController : Controller
    {
        private readonly DeliciousMealsDbContext dbContext;

        public HomeController(DeliciousMealsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Get("email") != null)
            {                
                ViewData["IsLogged"] = "yes";
            }
            else
            {                
                ViewData["IsLogged"] = null;
            }

            return View();
        }

        public IActionResult DisplayMenu()
        {
            if (HttpContext.Session.Get("email") != null)
            {             
                ViewData["IsLogged"] = "yes";
            }
            else
            {             
                ViewData["IsLogged"] = null;
            }

            return View();
        }
      
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
