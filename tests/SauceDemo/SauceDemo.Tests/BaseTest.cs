using NUnit.Framework;
using SauceDemo.Infrastructure.Services;
using SauceDemo.UseCases;
using SauceDemo.Infrastructure.Drivers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace SauceDemo.Tests;

public class PlaywrightReportAttribute : Attribute
{
    [OneTimeSetUp]
    public static void GeneratePlaywrightReport()
    {
        Microsoft.Playwright.Program.Main(new[] { "show-report", "playwright-report" });

        Process.Start(new ProcessStartInfo("playwright-report/index.html") { UseShellExecute = true });
    }
}

[TestFixture]
public class BaseTest
{
    [OneTimeSetUp]
    public static void GeneratePlaywrightReport()
    {
        PlaywrightReportAttribute.GeneratePlaywrightReport();
    }

    protected IWebDriverStrategy Driver;
    protected ILoginService LoginService;
    protected IShopService ShopService;
    protected ICheckoutService CheckoutService;

    protected LoginUseCases LoginUseCases;
    protected ShopUseCases ShopUseCases;
    protected CheckoutUseCases CheckoutUseCases;

    // Add an enum to select the driver type
    protected enum DriverType
    {
        Selenium,
        Playwright
    }

    // Set the desired driver type here
    protected DriverType CurrentDriverType = DriverType.Playwright;
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IPage _page;
    private IBrowserContext _context;

    [SetUp]
    public virtual async Task Setup()
    {
        string projectDirectory = TestContext.CurrentContext.TestDirectory;
        Directory.SetCurrentDirectory(projectDirectory);

        // Driver = await InitializeDriverStrategy();
        await InitializeDriverStrategy();
    }

    // private async Task<IWebDriverStrategy> InitializeDriverStrategy()
    private async Task InitializeDriverStrategy()
    {
        switch (CurrentDriverType)
        {
            case DriverType.Selenium:
                IWebDriver driver = new ChromeDriver();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.Manage().Window.Maximize();
                // return new SeleniumWebDriverStrategy(driver);
                Driver = new SeleniumWebDriverStrategy(driver);
                break;

            case DriverType.Playwright:
                // var playwright = await Playwright.CreateAsync();
                // var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
                // var page = await browser.NewPageAsync();
                // return new PlaywrightWebDriverStrategy(page);
                (_playwright, _browser, _page, _context, Driver) = await PlaywrightDriverFactory.CreateDriverAsync();
                break;

            default:
                throw new ArgumentException("Invalid driver type specified");
        }
    }

    [TearDown]
    public virtual async Task TearDown()
    {
        try
        {
            if (CurrentDriverType == DriverType.Playwright)
            {
                string testName = TestContext.CurrentContext.Test.Name;
                await _context.Tracing.StopAsync(new()
                {
                    Path = $"trace-{testName}.zip"
                });

                await _page.CloseAsync();
                await _browser.CloseAsync();
                _playwright?.Dispose();                
            }
            else
            {
                Driver?.Dispose();
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

    protected void LoginAsStandardUser()
    {
        LoginUseCases.GoToLoginPage();
        LoginUseCases.AttemptLogin(new Domain.Entities.User { Username = "standard_user", Password = "secret_sauce" });
    }
}

