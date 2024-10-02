using SauceDemo.UseCases;
using SauceDemo.Domain.Entities;
using SauceDemo.Infrastructure.Services;
using NUnit.Framework;

namespace SauceDemo.Tests
{
    public class LoginTests : BaseTest
    {
        private LoginUseCases _loginUseCases;

        [SetUp]
        public override async Task Setup()
        {
            await base.Setup();
            // var loginService = new LoginService(); // Implement this in your infrastructure layer
            var loginService = new LoginService(Driver);
            _loginUseCases = new LoginUseCases(loginService);
            _loginUseCases.GoToLoginPage();
        }

        [Test]
        [TestCase("standard_user", "secret_sauce", true)]
        [TestCase("locked_out_user", "secret_sauce", false)]
        [TestCase("invalid_user", "invalid_password", false)]
        public void Should_LoginSuccessfully_WhenCredentialsAreValid(string username, string password, bool expectedResult)
        {
            // Arrange
            var user = new User { Username = username, Password = password };

            // Act 
            bool loginResult = _loginUseCases.AttemptLogin(user);

            // Assert
            Assert.AreEqual(expectedResult, loginResult);
            if (expectedResult)
            {
                Assert.IsTrue(_loginUseCases.IsOnInventoryPage());
            }
            else
            {
                Assert.IsTrue(_loginUseCases.HasLoginError());
            }
        }
    }
}