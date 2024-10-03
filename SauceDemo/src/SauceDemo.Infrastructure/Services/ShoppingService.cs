using SauceDemo.Domain.Entities;
using SauceDemo.Infrastructure.Drivers;


namespace SauceDemo.Infrastructure.Services;

public class ShoppingService : IAuthenticationService, IShopService, ICheckoutService, IDisposable
{
    private readonly IWebDriverAdapter driver;
    private readonly AuthenticationService _loginService;
    private readonly ShopService _shopService;
    private readonly CheckoutService _checkoutService;
    private bool _disposed = false;

    public ShoppingService(IWebDriverAdapter driverAdapter)
    {
        driver = driverAdapter ?? throw new ArgumentNullException(nameof(driverAdapter));
        _loginService = new AuthenticationService(driverAdapter);
        _shopService = new ShopService(driverAdapter);
        _checkoutService = new CheckoutService(driverAdapter);
    }

    // ILoginService methods
    public void NavigateToLoginPage() => _loginService.NavigateToLoginPage();
    public bool Login(string username, string password) => _loginService.Login(username, password);
    public bool IsOnInventoryPage() => _loginService.IsOnInventoryPage();
    public bool HasLoginError() => _loginService.HasLoginError();

    // IShopService methods
    public void AddProductToCart(string productName) => _shopService.AddProductToCart(productName);
    public bool IsProductInCart(string productName) => _shopService.IsProductInCart(productName);
    public int GetCartItemCount() => _shopService.GetCartItemCount();
    public void RemoveProductFromCart(string productName) => _shopService.RemoveProductFromCart(productName);
    public void SortProducts(string sortOption) => _shopService.SortProducts(sortOption);
    public bool AreProductsSortedCorrectly(string sortOption) => _shopService.AreProductsSortedCorrectly(sortOption);
    public bool IsOnProductPage() => _shopService.IsOnProductPage();

    // ICheckoutService methods
    public void NavigateToCart() => _checkoutService.NavigateToCart();
    public void ClickCheckout() => _checkoutService.ClickCheckout();
    public void EnterCustomerInfo(CustomerInfo customerInfo) => _checkoutService.EnterCustomerInfo(customerInfo);
    public void ContinueToOverview() => _checkoutService.ContinueToOverview();
    public bool FinishPurchase() => _checkoutService.FinishPurchase();
    public bool IsOnOrderCompletePage() => _checkoutService.IsOnOrderCompletePage();
    public string GetConfirmationMessage() => _checkoutService.GetConfirmationMessage();

    public bool IsProductAvailable(string productName, int quantity = 1)
    {
        // Navigate to the inventory page if not already there
        if (!driver.GetCurrentUrl().EndsWith("/inventory.html"))
        {
            driver.NavigateToUrl("https://www.saucedemo.com/inventory.html");
        }

        // Check if the product exists
        var productElement = driver.FindElementsByXPath($"//div[@class='inventory_item_name' and text()='{productName}']");
        if (!productElement.Any())
        {
            Console.WriteLine($"Product {productName} not found.");
            return false;
        }

        // Check if the product can be added to cart
        var addToCartButton = driver.FindElementsByXPath($"//div[@class='inventory_item_name' and text()='{productName}']/ancestor::div[@class='inventory_item']//button[contains(@class, 'btn_inventory')]");
        if (!addToCartButton.Any() || addToCartButton.First().Text.ToLower().Contains("remove"))
        {
            Console.WriteLine($"Product {productName} is out of stock or already in cart.");
            return false;
        }

        // Sauce Demo doesn't show actual stock quantities, so we'll assume it's available if we can add it to cart
        Console.WriteLine($"{quantity} {productName}(s) is available.");
        return true;
    }

    public decimal GetCartTotal()
    {
        // Navigate to the cart page if not already there
        NavigateToCart();

        var totalElement = driver.FindElementByClassName("summary_total_label");
        string totalText = totalElement.Text.Replace("Total: $", "");
        return decimal.Parse(totalText, System.Globalization.CultureInfo.InvariantCulture);
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
                _loginService.Dispose();
                _shopService.Dispose();
                _checkoutService.Dispose();
                driver.Dispose();
            }
            _disposed = true;
        }
    }

    ~ShoppingService()
    {
        Dispose(false);
    }
}
