using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using MultiBank.QA.Automation.PageObjects;
using MultiBank.QA.Automation.Helpers;
using FluentAssertions;

namespace MultiBank.QA.Automation.Tests;

/// <summary>
/// Test suite for Negative / Edge Cases scenarios
/// </summary>
[TestFixture]
[Category("EdgeCases")]
[Parallelizable(ParallelScope.Self)]
public class EdgeCaseTests : BasePlaywrightTest
{
    private HomePage? _homePage;

    [SetUp]
    public async Task Setup()
    {
        _homePage = new HomePage(Page);
    }

    [Test]
    [Description("Verify invalid route handling returns 404 or redirects appropriately")]
    [TestCase("/invalid-page-12345")]
    public async Task InvalidRouteHandlingReturns404OrRedirects(string invalidRoute)
    {
        // Navigate to invalid route
        var response = await Page.GotoAsync($"{TestConfig.BaseUrl}{invalidRoute}"); //, new PageGotoOptions
 

        // Verify response
        response.Should().NotBeNull("Response should be received");

        // Accept either 404 status or redirect to valid page
        var isHandledGracefully = response!.Status == 404;

        isHandledGracefully.Should().BeTrue(
            $"Invalid route should be handled gracefully (got status {response.Status})");

        // Check if page shows 404 content or redirects to home
        var currentUrl = Page.Url;
        var pageContent = await Page.TextContentAsync("body");

        var is404Page = pageContent?.Contains("404", StringComparison.OrdinalIgnoreCase) ?? false;

        (is404Page || response.Status == 404).Should().BeTrue(
            "Invalid route should show 404 page or redirect to home");

        // Take screenshot
        await Page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = Path.Combine(TestConfig.ScreenshotDir, $"invalid_route_{invalidRoute.Replace("/", "_")}.png"),
            FullPage = true
        });

        Console.WriteLine($"Invalid route '{invalidRoute}' handled: Status={response.Status}, 404Page={is404Page}");
    }

    [Test]
    [Description("Verify broken link detection across navigation links")]
    public async Task BrokenLinkDetectionAcrossNavigationLinks()
    {
        await _homePage!.NavigateAsync();

        // Get all navigation links and validate them
        var linkValidation = await _homePage.ValidateNavigationLinksAsync();
        linkValidation.Should().NotBeEmpty("Navigation should have links to validate");

        // Check results
        var brokenLinks = linkValidation.Where(x => !x.Value.IsValid).ToList();
        var validLinks = linkValidation.Where(x => x.Value.IsValid).ToList();

        Console.WriteLine($"\nNavigation Link Validation Results:");
        Console.WriteLine($"Total links: {linkValidation.Count}");
        Console.WriteLine($"Valid links: {validLinks.Count}");
        Console.WriteLine($"Broken links: {brokenLinks.Count}");

    }

    [Test]
    [Description("Verify viewport regression at mobile breakpoint")]
    [TestCase(375, 667, "iPhone X")]
    [TestCase(390, 844, "iPhone 12 Pro")]
    [TestCase(360, 800, "Samsung Galaxy S20")]
    public async Task ViewportRegressionAtMobileBreakpoint(int width, int height, string deviceName)
    {
        // Set mobile viewport
        await Page.SetViewportSizeAsync(width, height);
        await _homePage!.NavigateAsync();
        await Task.Delay(2000); // Wait for responsive adjustments

        // Verify page loads at mobile viewport
        var currentUrl = _homePage.GetCurrentUrl();
        currentUrl.Should().NotBeNullOrEmpty("Page should load at mobile viewport");

        // Verify viewport size is set
        var viewportSize = _homePage.GetViewportSize();
        viewportSize.Should().NotBeNull();
        viewportSize!.Width.Should().Be(width);
        viewportSize.Height.Should().Be(height);
        
        // Navigation should exist in some form (visible or hidden behind hamburger)
        // We'll check if the page has meaningful content instead
        var bodyContent = await Page.TextContentAsync("body");
        bodyContent.Should().NotBeNullOrEmpty("Page should have content at mobile viewport");

        // Verify no horizontal scroll (content should fit viewport)
        var scrollWidth = await Page.EvaluateAsync<int>("document.documentElement.scrollWidth");
        var clientWidth = await Page.EvaluateAsync<int>("document.documentElement.clientWidth");
        
        // Allow small difference for scrollbars
        Math.Abs(scrollWidth - clientWidth).Should().BeLessThanOrEqualTo(20, 
            "Page should not have significant horizontal scroll at mobile viewport");

        // Take screenshot for visual regression
        await Page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = Path.Combine(TestConfig.ScreenshotDir, $"mobile_{width}x{height}_{deviceName.Replace(" ", "_")}.png"),
            FullPage = true
        });

        Console.WriteLine($"Mobile viewport verified for {deviceName} ({width}x{height}):");
       
        Console.WriteLine($"  Content length: {bodyContent?.Length ?? 0} chars");
        Console.WriteLine($"  Horizontal scroll: {scrollWidth} vs {clientWidth}");
    }

    [Test]
    [Description("Verify content loading timeout handling")]
    public async Task ContentLoadingTimeoutHandling()
    {
        var testTimeout = 5000; // 5 second timeout for this test

        try
        {
            // Attempt to load page with short timeout
            var response = await Page.GotoAsync(TestConfig.BaseUrl, new PageGotoOptions
            {
                Timeout = testTimeout,
                WaitUntil = WaitUntilState.NetworkIdle
            });

            // If we get here, page loaded within timeout
            response.Should().NotBeNull("Page should load or timeout gracefully");
            response!.Ok.Should().BeTrue("Page should load successfully");

            Console.WriteLine($"✓ Page loaded successfully within {testTimeout}ms timeout");
        }
        catch (TimeoutException ex)
        {
            // Timeout occurred - verify it's handled gracefully
            Console.WriteLine($"✓ Timeout occurred as expected: {ex.Message}");
            
            // Verify browser didn't crash
            var isPageAccessible = !Page.IsClosed;
            isPageAccessible.Should().BeTrue("Page should remain accessible after timeout");
        }

        // Verify page can still be interacted with after timeout scenario
        await _homePage!.NavigateAsync(); // Regular navigation with normal timeout
        var title = await _homePage.GetTitleAsync();
        title.Should().NotBeNullOrEmpty("Page should be functional after timeout scenario");

        Console.WriteLine("✓ Timeout handling verified - page remains functional");
    }

}
