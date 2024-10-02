using SauceDemo.UseCases;
using SauceDemo.Domain.Entities;
using SauceDemo.Infrastructure.Services;
using NUnit.Framework;

namespace SauceDemo.Tests;

public class ShopTests : BaseTest
{
    private ShopUseCases _shopUseCases;
    private LoginUseCases _loginUseCases;

    [SetUp]
    public override async Task Setup()
    {
        await base.Setup();
        var shopService = new ShopService(Driver); // Implement this in your infrastructure layer
        var loginService = new LoginService(Driver); // Reuse the LoginService we created earlier
        _shopUseCases = new ShopUseCases(shopService);
        _loginUseCases = new LoginUseCases(loginService);

        // Login before each test
        _loginUseCases.GoToLoginPage();
        _loginUseCases.AttemptLogin(new User { Username = "standard_user", Password = "secret_sauce" });
    }

    [Test]
    public void Should_AddProductToCart_WhenProductIsAvailable()
    {
        // Arrange
        var product = new Product { Name = "Sauce Labs Backpack" };

        // Act 
        _shopUseCases.AddProductToCart(product.Name);

        // Assert
        Assert.IsTrue(_shopUseCases.IsProductInCart(product.Name));
        Assert.AreEqual(1, _shopUseCases.GetCartItemCount());
    }

    [Test]
    public void Should_RemoveProductFromCart_WhenProductIsInCart()
    {
        // Arrange
        var product = new Product { Name = "Sauce Labs Bike Light" };
        _shopUseCases.AddProductToCart(product.Name);

        // Act
        _shopUseCases.RemoveProductFromCart(product.Name);

        // Assert
        Assert.IsFalse(_shopUseCases.IsProductInCart(product.Name));
        Assert.AreEqual(0, _shopUseCases.GetCartItemCount());
    }

    [Test]
    public void Should_SortProducts_WhenSortOptionIsSelected()
    {
        // Arrange
        string sortOption = "Price (high to low)";

        // Act
        _shopUseCases.SortProducts(sortOption);

        // Assert
        Assert.IsTrue(_shopUseCases.AreProductsSortedCorrectly(sortOption));
    }
}

