using SauceDemo.Infrastructure.Services;
using SauceDemo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SauceDemo.Application.UseCases;

public class ShoppingUseCases
{
    private readonly ShoppingService _shoppingService;

    public ShoppingUseCases(ShoppingService shoppingService)
    {
        _shoppingService = shoppingService ?? throw new ArgumentNullException(nameof(shoppingService));
    }

    public void AddProductToCart(string productName, int quantity = 1)
    {
        if (_shoppingService.IsProductAvailable(productName, quantity))
        {
            for (int i = 0; i < quantity; i++)
            {
                _shoppingService.AddProductToCart(productName);
            }
            Console.WriteLine($"Added {quantity} {productName}(s) to the cart.");
        }
        else
        {
            Console.WriteLine($"Sorry, {productName} is not available in the requested quantity.");
        }
    }

    public void RemoveProductFromCart(string productName)
    {
        _shoppingService.RemoveProductFromCart(productName);
        Console.WriteLine($"Removed {productName} from the cart.");
    }

    public int GetCurrentCartCount()
    {
        return _shoppingService.GetCartItemCount();
    }

    public decimal GetCartTotal()
    {
        return _shoppingService.GetCartTotal();
    }

    public void DisplayCartSummary()
    {
        int itemCount = GetCurrentCartCount();
        decimal total = GetCartTotal();

        Console.WriteLine("Cart Summary:");
        Console.WriteLine($"Total Items: {itemCount}");
        Console.WriteLine($"Total Price: ${total:F2}");
    }

    public bool ProcessPurchase(CustomerInfo customerInfo)
    {
        if (GetCurrentCartCount() == 0)
        {
            Console.WriteLine("Cannot process purchase. The cart is empty.");
            return false;
        }

        // Here you would typically call methods to process the purchase,
        // such as validating the customer info, processing payment, etc.
        // For this example, we'll just simulate a successful purchase.

        Console.WriteLine("Processing purchase...");
        Console.WriteLine($"Customer: {customerInfo.FirstName} {customerInfo.LastName}");
        Console.WriteLine($"Shipping to: {customerInfo.Address}");
        DisplayCartSummary();

        Console.WriteLine("Purchase completed successfully!");
        return true;
    }
}
