namespace Blazor.Core.Interop;

/// <summary>
/// Encapsulate the `callback-reviver.js` module.
/// This class allows C# callback to be passed
/// to JS and have JS calls C# callback.
/// </summary>
/// <param name="jSRuntime">JS runtime.</param>
internal sealed class CallbackReviverJsModule(IJSRuntime jSRuntime) : BaseJsModule(jSRuntime)
{
  private const string DotNetReviverHandlerObj = "CallbackReviverObj";

  /// <inheritdoc/>
  protected override string ModulePath => $"{ModulePrefixPath}/js/{nameof(Interop)}/callback-reviver.js";

  /// <summary>
  /// Register the reviver to the DotNet object
  /// in JS. Calling this multiple times will not affect
  /// anything.
  /// </summary>
  public async Task RegisterReviverAsync()
    => await Module.InvokeVoidAsync($"{DotNetReviverHandlerObj}.registerReviver");
}
