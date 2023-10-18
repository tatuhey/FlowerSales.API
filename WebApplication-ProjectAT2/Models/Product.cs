namespace WebApplication_ProjectAT2.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string StoreLocation { get; set; }
        public int PostCode { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        public virtual Category Category { get; set; }


    }
}
