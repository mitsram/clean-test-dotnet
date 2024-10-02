using SauceDemo.UseCases;
using SauceDemo.Domain.Entities;
using SauceDemo.Infrastructure.Services;
using NUnit.Framework;
using System.Threading.Tasks;

namespace SauceDemo.Tests;

public class PurchaseTests : BaseTest
{
    private LoginUseCases _loginUseCases;
    private ShopUseCases _shopUseCases;
    private CheckoutUseCases _checkoutUseCases;

    [SetUp]
    public override async Task Setup()
    {
        await base.Setup();
        var loginService = new LoginService(Driver);
        var shopService = new ShopService(Driver);
        var checkoutService = new CheckoutService(Driver);

        _loginUseCases = new LoginUseCases(loginService);
        _shopUseCases = new ShopUseCases(shopService);
        _checkoutUseCases = new CheckoutUseCases(checkoutService);



        // Login before each test
        _loginUseCases.GoToLoginPage();
        _loginUseCases.AttemptLogin(new User { Username = "standard_user", Password = "secret_sauce" });
    }

    [Test]
    public void Should_CompletePurchase_WhenValidDetailsProvided()
    {
        // Arrange
        var product1 = new Product { Name = "Sauce Labs Backpack" };
        var product2 = new Product { Name = "Sauce Labs Bike Light" };
        var customerInfo = new CustomerInfo
        {
            FirstName = "John",
            LastName = "Doe",
            ZipCode = "12345"
        };

        // Act
        _shopUseCases.AddProductToCart(product1.Name);
        _shopUseCases.AddProductToCart(product2.Name);
        
        _checkoutUseCases.GoToCart();
        _checkoutUseCases.ProceedToCheckout();
        _checkoutUseCases.FillCustomerInfo(customerInfo);
        _checkoutUseCases.ContinueToOverview();
        
        bool purchaseResult = _checkoutUseCases.CompletePurchase();

        // Assert
        Assert.IsTrue(purchaseResult);
        Assert.IsTrue(_checkoutUseCases.IsOnOrderCompletePage());
        Assert.AreEqual("Thank you for your order!", _checkoutUseCases.GetConfirmationMessage());
    }
}
