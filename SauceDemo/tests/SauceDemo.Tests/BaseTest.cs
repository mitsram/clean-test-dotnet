using SauceDemo.Infrastructure.Services;
using SauceDemo.UseCases;
using SauceDemo.Infrastructure.Drivers;
using SauceDemo.Infrastructure.Drivers.Factory;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Playwright;
using System.Diagnostics;
using TechTalk.SpecFlow;

namespace SauceDemo.Tests;


[Binding]
[TestFixture]
public class BaseTest
{

    protected IWebDriverAdapter? driver;
    protected IAuthenticationService? LoginService;
    protected IShopService? ShopService;
    protected ICheckoutService? CheckoutService;
    protected AuthenticationUseCases? AuthenticationUseCases;   

    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IPage? _page;
    private IBrowserContext? _context;


    /*
     * Set the desired driver type here
     */
    protected DriverType CurrentDriverType = DriverType.Playwright;

    [SetUp]
    [BeforeScenario]
    public virtual async Task Setup()
    {
        string projectDirectory = TestContext.CurrentContext.TestDirectory;
        Directory.SetCurrentDirectory(projectDirectory);

        await InitializeDriver();
    }   

    protected async Task InitializeDriver()
    {
        (driver, _playwright, _browser, _page, _context) = await WebDriverFactory.CreateDriverAsync(CurrentDriverType);
    }
    

    [TearDown]
    [AfterScenario]
    public virtual async Task TearDown()
    {
        try
        {
            if (CurrentDriverType == DriverType.Playwright)
            {
                string testName = TestContext.CurrentContext.Test.Name;
                if (_context?.Tracing != null)
                {
                    await _context.Tracing.StopAsync(new()
                    {
                        Path = $"trace-{testName}.zip"
                    });
                }

                await _page!.CloseAsync();
                await _browser!.CloseAsync();
                _playwright?.Dispose();                
            }
            else
            {
                driver?.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during driver disposal: {ex.Message}");
        }

        LoginService?.Dispose();
        ShopService?.Dispose();
        CheckoutService?.Dispose();
    }

    // protected void LoginAsStandardUser()
    // {
    //     LoginUseCases.GoToLoginPage();
    //     LoginUseCases.AttemptLogin(new Domain.Entities.User { Username = "standard_user", Password = "secret_sauce" });
    // }
}

