﻿using System.ComponentModel.DataAnnotations;
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

        // A Customer' email.
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        // A meal's id.
        [Required]
        public int MealId { get; set; }

        [Required]
        public required string MealName { get; set; }

        [Required]
        public required string MealImg { get; set; }

        // A meal's quantity on the cart.
        [Required]
        public int Quantity { get; set; }

        // The total price of for this cart item.
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Reference/Navigation property
        [Required]
        [ForeignKey("Email")]
        public required Customer Customer { get; set; }

        [Required]
        [ForeignKey("MealId")]
        public required Meal Meal { get; set; }
    }
}
