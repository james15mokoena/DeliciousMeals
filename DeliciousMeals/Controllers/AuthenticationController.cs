using Microsoft.AspNetCore.Mvc;
using DeliciousMeals.Data;
using DeliciousMeals.Models;
using Microsoft.EntityFrameworkCore;
using DeliciousMeals.Security;

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
            if (HttpContext.Session.GetString("email") != null)
            {
                ViewData["IsLogged"] = "yes";
            }

            return View();
        }

        public IActionResult AdminLogin()
        {
            if(HttpContext.Session.GetString("adminLogin") != null)
            {
                ViewData["AdminLogged"] = "yes";
            }           

            return View();
        }

        public async Task<IActionResult> CustomerLoginVerification(Customer customer)
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

        public IActionResult CreateAdminAccount()
        {
            if(HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdminAccountHelper(Administrator admin)
        {
            var exAdmin = await dbContext.Administrators.FirstOrDefaultAsync(a => a.Email == admin.Email);

            if(exAdmin == null)
            {
                if (admin.Email.Contains(".deliciousmeals.admin.inpower"))
                {
                    admin.Password = PasswordEncrypter.HashPassword(admin.Password);
                    await dbContext.Administrators.AddAsync(admin);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction("AdminLogin", "Authentication");
                }
                else
                {
                    TempData["Response"] = "What do you think you are doing?";
                    return RedirectToAction("CreateAdminAccount", "Authentication");
                }
            }

            TempData["Response"] = "Admin already exists, please login.";
            return RedirectToAction("AdminLogin", "Authentication");
        }               

        public async Task<IActionResult> AdminLoginVerification(Administrator admin)
        {
            var exAdmin = await dbContext.Administrators.FirstOrDefaultAsync(a => a.Email == admin.Email);

            if(exAdmin != null)
            {
                if (PasswordEncrypter.VerifyPassword(admin.Password, exAdmin.Password))
                {
                    HttpContext.Session.SetString("adminEmail", exAdmin.Email);
                    ViewData["AdminLogged"] = "yes";
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    TempData["Response"] = "Incorrect email or password.";
                    return RedirectToAction("AdminLogin", "Authentication");
                }
            }

            TempData["Response"] = "This admin doesn't exist.";
            return RedirectToAction("AdminLogin","Authentication");
        }

        [HttpGet]
        public IActionResult CreateCustomerAccount()
        {
            if(HttpContext.Session.GetString("email") != null)
            {
                ViewData["IsLogged"] = "yes";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAccountHelper(Customer customer)
        {
            var exCustomer = await dbContext.Customers.FirstOrDefaultAsync(cust =>  cust.Email == customer.Email);

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
            if(HttpContext.Session.GetString("email") != null || ViewData["IsLogged"] != null)
            {
                HttpContext.Session.Remove("email");
                ViewData["IsLogged"] = null;                
            }

            if(HttpContext.Session.GetString("adminEmail") != null || ViewData["AdminLogged"] != null)
            {
                HttpContext.Session.Remove("adminEmail");
                ViewData["AdminLogged"] = null;
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
