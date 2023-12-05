using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DeliciousMeals.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        // A Customer' email.
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required]
        public int MealId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateOrdered { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        public char IsReady { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        public char IsCollected { get; set; }

        [AllowNull]
        [DataType(DataType.Time)]
        public DateTime TimeCompleted { get; set; }

        // Reference/Navigation property
        [Required]
        [ForeignKey("Email")]
        public required Customer Customer { get; set; }

        [Required]
        [ForeignKey("MealId")]
        public required Meal Meal { get; set; }
    }
}
