namespace SauceDemo.Domain.Entities
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public Product()
        {
            // Default constructor
        }

        public Product(string id, string name, string description, decimal price, string imageUrl)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            ImageUrl = imageUrl;
        }

        public override string ToString()
        {
            return $"{Name} - ${Price}";
        }
    }
}
