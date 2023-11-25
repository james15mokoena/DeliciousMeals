using Microsoft.AspNetCore.Mvc;
using DeliciousMeals.Data;
using DeliciousMeals.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliciousMeals.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly DeliciousMealsDbContext dbContext;

        public AuthenticationController(DeliciousMealsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> LoginVerification(Customer customer)
        {            
            var exCustomer = await dbContext.Customers.FirstOrDefaultAsync(cust => cust.Email == customer.Email);

            if(exCustomer != null)
            {
                if (PasswordEncrypter.VerifyPassword(customer.Password, exCustomer.Password))
                {
                    HttpContext.Session.SetString("email", customer.Email);
                    ViewData["IsLogged"] = "yes";
                    return RedirectToAction("Index", "Home");
                }               
            }
            else
            {
                TempData["ErrorMessage"] = "User doesn't exist consider creating an account.";
                return RedirectToAction("Login","Authentication");
            }

            TempData["ErrorMessage"] = "Incorrect email or password.";
            return RedirectToAction("Login", "Authentication");            
        }

        public IActionResult CreateAdminAccount(Administrator admin)
        {
            return RedirectToAction("Login", "Authentication");
        }
         
        public IActionResult CreateCustomerAccount(Customer customer)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAccountHelper(Customer customer)
        {
            var exCustomer = await dbContext.Customers.FirstOrDefaultAsync(cust =>  customer.Email == customer.Email);

            if(exCustomer == null)
            {
                customer.Password = PasswordEncrypter.HashPassword(customer.Password);
                await dbContext.Customers.AddAsync(customer);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = "User already exists, you can login.";
            return RedirectToAction("Login", "Authentication");
        }

        public IActionResult Logout()
        {
            if(HttpContext.Session.Get("email") != null || ViewData["IsLogged"] != null)
            {
                HttpContext.Session.Remove("email");
                ViewData["IsLogged"] = null;                
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
