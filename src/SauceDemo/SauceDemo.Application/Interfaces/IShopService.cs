using System;

namespace SauceDemo.Application.Interfaces;

public interface IShopService : IDisposable
{
    void AddProductToCart(string productName);
    bool IsProductInCart(string productName);
    int GetCartItemCount();
    void RemoveProductFromCart(string productName);
    void SortProducts(string sortOption);
    bool AreProductsSortedCorrectly(string sortOption);
    bool IsOnProductPage();
    void NavigateToCart();
    public bool SearchForProduct(string productName);
}

