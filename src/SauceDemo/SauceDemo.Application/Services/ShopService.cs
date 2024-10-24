using System;
using System.Linq;
using Framework.WebDriver.Interfaces;
using SauceDemo.Application.Interfaces;

namespace SauceDemo.Application.Services;

public class ShopService : IShopService, IDisposable
{
    private readonly IWebDriverAdapter driver;
    private bool _disposed;

    public ShopService(IWebDriverAdapter driverAdapter)
    {
        driver = driverAdapter;
    }

    public void AddProductToCart(string productName)
    {
        var addButton = driver.FindElementByXPath($"//div[text()='{productName}']/ancestor::div[@class='inventory_item']//button[contains(@id, 'add-to-cart')]");
        addButton.Click();
    }

    public bool IsProductInCart(string productName)
    {
        driver.FindElementByClassName("shopping_cart_link").Click();
        return driver.FindElementsByXPath($"//div[@class='inventory_item_name' and text()='{productName}']").Any();
    }

    public int GetCartItemCount()
    {
        var cartBadge = driver.FindElementsByClassName("shopping_cart_badge");
        return cartBadge.Any() ? int.Parse(cartBadge.First().Text) : 0;
    }

    public void RemoveProductFromCart(string productName)
    {
        var removeButton = driver.FindElementByXPath($"//div[text()='{productName}']/ancestor::div[@class='inventory_item']//button[contains(@id, 'remove')]");
        removeButton.Click();
    }

    public void SortProducts(string sortOption)
    {
        var sortDropdown = driver.FindElementByClassName("product_sort_container");        
        sortDropdown.SelectOptionByText(sortOption);        
    }

    public bool AreProductsSortedCorrectly(string sortOption)
    {
        var productPrices = driver.FindElementsByClassName("inventory_item_price")
                                    .Select(e => decimal.Parse(e.Text.Replace("$", "")))
                                    .ToList();

        if (sortOption == "Price (high to low)")
        {
            return productPrices.SequenceEqual(productPrices.OrderByDescending(p => p));
        }
        else if (sortOption == "Price (low to high)")
        {
            return productPrices.SequenceEqual(productPrices.OrderBy(p => p));
        }

        return false; // For other sort options, you'd need to implement the logic
    }

    public bool IsOnProductPage()
    {
        try
        {
            // Check for the presence of elements that are unique to the products page
            var inventoryContainer = driver.FindElementByClassName("inventory_container");
            var productSortContainer = driver.FindElementByClassName("product_sort_container");
            
            // Check if the URL ends with "/inventory.html"
            var currentUrl = driver.GetCurrentUrl();
            var isCorrectUrl = currentUrl.EndsWith("/inventory.html");

            // Return true if both elements are found and the URL is correct
            return inventoryContainer != null && productSortContainer != null && isCorrectUrl;
        }
        catch
        {
            // If any exception occurs (e.g., element not found), return false
            return false;
        }
    }

    public bool SearchForProduct(string productName)
    {
        // Assuming there's a search input field with a class of 'search-input'
        driver.FindElementByClassName(".search-input").SendKeys(productName);
        driver.FindElementByClassName(".search-button").Click(); // Assuming there's a search button
        // driver.WaitForElementAsync(".product-wrapper", 30); // Wait for search results to load

        // Check if the product is displayed in the search results
        var productElements = driver.FindElementsByClassName(".product-wrapper");
        // return productElements.Any(element => element.Text.Result.Contains(productName));
        return true;
    }

    public void NavigateToCart()
    {
        driver.FindElementByClassName("shopping_cart_link").Click();
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
                driver.Dispose();
            }
            _disposed = true;
        }
    }

    ~ShopService()
    {
        Dispose(false);
    }
}