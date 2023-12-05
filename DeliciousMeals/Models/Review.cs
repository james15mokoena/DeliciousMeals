using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliciousMeals.Models
{
    [Table("Review")]
    public class Review
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RevId { get; set; }

        [Required]
        public int MealId { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public required string Comment { get; set; }

        [Required]
        [ForeignKey("Email")]
        public required Customer Customer { get; set; }

        [Required]
        [ForeignKey("MealId")]
        public required Meal Meal { get; set; }
    }
}
