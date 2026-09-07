using Microsoft.Playwright;
using MultiBank.QA.Automation.Helpers;

namespace MultiBank.QA.Automation.PageObjects;

/// <summary>
/// Base page class with common functionality for all page objects
/// </summary>
public abstract class BasePage
{
    protected readonly IPage _page;
    protected readonly string BaseUrl;

    protected BasePage(IPage page)
    {
        _page = page;
        BaseUrl = TestConfig.BaseUrl;
    }

    /// <summary>
    /// Navigate to specific path
    /// </summary>
    public virtual async Task NavigateToAsync(string path = "")
    {
        var url = string.IsNullOrEmpty(path) ? BaseUrl : $"{BaseUrl}{path}";
        Console.WriteLine(url);

        
        var response = await _page.GotoAsync(url, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = TestConfig.DefaultTimeout
        });
        Console.WriteLine(response?.Status);
    }

    /// <summary>
    /// Get page title
    /// </summary>
    public async Task<string> GetTitleAsync()
    {
        return await _page.TitleAsync();
    }

    /// <summary>
    /// Get current URL
    /// </summary>
    public string GetCurrentUrl()
    {
        return _page.Url;
    }

    /// <summary>
    /// Take screenshot
    /// </summary>
    public async Task<byte[]> TakeScreenshotAsync(string? name = null)
    {
        var screenshotDir = TestConfig.ScreenshotDir;
        Directory.CreateDirectory(screenshotDir);

        var fileName = name ?? $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
        var path = Path.Combine(screenshotDir, fileName);

        return await _page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = path,
            FullPage = true
        });
    }

    /// <summary>
    /// Wait for element to be visible
    /// </summary>
    protected async Task<ILocator> WaitForElementAsync(string selector, int? timeout = null)
    {
        return await _page.WaitForElementToBeVisibleAsync(selector, timeout);
    }

    /// <summary>
    /// Check if element is visible
    /// </summary>
    protected async Task<bool> IsElementVisibleAsync(string selector, int? timeout = 5000)
    {
        try
        {
            var locator = _page.Locator(selector);
            await locator.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = timeout
            });
            return await locator.IsVisibleAsync();
        }
        catch
        {
            return false;
        }
    }



    /// <summary>
    /// Get all elements matching selector
    /// </summary>
    protected ILocator GetElements(string selector)
    {
        return _page.Locator(selector);
    }



    /// <summary>
    /// Check if link is valid (returns 200 status)
    /// </summary>
    protected async Task<(bool IsValid, int StatusCode)> IsLinkValidAsync(string url)
    {
        try
        {
            var response = await _page.Context.APIRequest.GetAsync(url);
            return (response.Ok, response.Status);
        }
        catch
        {
            return (false, 0);
        }
    }


    /// <summary>
    /// Get count of elements
    /// </summary>
    protected async Task<int> GetElementCountAsync(string selector)
    {
        return await _page.Locator(selector).CountAsync();
    }
}
