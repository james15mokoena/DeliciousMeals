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

        private void CheckCustomerSession()
        {
            if (HttpContext.Session.GetString("email") != null)
            {
                ViewData["IsLogged"] = "yes";
            }
            else
            {
                ViewData["IsLogged"] = null;                
            }
        }

        public void CheckAdminSession()
        {
            if (HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
            }
            else
            {
                ViewData["AdminLogged"] = null;
            }
        }

        public async Task<IActionResult> Index()
        {
            CheckCustomerSession();
            CheckAdminSession();

            var meals = dbContext.Meals.Where(m => m.IsAvailable == 'Y');

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
            CheckCustomerSession();
            CheckAdminSession();

            var meal = await dbContext.Meals.FirstOrDefaultAsync(meal => meal.MealId == MealId);

            if(meal != null)
            {
                return View(meal);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            if(HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
                var orders = dbContext.Orders.DefaultIfEmpty();

                if (orders != null && orders.Any())
                {
                    return View(await orders.ToListAsync());
                }
                else
                {
                    TempData["Response"] = "No pending orders";
                    return RedirectToAction("Orders","Home");
                }
            }
            else
            {
                ViewData["AdminLogged"] = null;
            }

            return RedirectToAction("AdminLogin","Authentication");
        }

        public IActionResult EditOrder()
        {
            if(HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
                return View();
            }

            return RedirectToAction("AdminLogin","Authentication");
        }

        public async Task<IActionResult> EditOrderHelper(Order order)
        {
            var exOrder = await dbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            if(exOrder != null)
            {
                exOrder.IsReady = order.IsReady;
                exOrder.IsCollected = order.IsCollected;
                exOrder.TimeCompleted = DateTime.Now;

                if(exOrder.IsReady == 'Y' && exOrder.IsCollected == 'Y')
                {
                    dbContext.Orders.Remove(exOrder);
                }
                else
                {
                    dbContext.Orders.Update(exOrder);                    
                }

                await dbContext.SaveChangesAsync();
                return RedirectToAction("Orders", "Home");
            }
            else
            {
                TempData["Response"] = "This order doesn't exist.";
            }
            return RedirectToAction("EditOrder", "Home");
        }

        public async Task<IActionResult> Reviews(int? mealId)
        {
            CheckCustomerSession();
            CheckAdminSession();

            var reviews = dbContext.Reviews.Where(m => m.MealId == mealId);
            ViewData["MealId"] = mealId;
            int? mId = mealId;            
            if(mId != null)
            {
                int m = (int)mId;
                HttpContext.Session.SetInt32("MealId", m);
            }            

            if (reviews != null && reviews.Any())
            {                
                return View(await reviews.ToListAsync());
            }
            else
            {
                TempData["Response"] = "This meal does not have reviews.";
                return View();
            }
        }

        public IActionResult AddReview(int? mealId)
        {
            CheckCustomerSession();            

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(Review rev)
        {
            if(HttpContext.Session.GetString("email") != null)
            {
                ViewData["IsLogged"] = "yes";
                string? nullableEmail = HttpContext.Session.GetString("email");

                if(nullableEmail != null)
                {                    
                    string email = nullableEmail;
                    var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Email == email);
                    int? nullableMealId = HttpContext.Session.GetInt32("MealId");                    

                    if (nullableMealId != null)
                    {
                        int mealId = (int) nullableMealId;
                        
                        var meal = await dbContext.Meals.FirstOrDefaultAsync(m => m.MealId == mealId);

                        // check if the current user purchased the meal before allowing him/her to 
                        // write a review of the meal.
                        var invoice = await dbContext.Invoices.FirstOrDefaultAsync(i => i.MealId == mealId && i.Email == email);
                        var exRev = await dbContext.Reviews.FirstOrDefaultAsync(r => r.MealId == mealId && r.Email == email);

                        // Already review the meal.
                        if(invoice != null && exRev == null)
                        {
                            if (rev != null && customer != null && meal != null)
                            {
                                rev.MealId = mealId;
                                rev.Customer = customer;
                                rev.Meal = meal;
                                rev.Email = email;
                                await dbContext.AddAsync(rev);
                                await dbContext.SaveChangesAsync();
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else if(invoice != null && exRev != null)
                        {
                            TempData["Response"] = "Cannot review this meal again, because you already reviewed this meal.";
                        }
                        else
                        {
                            TempData["Response"] = "You cannot review a meal that you haven't purchased/ordered before on this website.";                                
                        }

                        return RedirectToAction("AddReview", "Home");                        
                    }                                                                                
                }                
            }
            else
            {
                TempData["ErrorMessage"] = "Please login, before reviewing a meal.";
            }
            return RedirectToAction("Login", "Authentication");
        }

        public async Task<IActionResult> DeleteMealFromCart(int? mealId)
        {

            if (HttpContext.Session.GetString("email") != null)
            {
                string? nullableEmail = HttpContext.Session.GetString("email");

                if (nullableEmail != null)
                {
                    string email = nullableEmail;
                    var cartMeal = await dbContext.Carts.FirstOrDefaultAsync(m => m.MealId == mealId && m.Email == email);
                    var meal = await dbContext.Meals.FirstOrDefaultAsync(m=> m.MealId == mealId);

                    if (cartMeal != null && meal != null)
                    {
                        meal.Quantity += cartMeal.Quantity;

                        if (meal.Quantity > 0)
                        {
                            meal.IsAvailable = 'Y';
                        }
                        dbContext.Meals.Update(meal);
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
            if(HttpContext.Session.GetString("email") != null)
            {
                string? nullableEmail = HttpContext.Session.GetString("email");

                if (nullableEmail != null)
                {
                    string email = nullableEmail;

                    var cartItem = await dbContext.Carts.FirstOrDefaultAsync(item => item.MealId == mealId && item.Email == email);
                    var meal = await dbContext.Meals.FirstOrDefaultAsync(m => m.MealId == mealId);

                    if (cartItem != null && meal != null)
                    {
                        int quantity = cartItem.Quantity;

                        if(quantity - 1 > 0)
                        {
                            cartItem.Quantity -= 1;
                            meal.Quantity += 1;

                            if (meal.Quantity > 0)
                            {
                                meal.IsAvailable = 'Y';
                            }

                            dbContext.Meals.Update(meal);
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
            if (HttpContext.Session.GetString("email") != null)
            {
                string? nullableEmail = HttpContext.Session.GetString("email");

                if (nullableEmail != null)
                {                    
                    string email = nullableEmail;
                    var cartItem = await dbContext.Carts.FirstOrDefaultAsync(item => item.MealId == mealId && item.Email == email);
                    var meal = await dbContext.Meals.FirstOrDefaultAsync(m => m.MealId == mealId);

                    if (cartItem != null && meal != null)
                    {
                        int quantity = cartItem.Quantity;                        

                        if ((quantity + 1) <= meal.Quantity || meal.Quantity > 0)
                        {
                            cartItem.Quantity += 1;
                            meal.Quantity -= 1;

                            if (meal.Quantity <= 0)
                            {
                                meal.IsAvailable = 'N';
                            }

                            dbContext.Meals.Update(meal);
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
            if (HttpContext.Session.GetString("email") != null)
            {
                ViewData["IsLogged"] = "yes";

                string? nullableEmail = HttpContext.Session.GetString("email");

                if (nullableEmail != null)
                {                    
                    string email = nullableEmail;
                    var meal = await dbContext.Meals.FirstOrDefaultAsync(meal => meal.MealId == mealId);
                    var cartItem = await dbContext.Carts.FirstOrDefaultAsync(cItem => cItem.MealId == mealId && cItem.Email == email);
                    var customer = await dbContext.Customers.FirstOrDefaultAsync(cust => cust.Email == email);                    

                    if (meal != null && meal.IsAvailable == 'Y' && email != null && mealId != null && cartItem == null && customer != null)
                    {

                        Cart newCartItem = new() 
                        {
                            Email = email,
                            MealId = (int)mealId,
                            Quantity = 1,
                            Price = meal.Price,
                            Customer = customer,
                            Meal = meal,
                            MealName = meal.Name,
                            MealImg = meal.Image
                        };
                        
                        meal.Quantity -= 1;

                        if (meal.Quantity <= 0)
                        {
                            meal.IsAvailable = 'N';
                        }
                       
                        dbContext.Meals.Update(meal);                        
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
            CheckCustomerSession();            

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
            CheckCustomerSession();            

            return View();
        }

        public async Task<IActionResult> DisplayCart()
        {            

            if (HttpContext.Session.GetString("email") != null)
            {
                ViewData["IsLogged"] = "yes";
                string? nullableEmail = HttpContext.Session.GetString("email");

                if (nullableEmail != null)
                {                    
                    string email = nullableEmail;
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

        public async Task<IActionResult> DisplayInvoice()
        {
            if(HttpContext.Session.GetString("email") != null)
            {
                ViewData["IsLogged"] = "yes";
                string? nullableEmail = HttpContext.Session.GetString("email");

                if (nullableEmail != null)
                {
                    string email = nullableEmail;                    
                    var invoices = dbContext.Invoices.Where(inv => inv.Email == email);

                    if (invoices != null)
                    {
                        int count = 0;
                        foreach (Invoice invoice in invoices)
                        {
                            if (invoice != null) ++count;
                        }

                        if (count > 0)
                            return View(await invoices.ToListAsync());
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
            if(HttpContext.Session.GetString("email") != null)
            {
                ViewData["IsLogged"] = "yes";
                return View();
            }

            TempData["ErrorMessage"] = "Please login, before accessing the checkout page.";
            return RedirectToAction("Login","Authentication");
        }

        public async Task<IActionResult> TempOrder()
        {
            if (HttpContext.Session.GetString("email") != null)
            {
                ViewData["IsLogged"] = "yes";

                string? nullableEmail = HttpContext.Session.GetString("email");

                if (nullableEmail != null)
                {
                    string email = nullableEmail;

                    var orders = dbContext.Orders.Where(order => order.Email == email && (order.IsCollected == 'N' && (order.IsReady == 'Y' || order.IsReady == 'N')));

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
            if(HttpContext.Session.GetString("email") != null)
            {
                ViewData["IsLogged"] = "yes";

                string? nullableEmail = HttpContext.Session.GetString("email");

                if (nullableEmail != null)
                {
                    string email = nullableEmail;
                    var cartItems = dbContext.Carts.Where(ct => ct.Email == email);                    
                    var customer = await dbContext.Customers.FirstOrDefaultAsync(cust => cust.Email == email);                    

                    if (cartItems != null && cartItems.Any() && customer != null)
                    {
                        foreach(Cart item in cartItems)
                        {                                               
                            if (item != null)
                            {                                
                                Order newOrder = new ()
                                {
                                    MealId = item.MealId,
                                    IsReady = 'N',
                                    IsCollected = 'N',
                                    TimeCompleted = null,
                                    DateOrdered = DateTime.Now,
                                    Email = email,
                                    Customer = customer,
                                    Meal = item.Meal,
                                    MealName = item.MealName,
                                    Quantity = item.Quantity,
                                };

                                dbContext.Orders.Add(newOrder);                                                               
                            }
                        }
       
                        foreach (Cart item in cartItems)
                        {
                            if (item != null)
                            {
                                Invoice newInv = new()
                                {
                                    MealId = item.MealId,
                                    Quantity = item.Quantity,
                                    Price = item.Quantity * item.Price,
                                    DateGenerated = DateTime.Now,
                                    Email = item.Email,
                                    Customer = item.Customer,
                                    Meal = item.Meal,
                                    MealName = item.MealName
                                };

                                PurchasedMeal pMeal = new()
                                {
                                    MealId = item.MealId,
                                    MealName = item.MealName,
                                    Quantity = item.Quantity,
                                    Email = item.Email,
                                    DatePurchased = DateTime.Now,
                                    Meal = item.Meal,
                                    TotalPrice = item.Price * item.Quantity,
                                    Customer = item.Customer
                                };

                                await dbContext.PurchasedMeals.AddAsync(pMeal);
                                await dbContext.Invoices.AddAsync(newInv);
                            }
                        }

                        dbContext.RemoveRange(cartItems);
                        await dbContext.SaveChangesAsync();

                        var orders = dbContext.Orders.Where(o => o.Email == email && (o.IsReady == 'N' && o.IsCollected == 'N' || o.IsReady == 'Y' && o.IsCollected == 'N'));

                        if (orders != null && orders.Any())
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

        [HttpGet]
        public async Task<IActionResult> ViewMeals()
        {
            if(HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
                var meals = dbContext.Meals.DefaultIfEmpty();

                if (meals != null)
                {
                    return View(await meals.ToListAsync());
                }
                else
                {
                    TempData["Response"] = "No meals are currently available in the database.";
                    return View();
                }
            }            

            return RedirectToAction("AdminLogin","Authentication");
        }

        [HttpGet]
        public IActionResult AddMeal()
        {
            if (HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
                return View();
            }

            return RedirectToAction("AdminLogin","Authentication");
        }

        public async Task<IActionResult> AddMeal(Meal meal)
        {
            if (HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
                var exMeal = await dbContext.Meals.FirstOrDefaultAsync(m=> m.MealId == meal.MealId);

                if (exMeal == null)
                {
                    await dbContext.Meals.AddAsync(meal);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction("ViewMeals", "Home");
                }
                else
                {
                    TempData["Response"] = "This meal already exists.";
                    return View();
                }
            }

            return RedirectToAction("AdminLogin","Authentication");
        }

        [HttpGet]
        public IActionResult EditMeal()
        {
            if (HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
                return View();
            }

            return RedirectToAction("AdminLogin", "Authentication");
        }

        public async Task<IActionResult> EditMeal(Meal meal)
        {
            if (HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
                var exMeal = await dbContext.Meals.FirstOrDefaultAsync(m => m.MealId == meal.MealId);

                if (exMeal != null)
                {
                    if (meal.Name != null && !meal.Name.Equals(""))
                        exMeal.Name = meal.Name;

                    if(meal.Description != null && !meal.Description.Equals(""))
                        exMeal.Description = meal.Description;

                    if (meal.Price >= 0)                                           
                        exMeal.Price = meal.Price;

                    if (meal.Quantity >= 0)
                        exMeal.Quantity = meal.Quantity;

                    if (meal.Image != null && !meal.Image.Equals(""))
                        exMeal.Image = meal.Image;

                    if (!meal.IsAvailable.Equals(""))
                        exMeal.IsAvailable = meal.IsAvailable;

                    if (meal.Category != null && !meal.Category.Equals(""))
                        exMeal.Category = meal.Category;

                    if (meal.Size != null && !meal.Size.Equals(""))
                        exMeal.Size = meal.Size;

                    dbContext.Meals.Update(exMeal);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction("ViewMeals", "Home");
                }
                else
                {
                    TempData["Response"] = "Cannot edit a non-existing meal.";
                    return View();
                }
            }

            return RedirectToAction("AdminLogin", "Authentication");
        }

        [HttpGet]
        public IActionResult DeleteMeal()
        {
            if (HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
                return View();
            }

            return RedirectToAction("AdminLogin", "Authentication");
        }

        public async Task<IActionResult> DeleteMeal(int mealId)
        {
            if (HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
                var meal = await dbContext.Meals.FirstOrDefaultAsync(m=> m.MealId == mealId);

                if (meal != null)
                {
                    dbContext.Meals.Remove(meal);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction("ViewMeals","Home");
                }
                else
                {
                    TempData["Response"] = "Cannot delete a non-existing meal.";
                    return View();
                }
            }

            return RedirectToAction("AdminLogin", "Authentication");
        }

        public async Task<IActionResult> DeleteInvoice()
        {
            if (HttpContext.Session.GetString("email") != null)
            {
                ViewData["IsLogged"] = "yes";
                string? nullableEmail = HttpContext.Session.GetString("email");

                if(nullableEmail != null)
                {                    
                    var invoices = dbContext.Invoices.Where(i => i.Email == nullableEmail);

                    if(invoices != null && invoices.Any())
                    {
                        dbContext.Invoices.RemoveRange(invoices);
                        await dbContext.SaveChangesAsync();
                        return RedirectToAction("DisplayInvoice","Home");
                    }
                    else
                    {
                        TempData["Response"] = "You don't have an invoice.";
                        return View();
                    }
                }                
            }

            TempData["ErrorMessage"] = "Please login before accessing your invoice.";
            return RedirectToAction("Login","Authentication");
        }

        public IActionResult SalesReport()
        {
            if (HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
                return View();
            }

            TempData["Response"] = "Please login before accessing the sales report.";
            return RedirectToAction("AdminLogin","Authentication");
        }

        public async Task<IActionResult> DisplayPurchasedMeals()
        {
            if(HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
                var purchasedMeals = dbContext.PurchasedMeals.DefaultIfEmpty();                

                if(purchasedMeals != null && purchasedMeals.Any())
                {                                       
                    return View(await purchasedMeals.ToListAsync());
                }
                else
                {
                    TempData["Response"] = "No meals have been purchased yet.";
                    return View();
                }
            }

            TempData["Response"] = "Please login before accessing the meals report.";
            return RedirectToAction("AdminLogin","Authentication");
        }

        public IActionResult DisplayGraphicReports()
        {
            if (HttpContext.Session.GetString("adminEmail") != null)
            {
                ViewData["AdminLogged"] = "yes";
                return View();
            }

            TempData["Response"] = "Please login before accessing the graphic reports.";
            return RedirectToAction("AdminLogin", "Authentication");
        }

        public async Task<IActionResult> FilterPurchasedMeals(DateTime fromDate, DateTime toDate)
        {          
            if (HttpContext.Session.GetString("adminEmail") != null)
            {
                DateTime f = fromDate;
                DateTime t = toDate;

                ViewData["AdminLogged"] = "yes";

                var purchasedMeals = dbContext.PurchasedMeals.Where(m => m.DatePurchased >= fromDate && m.DatePurchased <= toDate);

                if(purchasedMeals != null && purchasedMeals.Any())
                {
                    return View(await purchasedMeals.ToListAsync());
                }
                else
                {
                    TempData["Response"] = "No meals were purchased within that period.";
                    return RedirectToAction("DisplayPurchasedMeals","Home");
                }                
            }

            TempData["Response"] = "Please login before accessing the graphic reports.";
            return RedirectToAction("AdminLogin", "Authentication");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
