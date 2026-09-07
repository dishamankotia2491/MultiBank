using FluentAssertions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using MultiBank.QA.Automation.PageObjects;
using MultiBank.QA.Automation.Helpers;

namespace MultiBank.QA.Automation.Tests;

/// <summary>
/// Test suite for Content & Links scenarios
/// </summary>
[TestFixture]
[Category("Content")]
[Parallelizable(ParallelScope.Self)]
public class ContentTests : BasePlaywrightTest
{
   
    private HomePage? _homePage;
    private AboutUsPage? _aboutUsPage;

    [SetUp]
    public async Task Setup()
    {
        _homePage = new HomePage(Page);
        _aboutUsPage = new AboutUsPage(Page);
        await _homePage.NavigateAsync();
    }


    [Test]
    [Description("Verify App Store download link resolves correctly")]
    public async Task AppStoreDownloadLinkResolvesCorrectly()
    {
        // Check if App Store link exists
        var hasAppStoreLink = await _homePage!.HasAppStoreLinkAsync();
        
        if (!hasAppStoreLink)
        {
            // Scroll down to find app store links (often in footer)
            await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
            await Task.Delay(2000);
            hasAppStoreLink = await _homePage.HasAppStoreLinkAsync();
        }

        hasAppStoreLink.Should().BeTrue("App Store download link should exist on the page");

        await _homePage.ClickAppStoreLinkUrlAsync();
        
    }

    [Test]
    [Description("Verify Google Play download link resolves correctly")]
    public async Task GooglePlayDownloadLinkResolvesCorrectly()
    {
        // Check if Google Play link exists
        var hasGooglePlayLink = await _homePage!.HasGooglePlayLinkAsync();
        
        if (!hasGooglePlayLink)
        {
            // Scroll down to find app store links (often in footer)
            await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
            await Task.Delay(2000);
            hasGooglePlayLink = await _homePage.HasGooglePlayLinkAsync();
        }

        hasGooglePlayLink.Should().BeTrue("Google Play download link should exist on the page");

        await _homePage.ClickGooglePlayLinkUrlAsync();
    }

    

    [Test]
    [Description("Verify About Us page has correct headings and section text")]
    public async Task AboutUsPageHasCorrectHeadingsAndText()
    {
        await _aboutUsPage!.NavigateToWhyMultiBankAsync();
        await Task.Delay(2000);

        // Verify all components render
        var componentsRender = await _aboutUsPage.VerifyAllComponentsRenderAsync();
        componentsRender.Should().BeTrue("All page components should render correctly");

        // Verify expected sections
        var sections = await _aboutUsPage.VerifyExpectedSectionsAsync();
        sections.Should().BeTrue("Expected sections should be dispalyed");

    }

    [Test]
    [Description("Verify marketing banners render in the expected page region")]
    public async Task MarketingBannersRenderInExpectedRegion()
    {
        // Check if marketing banner is visible
        var isBannerVisible = await _homePage!.IsMarketingBannerVisibleAsync();

        if (!isBannerVisible)
        {
            // Wait a bit more for lazy-loaded content
            await Task.Delay(3000);
            isBannerVisible = await _homePage.IsMarketingBannerVisibleAsync();
        }

        isBannerVisible.Should().BeTrue("Marketing banner should be visible on the home page");

        // Get banners count
        var bannersCount = await _homePage.GetMarketingBannersCountAsync();
        bannersCount.Should().BeGreaterThan(0, "At least one marketing banner should be present");

        // Take screenshot for evidence
        await _homePage.TakeScreenshotAsync("marketing_banners.png");

        Console.WriteLine($"Marketing banners verified: {bannersCount} banner(s) found");
    }

}
