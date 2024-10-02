using SauceDemo.UseCases;
using SauceDemo.Domain.Entities;
using SauceDemo.Infrastructure.Services;

namespace SauceDemo.Tests;

public class LoginTests : BaseTest
{
    private AuthenticationUseCases authentication;

    [SetUp]
    public override async Task Setup()
    {
        await base.Setup();        
        authentication = new AuthenticationUseCases(new AuthenticationService(driver!));        
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
