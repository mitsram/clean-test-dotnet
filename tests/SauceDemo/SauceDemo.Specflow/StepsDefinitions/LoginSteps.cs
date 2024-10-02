using TechTalk.SpecFlow;
using NUnit.Framework;
using SauceDemo.UseCases;
using SauceDemo.Domain.Entities;
using SauceDemo.Infrastructure.Services;

namespace SauceDemo.Tests.StepDefinitions
{
    [Binding]
    public class LoginStepDefinitions : BaseTest
    {
        private AuthenticationUseCases authentication;
        private bool loginResult;

        [BeforeScenario]
        public override async Task Setup()
        {
            await base.Setup();
            authentication = new AuthenticationUseCases(new AuthenticationService(driver!));
        }

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            authentication.GoToLoginPage();
        }

        [When(@"I attempt to login with username ""(.*)"" and password ""(.*)""")]
        public void WhenIAttemptToLoginWithUsernameAndPassword(string username, string password)
        {
            var user = new User { Username = username, Password = password };
            loginResult = authentication.AttemptLogin(user);
        }

        [Then(@"the login result should be ""(.*)""")]
        public void ThenTheLoginResultShouldBe(string expectedResult)
        {
            bool expected = expectedResult == "successful";
            Assert.That(loginResult, Is.EqualTo(expected));
        }

        [Then(@"I should be on the ""(.*)"" page")]
        public void ThenIShouldBeOnThePage(string expectedPage)
        {
            if (expectedPage == "inventory")
            {
                Assert.IsTrue(authentication.IsOnInventoryPage());
            }
            else
            {
                Assert.IsTrue(authentication.HasLoginError());
            }
        }
    }
}
