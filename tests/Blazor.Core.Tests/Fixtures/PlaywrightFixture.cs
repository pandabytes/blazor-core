namespace Blazor.Core.Tests.Fixtures;

/// <summary>
/// Browser types we can use in the PlaywrightFixture.
/// </summary>
public enum Browser
{
  Chromium,
  Firefox,
  Webkit,
}

/// <summary>
/// Playwright fixture implementing an asynchronous life cycle.
/// </summary>
public class PlaywrightFixture : IAsyncLifetime
{
  /// <summary>
  /// Playwright module.
  /// </summary>
  public IPlaywright Playwright { get; private set; } = null!;

  /// <summary>
  /// Chromium lazy initializer.
  /// </summary>
  public Lazy<Task<IBrowser>> ChromiumBrowser { get; private set; } = null!;
  
  /// <summary>
  /// Firefox lazy initializer.
  /// </summary>
  public Lazy<Task<IBrowser>> FirefoxBrowser { get; private set; } = null!;
  
  /// <summary>
  /// Webkit lazy initializer.
  /// </summary>
  public Lazy<Task<IBrowser>> WebkitBrowser { get; private set; } = null!;

  /// <summary>
  /// Initialize the Playwright fixture.
  /// </summary>
  public async Task InitializeAsync()
  {
    // Install Playwright and its dependencies.
    InstallPlaywright();

    // Create Playwright module.
    Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

    // Setup Browser lazy initializers.
    ChromiumBrowser = new Lazy<Task<IBrowser>>(
      Playwright.Chromium.LaunchAsync());
    FirefoxBrowser = new Lazy<Task<IBrowser>>(
      Playwright.Firefox.LaunchAsync());
    WebkitBrowser = new Lazy<Task<IBrowser>>(
      Playwright.Webkit.LaunchAsync());
  }

  /// <summary>
  /// Dispose all Playwright module resources.
  /// </summary>
  public async Task DisposeAsync()
  {
    if (Playwright != null)
    {
      if (ChromiumBrowser != null && ChromiumBrowser.IsValueCreated)
      {
        var browser = await ChromiumBrowser.Value;
        await browser.DisposeAsync();
      }
      if (FirefoxBrowser != null && FirefoxBrowser.IsValueCreated)
      {
        var browser = await FirefoxBrowser.Value;
        await browser.DisposeAsync();
      }
      if (WebkitBrowser != null && WebkitBrowser.IsValueCreated)
      {
        var browser = await WebkitBrowser.Value;
        await browser.DisposeAsync();
      }
      Playwright.Dispose();
      Playwright = null!;
    }
  }

  /// <summary>
  /// Open a Browser page and navigate to the given URL before
  /// applying the given test handler.
  /// </summary>
  /// <param name="url">URL to navigate to.</param>
  /// <param name="testHandler">Test handler to apply on the page.
  /// </param>
  /// <param name="browserType">The Browser to use to open the page.
  /// </param>
  /// <returns>The GotoPage task.</returns>
  public async Task GotoPageAsync(
      string url,
      Func<IPage, Task> testHandler,
      Browser browserType)
  {
    // select and launch the browser.
    var browser = await SelectBrowserAsync(browserType);

    // Create a new context with an option to ignore HTTPS errors.
    await using var context = await browser
      .NewContextAsync(
        new BrowserNewContextOptions
        {
          IgnoreHTTPSErrors = true
        });

    // Open a new page.
    var page = await context.NewPageAsync();
    page.Should().NotBeNull();

    try
    {
      // Navigate to the given URL and wait until loading
      // network activity is done.
      var gotoResult = await page.GotoAsync(
        url,
        new PageGotoOptions {
          WaitUntil = WaitUntilState.NetworkIdle
        });
      gotoResult.Should().NotBeNull();
      await gotoResult!.FinishedAsync();
      gotoResult.Ok.Should().BeTrue();

      // Run the actual test logic.
      await testHandler(page);
    }
    finally
    {
      // Make sure the page is closed 
      await page.CloseAsync();
    }
  }

  /// <summary>
  /// Select the IBrowser instance depending on the given browser
  /// enumeration value.
  /// </summary>
  /// <param name="browser">The browser to select.</param>
  /// <returns>The selected IBrowser instance.</returns>
  private Task<IBrowser> SelectBrowserAsync(Browser browser)
  {
    return browser switch
    {
      Browser.Chromium => ChromiumBrowser.Value,
      Browser.Firefox => FirefoxBrowser.Value,
      Browser.Webkit => WebkitBrowser.Value,
      _ => throw new NotImplementedException(),
    };
  }

  /// <summary>
  /// Install and deploy all binaries Playwright may need.
  /// </summary>
  private static void InstallPlaywright()
  {
    var exitCode = Microsoft.Playwright.Program.Main(["install-deps"]);

    if (exitCode != 0)
    {
      throw new Exception(
        $"Playwright exited with code {exitCode} on install-deps");
    }

    exitCode = Microsoft.Playwright.Program.Main(["install"]);
    if (exitCode != 0)
    {
      throw new Exception(
        $"Playwright exited with code {exitCode} on install");
    }
  }
}
