using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using SauceDemo.Infrastructure.Drivers; // Add this line

namespace SauceDemo.Infrastructure.Services;

public class ShopService : IShopService, IDisposable
{
    private readonly IWebDriverStrategy driver;
    private bool _disposed;

    public ShopService(IWebDriverStrategy driverStrategy)
    {
        driver = driverStrategy;
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
        sortDropdown.Click();
        var option = driver.FindElementByXPath($"//option[text()='{sortOption}']");
        option.Click();
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

