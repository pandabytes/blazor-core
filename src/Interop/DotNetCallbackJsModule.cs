namespace Blazor.Core.Interop;

/// <summary>
/// Encapsulate the `dotnet-callback.js` module.
/// This class allows C# callback to be passed
/// to JS and have JS calls C# callback.
/// </summary>
public sealed class DotNetCallbackJsModule : BaseJsModule
{
  private const string DotNetReviverHandlerObj = "DotNetReviverHandlerObj";

  /// <inheritdoc/>
  protected override string ModulePath => $"{ModulePrefixPath}/js/{nameof(Interop)}/dotnet-callback.js";

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="jSRuntime">JS runtime.</param>
  public DotNetCallbackJsModule(IJSRuntime jSRuntime) : base(jSRuntime)
  {}

  /// <summary>
  /// Register the attach reviver to the DotNet object
  /// in JS. Calling this multiple times will not affect
  /// anything.
  /// </summary>
  public async Task RegisterAttachReviverAsync()
    => await Module.InvokeVoidAsync($"{DotNetReviverHandlerObj}.registerAttachReviver");
}
