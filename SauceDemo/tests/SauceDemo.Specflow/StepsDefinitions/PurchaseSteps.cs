using TechTalk.SpecFlow;
using NUnit.Framework;
using SauceDemo.UseCases;
using SauceDemo.Domain.Entities;
using SauceDemo.Infrastructure.Services;
using TechTalk.SpecFlow.Assist;

namespace SauceDemo.Tests.StepDefinitions
{
    [Binding]
    public class PurchaseSteps : BaseTest
    {
        private AuthenticationUseCases authentication;
        private ShopUseCases shop;
        private CheckoutUseCases checkout;
        private bool purchaseResult;

        [BeforeScenario]
        public override async Task Setup()
        {
            await base.Setup();
            authentication = new AuthenticationUseCases(new AuthenticationService(driver!));
            shop = new ShopUseCases(new ShopService(driver!));
            checkout = new CheckoutUseCases(new CheckoutService(driver!));
        }

        [Given(@"I am logged in as a standard user")]
        public void GivenIAmLoggedInAsAStandardUser()
        {
            authentication.GoToLoginPage();
            authentication.AttemptLogin(new User { Username = "standard_user", Password = "secret_sauce" });
        }

        [Given(@"I have added ""(.*)"" to my cart")]
        public void GivenIHaveAddedToMyCart(string productName)
        {
            shop.AddProductToCart(productName);
        }

        [When(@"I proceed to checkout")]
        public void WhenIProceedToCheckout()
        {
            checkout.GoToCart();
            checkout.ProceedToCheckout();
        }

        [When(@"I fill in the following customer information:")]
        public void WhenIFillInTheFollowingCustomerInformation(Table table)
        {
            var customerInfo = table.CreateInstance<CustomerInfo>();
            checkout.FillCustomerInfo(customerInfo);
        }

        [When(@"I continue to the overview page")]
        public void WhenIContinueToTheOverviewPage()
        {
            checkout.ContinueToOverview();
        }

        [When(@"I complete the purchase")]
        public void WhenICompleteThePurchase()
        {
            purchaseResult = checkout.CompletePurchase();
        }

        [Then(@"the purchase should be successful")]
        public void ThenThePurchaseShouldBeSuccessful()
        {
            Assert.That(purchaseResult, Is.True);
        }

        [Then(@"I should be on the order complete page")]
        public void ThenIShouldBeOnTheOrderCompletePage()
        {
            Assert.That(checkout.IsOnOrderCompletePage(), Is.True);
        }

        [Then(@"I should see the confirmation message ""(.*)""")]
        public void ThenIShouldSeeTheConfirmationMessage(string expectedMessage)
        {
            Assert.That(checkout.GetConfirmationMessage(), Is.EqualTo(expectedMessage));
        }
    }
}
