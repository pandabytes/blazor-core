namespace Blazor.Core.Tests;

[Collection(PlaywrightCollectionDefinition.CollectionName)]
public class IndexPageTests
{
  private const string AppUrl =  "http://localhost:5190/";

  private readonly PlaywrightFixture _playwrightFixture;

  /// <summary>
  /// Setup test class injecting a playwrightFixture instance.
  /// </summary>
  /// <param name="playwrightFixture">The playwrightFixture
  /// instance.</param>
  public IndexPageTests(PlaywrightFixture playwrightFixture)
  {
    _playwrightFixture = playwrightFixture;
  }

  [InlineData(1)]
  [InlineData(5)]
  [Theory]
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
