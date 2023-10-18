using System.ComponentModel.DataAnnotations;

namespace WebApplication_ProjectAT2.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string StoreLocation { get; set; }
        public int PostCode { get; set; }
        public decimal Price { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        public virtual Category? Category { get; set; }


    }
}
