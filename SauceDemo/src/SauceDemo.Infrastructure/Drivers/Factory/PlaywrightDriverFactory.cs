using Microsoft.Playwright;
using System.Threading.Tasks;
using SauceDemo.Infrastructure.Drivers;

namespace SauceDemo.Infrastructure.Drivers.Factory;

public class PlaywrightDriverFactory
{
    public static async Task<(IWebDriverAdapter adapter, IPlaywright playwright, IBrowser browser, IPage page, IBrowserContext context)> CreateDriverAsync()
    {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = ViewportSize.NoViewport
        });
        await context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        var page = await context.NewPageAsync();
        await page.SetViewportSizeAsync(0, 0); // This maximizes the viewport
        var adapter = new PlaywrightAdapter(page);
        return (adapter, playwright, browser, page, context);
    }
}
