using System;

namespace SauceDemo.Application.Interfaces;

public interface IAuthenticationService : IDisposable
{
    void NavigateToLoginPage();
    bool Login(string username, string password);
    bool IsOnInventoryPage();
    bool HasLoginError();
}

