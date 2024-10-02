using SauceDemo.Domain.Entities;
using SauceDemo.Infrastructure.Drivers;
using System;

namespace SauceDemo.Infrastructure.Services;

public class CheckoutService : ICheckoutService, IDisposable
{
    private readonly IWebDriverStrategy _driverStrategy;
    private bool _disposed = false;

    public CheckoutService(IWebDriverStrategy driverStrategy)
    {
        _driverStrategy = driverStrategy ?? throw new ArgumentNullException(nameof(driverStrategy));
    }

    public void NavigateToCart()
    {
        _driverStrategy.FindElementByClassName("shopping_cart_link").Click();
    }

    public void ClickCheckout()
    {
        _driverStrategy.FindElementById("checkout").Click();
    }

    public void EnterCustomerInfo(CustomerInfo customerInfo)
    {
        _driverStrategy.FindElementById("first-name").SendKeys(customerInfo.FirstName);
        _driverStrategy.FindElementById("last-name").SendKeys(customerInfo.LastName);
        _driverStrategy.FindElementById("postal-code").SendKeys(customerInfo.ZipCode);
    }

    public void ContinueToOverview()
    {
        _driverStrategy.FindElementById("continue").Click();
    }

    public bool FinishPurchase()
    {
        _driverStrategy.FindElementById("finish").Click();
        return IsOnOrderCompletePage();
    }

    public bool IsOnOrderCompletePage()
    {
        return _driverStrategy.GetCurrentUrl().Contains("/checkout-complete.html");
    }

    public string GetConfirmationMessage()
    {
        return _driverStrategy.FindElementByClassName("complete-header").Text;
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
                _driverStrategy.Dispose();
            }

            _disposed = true;
        }
    }

    ~CheckoutService()
    {
        Dispose(false);
    }
}

