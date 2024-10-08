namespace Blazor.Core.Tests;

// This class is disabled to prevent installing PLaywright on CI environment
// [Collection(PlaywrightCollectionDefinition.CollectionName)]
public class IndexPageTests
{
  private const string AppUrl =  "http://localhost:5190/";

  private readonly PlaywrightFixture _playwrightFixture = null!;

  // Comment this constructor so that we don't request the playwright
  // fixture in which it will start the playwright installation
  //
  // public IndexPageTests(PlaywrightFixture playwrightFixture)
  // {
  //   _playwrightFixture = playwrightFixture;
  // }

  [InlineData(1)]
  [InlineData(5)]
  [Theory(Skip = "Temp skip E2E tests")]
  public async Task IndexPage_ClickFuncButton_FuncIsCalled(int clickCount)
  {
    await _playwrightFixture.GotoPageAsync(
      AppUrl,
      async (page) =>
      {
        // Act
        await page
          .GetByTestId(TestIds.FuncTestId)
          .ClickAsync(new() { ClickCount = clickCount });

        // Assert
        var items = page.GetByRole(AriaRole.Listitem);
        var formattedDateTimes = await items.AllTextContentsAsync();

        formattedDateTimes.Count.Should().Be(clickCount);

        foreach (var formattedDateTime in formattedDateTimes)
        {
          var parsed = DateTime.TryParse(formattedDateTime, out var _);
          parsed.Should().BeTrue();
        }
      },
      Browser.Chromium
    );
  }
}
