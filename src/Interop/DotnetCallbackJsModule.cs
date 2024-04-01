namespace Blazor.Interop;

internal sealed class DotnetCallbackJsModule : BaseJsModule
{
  /// <inheritdoc/>
  protected override string ModulePath { get; }

  public DotnetCallbackJsModule(IJSRuntime jSRuntime) : base(jSRuntime)
  {
    var pathToJsModule = $"{nameof(Interop)}/dotnet-callback.js";
    ModulePath = $"{ModulePrefixPath}/{pathToJsModule}";
  }
}
