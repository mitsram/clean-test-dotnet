using SauceDemo.UseCases;
using SauceDemo.Domain.Entities;
using SauceDemo.Infrastructure.Services;

namespace SauceDemo.Tests;

public class PurchaseTests : BaseTest
{
    private AuthenticationUseCases authentication;
    private ShopUseCases shop;
    private CheckoutUseCases checkout;

    [SetUp]
    public override async Task Setup()
    {
        await base.Setup(); 
        authentication = new AuthenticationUseCases(new AuthenticationService(driver!));
        shop = new ShopUseCases(new ShopService(driver!));
        checkout = new CheckoutUseCases(new CheckoutService(driver!));

        // Login before each test
        authentication.GoToLoginPage();
        authentication.AttemptLogin(new User { Username = "standard_user", Password = "secret_sauce" });
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
        shop.AddProductToCart(product1.Name);
        shop.AddProductToCart(product2.Name);
        checkout.GoToCart();
        checkout.ProceedToCheckout();
        checkout.FillCustomerInfo(customerInfo);
        checkout.ContinueToOverview();        
        bool purchaseResult = checkout.CompletePurchase();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(purchaseResult, Is.True);
            Assert.That(checkout.IsOnOrderCompletePage(), Is.True);
            Assert.That(checkout.GetConfirmationMessage(), Is.EqualTo("Thank you for your order!"));
        });
    }
}
