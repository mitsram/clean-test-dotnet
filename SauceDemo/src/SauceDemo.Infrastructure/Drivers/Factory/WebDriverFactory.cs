using Microsoft.Playwright;
using OpenQA.Selenium;
namespace SauceDemo.Infrastructure.Drivers.Factory;

public static class WebDriverFactory
{
    public static async Task<(IWebDriverAdapter, IPlaywright?, IBrowser?, IPage?, IBrowserContext?)> CreateDriverAsync(DriverType driverType)
    {
        switch (driverType)
        {
            case DriverType.Selenium:                
                var (adapter, webDriver) = SeleniumDriverFactory.CreateDriver();
                return (adapter, null, null, null, null);

            case DriverType.Playwright:
                return await PlaywrightDriverFactory.CreateDriverAsync();

            default:
                throw new ArgumentException("Invalid driver type specified");
        }
    }
}