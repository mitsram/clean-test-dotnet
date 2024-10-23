using Framework.WebDriver;
using SauceDemo.Tests.Base;
using SauceDemo.Application.UseCases;
using SauceDemo.Application.Services;
using SauceDemo.Domain.Entities;

namespace SauceDemo.Tests.Specs;

public class LoginTests : BaseTest
{
    private AuthenticationUseCases authentication;

    [SetUp]
    public void SetUp()
    {
        authentication = new AuthenticationUseCases(new AuthenticationService(driver));
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
        authentication.GoToLoginPage();
        bool loginResult = authentication.AttemptLogin(user);

        // Assert
        Assert.That(loginResult, Is.EqualTo(expectedResult));
        if (expectedResult)
        {
            Assert.IsTrue(authentication.IsOnInventoryPage());
        }
        else
        {
            Assert.IsTrue(authentication.HasLoginError());
        }        
    }
}
