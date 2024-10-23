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
    [TestCase("standard_user", "secret_sauce", false)]
    public void Should_LoginSuccessfully_WhenCredentialsAreValid(string username, string password, bool expectedResult)
    {
        // Arrange
        var user = new User { Username = username, Password = password };

        // Act 
        authentication.GoToLoginPage();
        bool loginResult = authentication.AttemptLogin(user);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(loginResult, Is.True, "Login attempt for standard_user failed unexpectedly.");
            Assert.That(authentication.IsOnInventoryPage(), Is.True, "User was not redirected to the inventory page after successful login.");            
        });
    }

    [Test]
    [TestCase("locked_out_user", "secret_sauce")]
    [TestCase("invalid_user", "invalid_password")]
    public void Should_HaveLoginError_WhenCredentialsAreInvalid(string username, string password)
    {
        // Arrange
        var user = new User { Username = username, Password = password };

        // Act 
        authentication.GoToLoginPage();
        bool loginResult = authentication.AttemptLogin(user);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(loginResult, Is.False, $"Login attempt for '{username}' succeeded unexpectedly.");            
            Assert.That(authentication.HasLoginError(), Is.True, "Login error message is not displayed for invalid credentials.");            
            Assert.That(authentication.IsOnInventoryPage(), Is.False, "User was incorrectly redirected to the inventory page after failed login.");
        });
    }
}
