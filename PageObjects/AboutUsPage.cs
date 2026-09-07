using Microsoft.Playwright;

namespace MultiBank.QA.Automation.PageObjects;

/// <summary>
/// Page Object Model for About Us > Why MultiBank Page
/// </summary>
public class AboutUsPage : BasePage
{
    IPage _page;

    public AboutUsPage(IPage page) : base(page)
    {
        _page = page;
    }

    // Selectors
    private ILocator logoButton => _page.Locator("img[alt='Logo']");
    private ILocator companyLink => _page.Locator("a[href='/en/company']");
    private const string PageHeading = "h1, [class*='heading'], .page-title";
    private const string SectionHeadings = "h2, h3, [class*='section-title']";
    private const string SectionContent = "section, .content-section, [class*='section']";
   

    /// <summary>
    /// Navigate to Why MultiBank page
    /// </summary>
    public async Task NavigateToWhyMultiBankAsync()
    {

        await logoButton.ClickAsync();
        await companyLink.ClickAsync();
        
    }

    /// <summary>
    /// Get page main heading
    /// </summary>
    public async Task<string?> GetPageHeadingAsync()
    {
        try
        {
            var heading = _page.Locator(PageHeading).First;
            return await heading.TextContentAsync();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Get all section headings
    /// </summary>
    public async Task<List<string>> GetSectionHeadingsAsync()
    {
        var headings = new List<string>();
        
        try
        {
            var locators = GetElements(SectionHeadings);
            var count = await locators.CountAsync();

            for (int i = 0; i < count; i++)
            {
                var text = await locators.Nth(i).TextContentAsync();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    headings.Add(text.Trim());
                }
            }
        }
        catch
        {
            // Return empty list if no headings found
        }

        return headings;
    }

    /// <summary>
    /// Get sections count
    /// </summary>
    public async Task<int> GetSectionsCountAsync()
    {
        return await GetElementCountAsync(SectionContent);
    }

 

    /// <summary>
    /// Verify all expected sections are present
    /// </summary>
    public async Task<bool> VerifyExpectedSectionsAsync()
    {
        try
        {
            var heading = await GetPageHeadingAsync();
            Assert.AreEqual("Why MultiBank Group?", heading, "Page heading should be 'Why MultiBank'");

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }


    /// <summary>
    /// Verify all expected components render
    /// </summary>
    public async Task<bool> VerifyAllComponentsRenderAsync()
    {
        var hasHeading = !string.IsNullOrEmpty(await GetPageHeadingAsync());
        var hasSections = await GetSectionsCountAsync() > 0;
        var hasHeadings = (await GetSectionHeadingsAsync()).Count > 0;

        return hasHeading && hasSections && hasHeadings;
    }
}
