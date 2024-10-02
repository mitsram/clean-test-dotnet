using TechTalk.SpecFlow;
using SauceDemo.UseCases;
using SauceDemo.Infrastructure.Services;
using NUnit.Framework;
using SauceDemo.Tests;
using SauceDemo.Domain.Entities;

namespace SauceDemo.Specflow.StepDefinitions
{
    [Binding]
    public class ShopSteps : BaseTest
    {
        private ShopUseCases shop;
        private AuthenticationUseCases authentication;

        [BeforeScenario]
        public override async Task Setup()
        {
            await base.Setup();
            shop = new ShopUseCases(new ShopService(driver!));
            authentication = new AuthenticationUseCases(new AuthenticationService(driver!));
        }

        [Given(@"I am logged in as a standard user")]
        public void GivenIAmLoggedInAsAStandardUser()
        {
            authentication.GoToLoginPage();
            authentication.AttemptLogin(new User { Username = "standard_user", Password = "secret_sauce" });
        }

        [When(@"I add ""(.*)"" to my cart")]
        [Given(@"I have added ""(.*)"" to my cart")]
        public void WhenIAddToMyCart(string productName)
        {
            shop.AddProductToCart(productName);
        }

        [When(@"I remove ""(.*)"" from my cart")]
        public void WhenIRemoveFromMyCart(string productName)
        {
            shop.RemoveProductFromCart(productName);
        }

        [When(@"I sort products by ""(.*)""")]
        public void WhenISortProductsBy(string sortOption)
        {
            shop.SortProducts(sortOption);
        }

        [Then(@"""(.*)"" should be in my cart")]
        public void ThenShouldBeInMyCart(string productName)
        {
            Assert.IsTrue(shop.IsProductInCart(productName));
        }

        [Then(@"""(.*)"" should not be in my cart")]
        public void ThenShouldNotBeInMyCart(string productName)
        {
            Assert.IsFalse(shop.IsProductInCart(productName));
        }

        [Then(@"the cart item count should be (.*)")]
        public void ThenTheCartItemCountShouldBe(int expectedCount)
        {
            Assert.That(shop.GetCartItemCount(), Is.EqualTo(expectedCount));
        }

        [Then(@"the products should be sorted correctly by ""(.*)""")]
        public void ThenTheProductsShouldBeSortedCorrectlyBy(string sortOption)
        {
            Assert.IsTrue(shop.AreProductsSortedCorrectly(sortOption));
        }
    }
}
