using System;

using SauceDemo.Domain.Entities;

namespace SauceDemo.Infrastructure.Services;

public interface ICheckoutService : IDisposable
{
    void NavigateToCart();
    void ClickCheckout();
    void EnterCustomerInfo(CustomerInfo customerInfo);
    void ContinueToOverview();
    bool FinishPurchase();
    bool IsOnOrderCompletePage();
    string GetConfirmationMessage();
}

