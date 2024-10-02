using SauceDemo.UseCases;
using SauceDemo.Domain.Entities;
using SauceDemo.Infrastructure.Services;

namespace SauceDemo.Tests;

public class ShopTests : BaseTest
{
    private ShopUseCases shop;
    private AuthenticationUseCases authentication;

    [SetUp]
    public override async Task Setup()
    {
        await base.Setup();
        shop = new ShopUseCases(new ShopService(driver!));
        authentication = new AuthenticationUseCases(new AuthenticationService(driver!));

        // Login before each test
        authentication.GoToLoginPage();
        authentication.AttemptLogin(new User { Username = "standard_user", Password = "secret_sauce" });
    }

    [Test]
    public void Should_AddProductToCart_WhenProductIsAvailable()
    {
        // Arrange
        var product = new Product { Name = "Sauce Labs Backpack" };

        // Act 
        shop.AddProductToCart(product.Name);

        // Assert
        Assert.IsTrue(shop.IsProductInCart(product.Name));
        Assert.That(shop.GetCartItemCount(), Is.EqualTo(1));
    }

    [Test]
    public void Should_RemoveProductFromCart_WhenProductIsInCart()
    {
        // Arrange
        var product = new Product { Name = "Sauce Labs Bike Light" };
        shop.AddProductToCart(product.Name);

        // Act
        shop.RemoveProductFromCart(product.Name);

        // Assert
        Assert.IsFalse(shop.IsProductInCart(product.Name));
        Assert.That(shop.GetCartItemCount(), Is.EqualTo(0));
    }

    [Test]
    public void Should_SortProducts_WhenSortOptionIsSelected()
    {
        // Arrange
        string sortOption = "Price (high to low)";

        // Act
        shop.SortProducts(sortOption);

        // Assert
        Assert.IsTrue(shop.AreProductsSortedCorrectly(sortOption));
    }
}

