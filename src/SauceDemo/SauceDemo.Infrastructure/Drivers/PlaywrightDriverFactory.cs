using Microsoft.Playwright;
using System.Threading.Tasks;

namespace SauceDemo.Infrastructure.Drivers;

public class PlaywrightDriverFactory
{
    public static async Task<(IPlaywright playwright, IBrowser browser, IPage page, IBrowserContext context, IWebDriverStrategy strategy)> CreateDriverAsync()
    {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            Args = new[] { "--start-maximized" }
        });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = null
        });
        await context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        var page = await context.NewPageAsync();
        var strategy = new PlaywrightWebDriver(page);
        return (playwright, browser, page, context, (IWebDriverStrategy)strategy);
    }
}
