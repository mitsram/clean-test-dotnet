using System;

namespace SauceDemo.Infrastructure.Services;

public interface IAuthenticationService : IDisposable
{
    void NavigateToLoginPage();
    bool Login(string username, string password);
    bool IsOnInventoryPage();
    bool HasLoginError();
}

