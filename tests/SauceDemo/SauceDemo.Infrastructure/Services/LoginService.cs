using SauceDemo.Infrastructure.Drivers;
using System;

namespace SauceDemo.Infrastructure.Services;

public class LoginService : ILoginService, IDisposable
{
    private readonly IWebDriverStrategy _driverStrategy;
    private bool _disposed = false;

    public LoginService(IWebDriverStrategy driverStrategy)
    {
        _driverStrategy = driverStrategy;
    }

    public void NavigateToLoginPage()
    {
        _driverStrategy.NavigateToUrl("https://www.saucedemo.com/");
        
        // Optionally, we can add a check to ensure the page has loaded
        var usernameField = _driverStrategy.FindElementById("user-name");
        if (usernameField == null)
        {
            throw new Exception("Login page did not load properly");
        }
    }

    public bool Login(string username, string password)
    {
        var usernameField = _driverStrategy.FindElementById("user-name");
        var passwordField = _driverStrategy.FindElementById("password");
        var loginButton = _driverStrategy.FindElementById("login-button");

        usernameField.SendKeys(username);
        passwordField.SendKeys(password);
        loginButton.Click();

        return IsOnInventoryPage();
    }

    public bool IsOnInventoryPage()
    {
        return _driverStrategy.GetCurrentUrl().EndsWith("/inventory.html");
    }

    public bool HasLoginError()
    {
        var errorElement = _driverStrategy.FindElementByClassName("error-message-container");
        return !string.IsNullOrEmpty(errorElement.Text);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Implement disposal logic using _driverStrategy
            }
            _disposed = true;
        }
    }

    ~LoginService()
    {
        Dispose(false);
    }
}

