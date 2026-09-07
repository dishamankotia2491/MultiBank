using Microsoft.Playwright;

namespace MultiBank.QA.Automation.Helpers;

/// <summary>
/// Custom wait utilities for resilient element interactions
/// </summary>
public static class WaitHelpers
{
    /// <summary>
    /// Wait for element to be visible and stable
    /// </summary>
    public static async Task<ILocator> WaitForElementToBeVisibleAsync(this IPage page, string selector, int? timeout = null)
    {
        var locator = page.Locator(selector);
        await locator.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = timeout ?? TestConfig.DefaultTimeout
        });
        return locator;
    }
 

    /// <summary>
    /// Retry action with exponential backoff
    /// </summary>
    public static async Task<T> RetryAsync<T>(Func<Task<T>> action, int maxAttempts = 3, int delayMs = 1000)
    {
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                if (attempt == maxAttempts)
                    throw;

                Console.WriteLine($"Attempt {attempt} failed: {ex.Message}. Retrying in {delayMs}ms...");
                await Task.Delay(delayMs);
                delayMs *= 2; // Exponential backoff
            }
        }
        throw new InvalidOperationException("Retry logic failed unexpectedly");
    }
}
