using System;

namespace SauceDemo.Infrastructure.Services;

public interface IShopService : IDisposable
{
    void AddProductToCart(string productName);
    bool IsProductInCart(string productName);
    int GetCartItemCount();
    void RemoveProductFromCart(string productName);
    void SortProducts(string sortOption);
    bool AreProductsSortedCorrectly(string sortOption);
    bool IsOnProductPage();
}

