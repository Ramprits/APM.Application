using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APM.Application.Models
{
    public class Product

    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "You must enter Description !"),
         MaxLength(200, ErrorMessage = "Description must be less then 200"),
         MinLength(2, ErrorMessage = "Description must be greater then 2 charecters ")]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "You must enter Product Code !"),
         MaxLength(10, ErrorMessage = "Product Code must be less then 10"),
         MinLength(2, ErrorMessage = "Product Code must be greater then 2 charecters ")]
        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;
    }
}