using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliciousMeals.Models
{
    [Table("PurchasedMeal")]
    public class PurchasedMeal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int PurchaseId { get; set; }

        [Required]
        public int MealId { get; set; }

        [Required]
        public required string MealName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]        
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DatePurchased { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [ForeignKey("MealId")]
        public required Meal Meal { get; set; }

        [Required]
        [ForeignKey("Email")]
        public required Customer Customer { get; set; }
    }
}
