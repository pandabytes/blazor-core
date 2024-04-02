namespace Blazor.Interop;

/// <summary>
/// Encapsulate the `dotnet-callback.js` module.
/// This class has no functionality except to load
/// the JS module.
/// </summary>
public sealed class DotnetCallbackJsModule : BaseJsModule
{
  /// <inheritdoc/>
  protected override string ModulePath => $"{ModulePrefixPath}/js/dotnet-callback.js";

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="jSRuntime">JS runtime.</param>
  public DotnetCallbackJsModule(IJSRuntime jSRuntime) : base(jSRuntime)
  {}
}
