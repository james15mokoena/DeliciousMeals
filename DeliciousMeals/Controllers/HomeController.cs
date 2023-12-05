using DeliciousMeals.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DeliciousMeals.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliciousMeals.Controllers
{
    public class HomeController : Controller
    {
        private readonly DeliciousMealsDbContext dbContext;

        public HomeController(DeliciousMealsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.Get("email") != null)
            {                
                ViewData["IsLogged"] = "yes";
            }
            else
            {                
                ViewData["IsLogged"] = null;
            }

            var meals = dbContext.Meals.DefaultIfEmpty();

            if(meals != null)
            {
                return View(await meals.ToListAsync());
            }
            else
            {
                ViewData["MealStatus"] = "No meals currently";                
            }            

            return View();
        }

        public async Task<IActionResult> DisplayMeal(int? MealId)
        {
            if (HttpContext.Session.Get("email") != null)
            {
                ViewData["IsLogged"] = "yes";
            }
            else
            {
                ViewData["IsLogged"] = null;
            }

            var meal = await dbContext.Meals.FirstOrDefaultAsync(meal => meal.MealId == MealId);

            if(meal != null)
            {
                return View(meal);
            }

            return View();
        }

        public IActionResult Reviews(int? MealId)
        {
            return View();
        }

        public async Task<IActionResult> DeleteMealFromCart(int? mealId)
        {

            if (HttpContext.Session.Get("email") != null)
            {

                byte[]? emailBytes = HttpContext.Session.Get("email");

                if (emailBytes != null)
                {
                    char[] emailArray = new char[emailBytes.Length];

                    for (int i = 0; i < emailArray.Length; ++i)
                    {
                        emailArray[i] = (char)emailBytes[i];
                    }

                    string email = new(emailArray);

                    var cartMeal = await dbContext.Carts.FirstOrDefaultAsync(m => m.MealId == mealId && m.Email == email);

                    if (cartMeal != null)
                    {
                        dbContext.Carts.Remove(cartMeal);
                        await dbContext.SaveChangesAsync();
                        return RedirectToAction("DisplayCart", "Home");
                    }

                }
            }
            
            TempData["Response"] = "Meal is no longer in the cart.";
            return RedirectToAction("DisplayCart", "Home");
        }

        public async Task<IActionResult> DecreaseMealQuantity(int? mealId)
        {
            if(HttpContext.Session.Get("email") != null)
            {
                byte[]? emailBytes = HttpContext.Session.Get("email");

                if(emailBytes != null)
                {
                    char[] emailChars = new char[emailBytes.Length];

                    for(int i=0; i<emailBytes.Length; ++i)
                    {
                        emailChars[i] = (char) emailBytes[i];
                    }

                    string email = new(emailChars);

                    var cartItem = await dbContext.Carts.FirstOrDefaultAsync(item => item.MealId == mealId && item.Email == email);

                    if (cartItem != null)
                    {
                        int quantity = cartItem.Quantity;

                        if(quantity - 1 > 0)
                        {
                            cartItem.Quantity -= 1;
                            dbContext.Update(cartItem);
                            await dbContext.SaveChangesAsync();
                            return RedirectToAction("DisplayCart", "Home");
                        }
                        else
                        {
                            TempData["Notification"] = "If you want to remove a meal from the cart, click the 'Remove' button.";
                            return RedirectToAction("DisplayCart", "Home");
                        }
                    }
                }
            }

            TempData["ErrorMessage"] = "Meal is no longer in your cart.";
            return RedirectToAction("DisplayCart","Home");
        }

        public async Task<IActionResult> IncreaseMealQuantity(int? mealId)
        {
            if (HttpContext.Session.Get("email") != null)
            {
                byte[]? emailBytes = HttpContext.Session.Get("email");

                if (emailBytes != null)
                {
                    char[] emailChars = new char[emailBytes.Length];

                    for (int i = 0; i < emailBytes.Length; ++i)
                    {
                        emailChars[i] = (char)emailBytes[i];
                    }

                    string email = new(emailChars);

                    var cartItem = await dbContext.Carts.FirstOrDefaultAsync(item => item.MealId == mealId && item.Email == email);
                    var meal = await dbContext.Meals.FirstOrDefaultAsync(m => m.MealId == mealId);

                    if (cartItem != null && meal != null)
                    {
                        int quantity = cartItem.Quantity;                        

                        if ((quantity + 1) <= meal.Quantity)
                        {
                            cartItem.Quantity += 1;
                            dbContext.Update(cartItem);
                            await dbContext.SaveChangesAsync();
                            return RedirectToAction("DisplayCart", "Home");
                        }
                        else
                        {
                            TempData["Notification"] = "The quantity exceeds the available meal quantity.";
                            return RedirectToAction("DisplayCart", "Home");
                        }
                    }
                }
            }

            TempData["ErrorMessage"] = "Meal is no longer in your cart.";
            return RedirectToAction("DisplayCart", "Home");
        }

        public async Task<IActionResult> AddToCart(int? mealId)
        {
            if (HttpContext.Session.Get("email") != null)
            {
                ViewData["IsLogged"] = "yes";

                var meal = await dbContext.Meals.FirstOrDefaultAsync(meal => meal.MealId == mealId);
                byte[]? emailBytes = HttpContext.Session.Get("email");   

                if(emailBytes != null)
                {
                    char[] emailArray = new char[emailBytes.Length];

                    for(int i=0; i<emailArray.Length; ++i)
                    {
                        emailArray[i] = (char)emailBytes[i];
                    }

                    string email = new (emailArray);

                    var cartItem = await dbContext.Carts.FirstOrDefaultAsync(cItem => cItem.MealId == mealId && cItem.Email == email);
                    var customer = await dbContext.Customers.FirstOrDefaultAsync(cust => cust.Email == email);

                    if (meal != null && meal.IsAvailable == 'y' && email != null && mealId != null && cartItem == null && customer != null)
                    {

                        Cart newCartItem = new() 
                        {
                            Email = email,
                            MealId = (int)mealId,
                            Quantity = 1,
                            Price = meal.Price,
                            Customer = customer,
                            Meal = meal
                        };

                        await dbContext.Carts.AddAsync(newCartItem);
                        await dbContext.SaveChangesAsync();
                        return RedirectToAction("DisplayCart", "Home");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error while adding meal to the cart." + email;
                        return RedirectToAction("DisplayCart", "Home");
                    }

                }
            }
            else
            {
                ViewData["IsLogged"] = null;
                TempData["ErrorMessage"] = "Please login, before adding a meal to your cart.";
                return RedirectToAction("Login", "Authentication");
            }
            return RedirectToAction("DisplayCart","Home");
        }

        public async Task<IActionResult> DisplayByCategory(string? Category)
        {
            if (HttpContext.Session.Get("email") != null)
            {
                ViewData["IsLogged"] = "yes";
            }
            else
            {
                ViewData["IsLogged"] = null;
            }

            var meals = dbContext.Meals.Where(meal => meal.Category == Category);

            if(meals != null)
            {
                return View(await meals.ToListAsync());
            }
            else
            {
                TempData["Response"] = "Meals in this category aren't currently available.";
                return RedirectToAction("DisplayByCategory", "Home");
            }            
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

        public async Task<IActionResult> DisplayCart()
        {            

            if (HttpContext.Session.Get("email") != null)
            {
                ViewData["IsLogged"] = "yes";
                byte[]? emailBytes = HttpContext.Session.Get("email");

                if(emailBytes != null)
                {
                    char[] emailArray = new char[emailBytes.Length];

                    for(int i=0; i<emailArray.Length; i++)
                    {
                        emailArray[i] = (char) emailBytes[i];
                    }

                    string email = new(emailArray);
                    var cart = dbContext.Carts.Where(cartItem => cartItem.Email == email);

                    if (cart != null)
                    {
                        int count = 0;
                        foreach (Cart ct in cart)
                        {
                            if (ct != null)
                                ++count;
                        }

                        if (count > 0)
                            return View(await cart.ToListAsync());
                        else
                            TempData["Response"] = "Cart is empty.";
                    }
                    else
                    {
                        TempData["Response"] = "Cart is empty.";
                    }
                }                           
            }
            else
            {
                ViewData["IsLogged"] = null;
                TempData["ErrorMessage"] = "Please login, before accessing your cart.";
                return RedirectToAction("Login", "Authentication");
            }

            return View();
        }

        public IActionResult DisplayInvoice()
        {
            if(HttpContext.Session.Get("email") != null)
            {
                ViewData["IsLogged"] = "yes";
                byte[]? emailBytes = HttpContext.Session.Get("email");

                if(emailBytes != null)
                {
                    char[] emailChars = new char[emailBytes.Length];

                    for(int i=0; i<emailBytes.Length; i++)
                    {
                        emailChars[i] = (char)emailBytes[i];
                    }

                    string email = new(emailChars);
                    var invoices = dbContext.Invoices.Where(inv => inv.Email == email);

                    if (invoices != null)
                    {
                        int count = 0;
                        foreach (Invoice invoice in invoices)
                        {
                            if (invoice != null) ++count;
                        }

                        if (count > 0)
                            TempData["Response"] = "You have an invoice.";
                        else
                            TempData["Response"] = "You don't have an invoice.";
                    }
                }                
            }
            else
            {
                ViewData["IsLogged"] = null;
                TempData["ErrorMessage"] = "Please login, before accessing your cart.";
                return RedirectToAction("Login", "Authentication");
            }

            return View();
        }
       
        public IActionResult Checkout()
        {
            if(HttpContext.Session.Get("email") != null)
            {
                ViewData["IsLogged"] = "yes";

                return View();
            }

            TempData["ErrorMessage"] = "Please login, before accessing the checkout page.";
            return RedirectToAction("Login","Authentication");
        }

        public async Task<IActionResult> TempOrder()
        {
            if (HttpContext.Session.Get("email") != null)
            {
                ViewData["IsLogged"] = "yes";

                byte[]? emailBytes = HttpContext.Session.Get("email");

                if (emailBytes != null)
                {
                    char[] emailChars = new char[emailBytes.Length];

                    for (int i = 0; i < emailBytes.Length; i++)
                    {
                        emailChars[i] = (char)emailBytes[i];
                    }

                    string email = new(emailChars);

                    var orders = dbContext.Orders.Where(order => order.Email == email);

                    if(orders != null && orders.Any())
                    {
                        return View(await orders.ToListAsync());
                    }
                    else
                    {
                        TempData["Response"] = "You don't have pending orders.";
                        return View();
                    }
                }
            }

            TempData["ErrorMessage"] = "Please login before accessing your orders.";
            return RedirectToAction("Login","Authentication");
        }
       
        public async Task<IActionResult> Order()
        {
            if(HttpContext.Session.Get("email") != null)
            {
                ViewData["IsLogged"] = "yes";

                byte[]? emailBytes = HttpContext.Session.Get("email");

                if(emailBytes != null)
                {
                    char[] emailChars = new char[emailBytes.Length];

                    for(int i=0; i<emailBytes.Length; i++)
                    {
                        emailChars[i] = (char) emailBytes[i];
                    }

                    string email = new(emailChars);

                    var cartItems = dbContext.Carts.Where(ct => ct.Email == email);                    
                    var customer = await dbContext.Customers.FirstOrDefaultAsync(cust => cust.Email == email);                    

                    if (cartItems != null && cartItems.Any() && customer != null)
                    {
                        foreach(Cart item in cartItems)
                        {                                               
                            if (item != null)
                            {                                
                                Order newOrder = new Order
                                {
                                    MealId = item.MealId,
                                    IsReady = 'n',
                                    IsCollected = 'n',
                                    TimeCompleted = DateTime.Now,
                                    DateOrdered = DateTime.Now,
                                    Email = email,
                                    Customer = customer,
                                    Meal = item.Meal
                                };

                                dbContext.Orders.Add(newOrder);                                                               
                            }
                        }

                        var meals = dbContext.Meals.Where(m => m.IsAvailable == 'y');

                        if(meals != null)
                        {
                            foreach(Cart cartItem in cartItems)
                            {                                                                            
                                foreach(Meal meal in meals)
                                {
                                    if(cartItem != null && meal != null)
                                    {
                                        if(cartItem.MealId == meal.MealId)
                                        {
                                            meal.Quantity -= cartItem.Quantity;
                                            dbContext.Update(meal);
                                        }
                                    }                                
                                }                                
                            }
                        }

                        dbContext.RemoveRange(cartItems);
                        await dbContext.SaveChangesAsync();

                        var orders = dbContext.Orders.Where(o => o.Email == email);

                        if(orders != null && orders.Any())
                        {
                            return View(await orders.ToListAsync());
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Failed to place your order(s).";
                            return RedirectToAction("Order", "Home");
                        }
                    }
                }                
            }
           
            TempData["ErrorMessage"] = "Please login before accessing your orders.";
            return RedirectToAction("Login", "Authentication");           
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
