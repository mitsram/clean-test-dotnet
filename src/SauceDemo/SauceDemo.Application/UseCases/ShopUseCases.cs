using SauceDemo.Application.Interfaces;
using System.Threading.Tasks;

namespace SauceDemo.Application.UseCases;

public class ShopUseCases
{
    private readonly IShopService _shopService;

    public ShopUseCases(IShopService shopService)
    {
        _shopService = shopService;
    }

    public void AddProductToCart(string productName)
    {
        _shopService.AddProductToCart(productName);
    }

    public bool IsProductInCart(string productName)
    {
        return _shopService.IsProductInCart(productName);
    }

    public int GetCartItemCount()
    {
        return _shopService.GetCartItemCount();
    }

    public void RemoveProductFromCart(string productName)
    {
        _shopService.RemoveProductFromCart(productName);
    }

    public void SortProducts(string sortOption)
    {
        _shopService.SortProducts(sortOption);
    }

    public bool AreProductsSortedCorrectly(string sortOption)
    {
        return _shopService.AreProductsSortedCorrectly(sortOption);
    }

    public bool IsOnProductPage()
    {
        return _shopService.IsOnProductPage();
    }

    public bool SearchForProduct(string productName)
    {
        return _shopService.SearchForProduct(productName);
    }

    public void GoToCart()
    {
        _shopService.NavigateToCart();
    }
}
