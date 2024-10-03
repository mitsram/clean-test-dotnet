using TechTalk.SpecFlow;
using NUnit.Framework;
using SauceDemo.UseCases;
using SauceDemo.Domain.Entities;
using SauceDemo.Infrastructure.Services;

namespace SauceDemo.Tests.StepDefinitions
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

        [Given(@"I am on the products page")]
        public void GivenIAmOnTheProductsPage()
        {
            Assert.IsTrue(shop.IsOnProductPage());
        }

        [When(@"I add the ""(.*)"" to the cart")]
        public void WhenIAddTheProductToTheCart(string productName)
        {
            shop.AddProductToCart(productName);
        }

        [Then(@"the ""(.*)"" should be in the cart")]
        public void ThenTheProductShouldBeInTheCart(string productName)
        {
            Assert.IsTrue(shop.IsProductInCart(productName));
        }

        [Then(@"the cart item count should be (.*)")]
        public void ThenTheCartItemCountShouldBe(int expectedCount)
        {
            Assert.That(shop.GetCartItemCount(), Is.EqualTo(expectedCount));
        }

        [Given(@"I have added the ""(.*)"" to the cart")]
        public void GivenIHaveAddedTheProductToTheCart(string productName)
        {
            shop.AddProductToCart(productName);
        }

        [When(@"I remove the ""(.*)"" from the cart")]
        public void WhenIRemoveTheProductFromTheCart(string productName)
        {
            shop.RemoveProductFromCart(productName);
        }

        [Then(@"the ""(.*)"" should not be in the cart")]
        public void ThenTheProductShouldNotBeInTheCart(string productName)
        {
            Assert.IsFalse(shop.IsProductInCart(productName));
        }

        [When(@"I sort the products by ""(.*)""")]
        public void WhenISortTheProductsBy(string sortOption)
        {
            shop.SortProducts(sortOption);
        }

        [Then(@"the products should be sorted correctly by ""(.*)""")]
        public void ThenTheProductsShouldBeSortedCorrectlyBy(string sortOption)
        {
            Assert.IsTrue(shop.AreProductsSortedCorrectly(sortOption));
        }
    }
}
