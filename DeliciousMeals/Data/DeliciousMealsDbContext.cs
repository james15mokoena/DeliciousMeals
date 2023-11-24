using Microsoft.EntityFrameworkCore;
using DeliciousMeals.Models;

namespace DeliciousMeals.Data
{
    /*
        This class serves as a bridge between the application
        and the database, by connecting the application to the database.
     */
    public class DeliciousMealsDbContext : DbContext
    {
        public DeliciousMealsDbContext(DbContextOptions<DeliciousMealsDbContext> options) : base(options)
        {
            // left empty on purpose.
        }

        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}
