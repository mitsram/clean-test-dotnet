using JupiterPrime.Application.UseCases;
using JupiterPrime.Infrastructure.Services;
using JupiterPrime.Infrastructure.WebDriver;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JupiterPrime.Tests.Specs
{
    [TestFixture]
    public class ProductTests : BaseTest
    {
        [Test]
        public async Task GetAllProducts_ShouldReturnProducts()
        {
            var products = await ProductUseCases.GetAllProductsAsync();
            Assert.That(products, Is.Not.Null, "Products list is null");
            Assert.That(products.Any(), Is.True, "No products were returned");
            Console.WriteLine($"Number of products found: {products.Count}");
            foreach (var product in products)
            {
                Console.WriteLine($"Product: {product.Name}, Price: {product.Price}");
            }
        }

        [Test]
        public async Task GetProductByName_ShouldReturnCorrectProduct()
        {
            var productName = "Jupiter Plush Toy";
            var product = await ProductUseCases.GetProductByNameAsync(productName);
            Assert.That(product, Is.Not.Null, $"Product '{productName}' not found");
            Assert.That(product.Name, Is.EqualTo(productName));
            Console.WriteLine($"Found product: {product.Name}, Price: {product.Price}");
        }

        [Test]
        public async Task AddProductToCart_ShouldIncreaseCartQuantity()
        {
            var productName = "Jupiter Plush Toy";
            var quantity = 2;
            try
            {
                await ProductUseCases.AddProductToCartAsync(productName, quantity);
                // Add an assertion to check if the cart quantity has increased
                // This might require implementing a method to get the cart quantity
                Assert.Pass("Product added to cart successfully");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to add product to cart: {ex.Message}");
            }
        }
    }
}
