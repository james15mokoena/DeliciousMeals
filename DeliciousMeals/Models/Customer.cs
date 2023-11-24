using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliciousMeals.Models
{
    // Represents the Customer's table.
    [Table("Customer")]
    public class Customer
    {
        // A primary key and a unique identifier for the customer.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustId { get; set; }

        // A customer's first name 
        [Required]
        public required string FirstName { get; set; }

        // A customer's last name
        [Required]
        public required string LastName { get; set; }

        // A customer's email address
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        // A customer's phone number
        [Required]
        [DataType(DataType.PhoneNumber)]
        public required string Phone { get; set; }

        // Navigation properties
        public ICollection<Cart>? CartItems { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }

        public ICollection<Order>? Orders { get; set; }

    }
}
