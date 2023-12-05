using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliciousMeals.Models
{
    // Represents the Meal table.
    [Table("Meal")]
    public class Meal
    {

        // A primary key.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MealId { get; set; }

        // A meal's name.
        [Required]
        public required string Name { get; set; }

        // A meal's description.
        [Required]
        public required string Description { get; set; }

        // A meal's price
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // A meal's quantity.
        [Required]
        public int Quantity { get; set; }

        // A meal's image.
        [Required]
        public required string Image { get; set; }

        // Indicates if this meal is available for ordering.
        [Required]
        [Column(TypeName = "char(1)")]
        public char IsAvailable { get; set; }

        // Indicates the size of the meal.
        [Required]
        public required string Size { get; set; }

        // Meal category.
        [Required]
        public required string Category { get; set; }

        // Navigation properties.
        public ICollection<Cart>? Carts { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
