using SauceDemo.Domain.Entities;
using SauceDemo.Application.Interfaces;

namespace SauceDemo.Application.UseCases;

public class AuthenticationUseCases
{
    private readonly IAuthenticationService _loginService;

    public AuthenticationUseCases(IAuthenticationService loginService)
    {
        _loginService = loginService;
    }

    public void GoToLoginPage()
    {
        _loginService.NavigateToLoginPage();
    }

    public bool AttemptLogin(User user)
    {
        return _loginService.Login(user.Username, user.Password);
    }

    public bool IsOnInventoryPage()
    {
        return _loginService.IsOnInventoryPage();
    }

    public bool HasLoginError()
    {
        return _loginService.HasLoginError();
    }
}

