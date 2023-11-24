using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliciousMeals.Models
{
    // Represents the Invoice table.
    [Table("Invoice")]
    public class Invoice
    {
        // A primary key.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }

        [Required]
        public int CustId { get; set; }

        [Required]
        public int MealId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateGenerated { get; set; }

        // Reference/Navigation property
        [Required]
        [ForeignKey("CustId")]
        public required Customer Customer { get; set; }

        [Required]
        [ForeignKey("MealId")]
        public required Meal Meal { get; set; }
    }
}
