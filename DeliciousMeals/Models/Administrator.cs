using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliciousMeals.Models
{
    // Represents the Administrator table.
    [Table("Administrator")]
    public class Administrator
    {       
        // The admin's email address.
        [Key]
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        // The first name of the admin.
        [Required]
        public required string FirstName { get; set; }

        // The last name of the admin.
        [Required]
        public required string LastName { get; set; }      

        // The admin's phone number.
        [Required]
        [DataType(DataType.PhoneNumber)]
        public required string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
