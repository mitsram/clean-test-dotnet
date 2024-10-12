using JupiterPrime.Application.Interfaces;
using JupiterPrime.Domain.Entities;
using JupiterPrime.Infrastructure.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JupiterPrime.Infrastructure.Services
{
    public class ProductService : IProductService, IDisposable
    {
        private readonly IWebDriverAdapter _driver;
        private readonly string _baseUrl;
        private bool _disposed = false;

        public ProductService(IWebDriverFactory webDriverFactory, string baseUrl)
        {
            _driver = webDriverFactory.CreateWebDriver();
            _baseUrl = baseUrl;
        }

        private async Task NavigateToProductsPage()
        {
            await _driver.NavigateToAsync(_baseUrl);
            await _driver.WaitForElementAsync(".products-wrapper", 30);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            await NavigateToProductsPage();

            var productElements = await _driver.FindElementsAsync(".product-wrapper");

            if (!productElements.Any())
            {
                Console.WriteLine("No product elements found on the page.");
                return new List<Product>();
            }

            var products = new List<Product>();
            foreach (var element in productElements)
            {
                try
                {
                    var name = await _driver.GetTextAsync(element, ".product-title");
                    var priceText = await _driver.GetTextAsync(element, ".product-price");
                    var price = decimal.Parse(priceText.TrimStart('$'));
                    products.Add(new Product { Name = name, Price = price, Quantity = 0 });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing product: {ex.Message}");
                }
            }

            return products;
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
            var products = await GetAllProductsAsync();
            return products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task AddProductToCartAsync(string productName, int quantity)
        {
            await NavigateToProductsPage();

            var productSelector = $"//div[contains(@class, 'product-wrapper')]//h3[contains(@class, 'product-title') and contains(text(),'{productName}')]/ancestor::div[contains(@class, 'product-wrapper')]";
            var addToCartButtonSelector = ".//button[contains(@class, 'btn-add-to-cart')]";

            for (int i = 0; i < quantity; i++)
            {
                try
                {
                    var productElement = await _driver.FindElementAsync(productSelector);
                    var addToCartButton = await _driver.FindElementAsync(productElement, addToCartButtonSelector);
                    await _driver.ClickElementAsync(addToCartButton);
                    await Task.Delay(1000); // Wait for cart update
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding product to cart: {ex.Message}");
                    throw;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _driver?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
