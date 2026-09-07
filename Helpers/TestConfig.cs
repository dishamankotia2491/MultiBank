namespace MultiBank.QA.Automation.Helpers;

/// <summary>
/// Centralized configuration management for test execution
/// </summary>
public static class TestConfig
{
    /// <summary>
    /// Base URL of the application under test
    /// </summary>
    public static string BaseUrl => Environment.GetEnvironmentVariable("BASE_URL") ?? "https://trade.mb.io";

    /// <summary>
    /// Default timeout for page loads and element visibility (in milliseconds)
    /// </summary>
    public static int DefaultTimeout => 50000;


    /// <summary>
    /// Screenshot directory
    /// </summary>
    public static string ScreenshotDir => Path.Combine(Directory.GetCurrentDirectory(), "screenshots");


    /// <summary>
    /// Whether to run tests in headless mode
    /// </summary>
    // Interpret HEADLESS environment variable as a boolean. Default to true (headless) when not provided.
    //public static bool Headless
    //{
    //    get
    //    {
    //        var val = Environment.GetEnvironmentVariable("HEADLESS");
    //        if (string.IsNullOrWhiteSpace(val))
    //            return true;

    //        if (bool.TryParse(val, out var parsed))
    //            return parsed;

    //        // Support numeric flags like 0/1
    //        return val.Trim() switch
    //        {
    //            "0" => false,
    //            "1" => true,
    //            _ => true
    //        };
    //    }
    //}

    ///// <summary>
    ///// Preferred browser channel (e.g., "chrome", "msedge"). If null/empty, Playwright default is used.
    ///// </summary>
    //public static string? BrowserChannel => Environment.GetEnvironmentVariable("BROWSER_CHANNEL");

}
