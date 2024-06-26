﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliciousMeals.Models
{
    // Represents the Customer's table.
    [Table("Customer")]
    public class Customer
    {      
        // A customer's email address
        [Key]
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        // A customer's first name 
        [Required]
        public required string FirstName { get; set; }

        // A customer's last name
        [Required]
        public required string LastName { get; set; }        

        // A customer's phone number        
        [Required]
        [DataType(DataType.PhoneNumber)]
        public required string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        // Navigation properties
        public ICollection<Cart>? CartItems { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }

        public ICollection<Order>? Orders { get; set; }

    }
}
