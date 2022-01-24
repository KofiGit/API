
namespace Api.Models
{
    public class Product
    {
        public Product()
        {
            
        }
        
        public Product(long id, string name, string imgUri, decimal price, string? description = null)
        {
            Id = id;
            Name = name;
            ImgUri = imgUri;
            Price = price;
            Description = description;
        }
        
        public long Id {  get; set; }
        public string Name { get; set; }
        public string ImgUri { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}