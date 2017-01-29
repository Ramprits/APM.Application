using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APM.Application.Models
{
    public class Product

    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime ReleaseDate { get; set; }

    }
}