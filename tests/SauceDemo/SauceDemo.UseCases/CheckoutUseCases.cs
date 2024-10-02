using SauceDemo.Domain.Entities;
using SauceDemo.Infrastructure.Services;

namespace SauceDemo.UseCases;

public class CheckoutUseCases
{
    private readonly ICheckoutService _checkoutService;

    public CheckoutUseCases(ICheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    public void GoToCart()
    {
        _checkoutService.NavigateToCart();
    }

    public void ProceedToCheckout()
    {
        _checkoutService.ClickCheckout();
    }

    public void FillCustomerInfo(CustomerInfo customerInfo)
    {
        _checkoutService.EnterCustomerInfo(customerInfo);
    }

    public void ContinueToOverview()
    {
        _checkoutService.ContinueToOverview();
    }

    public bool CompletePurchase()
    {
        return _checkoutService.FinishPurchase();
    }

    public bool IsOnOrderCompletePage()
    {
        return _checkoutService.IsOnOrderCompletePage();
    }

    public string GetConfirmationMessage()
    {
        return _checkoutService.GetConfirmationMessage();
    }
}

