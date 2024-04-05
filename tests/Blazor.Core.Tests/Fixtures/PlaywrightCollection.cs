namespace Blazor.Core.Tests.Fixtures;

/// <summary>
/// PlaywrightCollection name that is used in the Collection
/// attribute on each test classes.
/// Like "[Collection(PlaywrightFixture.PlaywrightCollection)]"
/// </summary>
[CollectionDefinition(CollectionName)]
public class PlaywrightCollectionDefinition
  : ICollectionFixture<PlaywrightFixture>
{
  // This class is just xUnit plumbing code to apply
  // [CollectionDefinition] and the ICollectionFixture<>
  // interfaces. Witch in our case is parametrized
  // with the PlaywrightFixture.
  public const string CollectionName = nameof(PlaywrightCollectionDefinition);
}
