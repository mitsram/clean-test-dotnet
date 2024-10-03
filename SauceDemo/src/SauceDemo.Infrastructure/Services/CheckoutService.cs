using SauceDemo.Domain.Entities;
using SauceDemo.Infrastructure.Drivers;
using System;

namespace SauceDemo.Infrastructure.Services;

public class CheckoutService : ICheckoutService, IDisposable
{
    private readonly IWebDriverAdapter driver;
    private bool _disposed = false;

    public CheckoutService(IWebDriverAdapter driverAdapter)
    {
        driver = driverAdapter ?? throw new ArgumentNullException(nameof(driverAdapter));
    }

    public void NavigateToCart()
    {
        driver.FindElementByClassName("shopping_cart_link").Click();
    }

    public void ClickCheckout()
    {
        driver.FindElementById("checkout").Click();
    }

    public void EnterCustomerInfo(CustomerInfo customerInfo)
    {
        driver.FindElementById("first-name").SendKeys(customerInfo.FirstName);
        driver.FindElementById("last-name").SendKeys(customerInfo.LastName);
        driver.FindElementById("postal-code").SendKeys(customerInfo.ZipCode);
    }

    public void ContinueToOverview()
    {
        driver.FindElementById("continue").Click();
    }

    public bool FinishPurchase()
    {
        driver.FindElementById("finish").Click();
        return IsOnOrderCompletePage();
    }

    public bool IsOnOrderCompletePage()
    {
        return driver.GetCurrentUrl().Contains("/checkout-complete.html");
    }

    public string GetConfirmationMessage()
    {
        return driver.FindElementByClassName("complete-header").Text;
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
                // Dispose managed resources
                driver.Dispose();
            }

            _disposed = true;
        }
    }

    ~CheckoutService()
    {
        Dispose(false);
    }
}

