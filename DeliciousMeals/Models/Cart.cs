using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliciousMeals.Models
{
    // Represents a Cart table.
    [Table("Cart")]
    public class Cart
    {
        // A primary key.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        // A Customer' id.
        [Required]
        public int CustId { get; set; }

        // A meal's id.
        [Required]
        public int MealId { get; set; }

        // A meal's quantity on the cart.
        [Required]
        public int Quantity { get; set; }

        // The total price of for this cart item.
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Reference/Navigation property
        [Required]
        [ForeignKey("CustId")]
        public required Customer Customer { get; set; }

        [Required]
        [ForeignKey("MealId")]
        public required Meal Meal { get; set; }
    }
}
