using FluentAssertions;
using Microsoft.Playwright;

namespace MultiBank.QA.Automation.PageObjects;

/// <summary>
/// Page Object Model for MultiBank Home Page
/// </summary>
public class HomePage : BasePage
{
    private readonly IPage _page;
    public HomePage(IPage page) : base(page)
    {
        _page = page;
    }

    // Selectors

    private ILocator logoButton => _page.Locator("img[alt='Logo']");
    private ILocator NavigationItems => _page.GetByRole(AriaRole.Navigation, new() { Name = "Main" });

    private ILocator MarketingBanner => _page.Locator("div.absolute.inset-0");
    private ILocator AppStoreLink => _page.Locator("button[aria-label='Continue with Apple']");
    private ILocator GooglePlayLink => _page.Locator("button[aria-label='Continue with Google']");
    private const string TradingSection = "[class*='trading'], [class*='market'], .spot-trading";
    


    /// <summary>
    /// Navigate to home page
    /// </summary>
    public async Task NavigateAsync()
    {
        await NavigateToAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Get all navigation items
    /// </summary>
    public async Task<List<string>> GetNavigationItemsAsync()
    {
        await logoButton.ClickAsync();
        await NavigationItems.IsVisibleAsync();
        var items = new List<string>();
        var locators = await NavigationItems.GetByRole(AriaRole.Link).AllAsync();
        var count = await NavigationItems.CountAsync();

        foreach (var locator in locators)
        {
            var text = await locator.TextContentAsync();
            if (!string.IsNullOrWhiteSpace(text))
            {
                items.Add(text.Trim());
            }
        }

        return items;
    }

    /// <summary>
    /// Get navigation item by text
    /// </summary>
    public ILocator GetNavigationItem(string text)
    {
        return _page.Locator($"nav a:has-text('{text}'), header nav a:has-text('{text}')").First;
    }

    /// <summary>
    /// Click navigation item
    /// </summary>
    public async Task ClickNavigationItemAsync(string text)
    {
        var item = GetNavigationItem(text);
        await item.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Get navigation item href
    /// </summary>
    public async Task<string?> GetNavigationItemHrefAsync(string text)
    {
        var item = GetNavigationItem(text);
        return await item.GetAttributeAsync("href");
    }

    /// <summary>
    /// Check if marketing banner is visible
    /// </summary>
    public async Task<bool> IsMarketingBannerVisibleAsync()
    {
        return await MarketingBanner.IsVisibleAsync();
    }

    /// <summary>
    /// Get marketing banners count
    /// </summary>
    public async Task<int> GetMarketingBannersCountAsync()
    {
        return await MarketingBanner.CountAsync();
    }

    /// <summary>
    /// Check if App Store link exists
    /// </summary>
    public async Task<bool> HasAppStoreLinkAsync()
    {
        return await AppStoreLink.IsVisibleAsync();

    }

    /// <summary>
    /// Check if Google Play link exists
    /// </summary>
    public async Task<bool> HasGooglePlayLinkAsync()
    {
        return await GooglePlayLink.IsVisibleAsync();
    }

    /// <summary>
    /// Get App Store link URL
    /// </summary>
    public async Task ClickAppStoreLinkUrlAsync()
    {
        await AppStoreLink.ClickAsync();

        var url = _page.Url;
        url.Should().Contain("appleid.apple.com", "App Store link should point to Apple App Store");
    }

    /// <summary>
    /// Get Google Play link URL
    /// </summary>
    public async Task ClickGooglePlayLinkUrlAsync()
    {
        await GooglePlayLink.ClickAsync();

        var url = _page.Url;
        url.Should().Contain("accounts.google.com", "Google Play link should point to Google Play Store");
    }


    /// <summary>
    /// Validate all navigation links are not broken
    /// </summary>
    public async Task<Dictionary<string, (bool IsValid, int StatusCode)>> ValidateNavigationLinksAsync()
    {
        var results = new Dictionary<string, (bool IsValid, int StatusCode)>();
        var items = await GetNavigationItemsAsync();

        foreach (var item in items)
        {
            var href = await GetNavigationItemHrefAsync(item);
            if (!string.IsNullOrEmpty(href))
            {
                var fullUrl = href.StartsWith("http") ? href : $"{BaseUrl}{href}";
                results[item] = await IsLinkValidAsync(fullUrl);
            }
        }

        return results;
    }

    /// <summary>
    /// Get viewport size
    /// </summary>
    public PageViewportSizeResult? GetViewportSize()
    {
        return _page.ViewportSize;
    }

    /// <summary>
    /// Set viewport size
    /// </summary>
    public async Task SetViewportSizeAsync(int width, int height)
    {
        await _page.SetViewportSizeAsync(width, height);
    }
}
