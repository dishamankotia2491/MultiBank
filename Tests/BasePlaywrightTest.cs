using Microsoft.Playwright;
using MultiBank.QA.Automation.Helpers;
using NUnit.Framework;

namespace MultiBank.QA.Automation.Tests;

/// <summary>
/// Base test class that launches a dedicated Playwright Chromium browser with explicit options.
/// Tests should inherit from this class to use a headful Chrome when configured.
/// </summary>
public class BasePlaywrightTest
{
    protected IPlaywright Playwright = null!;
    protected IBrowser Browser = null!;
    protected IBrowserContext Context = null!;
    protected IPage Page = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetupAsync()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

        // Launch an explicit headful Chrome instance so tests run in a visible browser window
        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = false,
            Channel = "chrome"
        };

        Browser = await Playwright.Chromium.LaunchAsync(launchOptions);
        Context = await Browser.NewContextAsync();
        Page = await Context.NewPageAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTeardownAsync()
    {
        try { if (Context != null) await Context.CloseAsync(); } catch { }
        try { if (Browser != null) await Browser.CloseAsync(); } catch { }
        try { Playwright?.Dispose(); } catch { }
    }

    [TearDown]
    public async Task AfterEachAsync()
    {
        // recreate context/page to isolate tests
        try { if (Context != null) await Context.CloseAsync(); } catch { }
        Context = await Browser.NewContextAsync();
        Page = await Context.NewPageAsync();
    }
}
