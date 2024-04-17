using Blazor.Core.Interop.Callbacks;

namespace Blazor.Core.Interop;

/// <summary>
/// Base class that represent a JS module.
/// </summary>
public abstract class BaseJsModule : IAsyncDisposable
{
  private IJSObjectReference? _module;

  /// <summary>
  /// Name of this library so that
  /// derived classes know where to
  /// load the JS files.
  /// </summary>
  protected string LibraryName =>
    GetType().Assembly.GetName().Name ??
    throw new InvalidOperationException("Fail to get library name.");

  /// <summary>
  /// The prefix path to where the module is located.
  /// </summary>
  protected string ModulePrefixPath => $"./_content/{LibraryName}";

  /// <summary>
  /// The JS runtime used to run Javascript code.
  /// </summary>
  protected IJSRuntime JSRuntime { get; private set; }

  /// <summary>
  /// Some JS functions accept callbacks and since
  /// the implementation of callback for JS interop
  /// uses IDispose, we need to keep track of these
  /// callback objects so that we dispose them later.
  /// </summary>
  protected IList<BaseCallbackInterop> CallbackInterops { get; private set; }

  /// <summary>
  /// The Javascript module that contains exported variables,
  /// classes, functions, etc...
  /// <see cref="ImportAsync" /> must be called first before
  /// this property can be accessed.
  /// </summary>
  /// <exception cref="InvalidOperationException">
  /// Thrown when the module is null (i.e. not loaded yet).
  /// </exception>
  protected IJSObjectReference Module
  {
    get
    {
      if (_module is null)
      {
        var disposed = ModuleStatus == JsModuleStatus.Disposed;
        var moduleName = GetType().Name;
        var message = disposed ? $"Module {moduleName} is already disposed." :
          $"Module at \"{ModulePath}\" is not loaded. " +
          $"Please use the method {nameof(ImportAsync)} to import the module first.";
        throw new InvalidOperationException(message);
      }

      return _module;
    }
    private set => _module = value;
  }

  /// <summary>
  /// Path to where the Javascript module is located.
  /// It can be a file path or URL to the module
  /// hosted on CDN.
  /// </summary>
  protected abstract string ModulePath { get; }

  /// <summary>
  /// Indicate the status of the JS module.
  /// </summary>
  public JsModuleStatus ModuleStatus { get; private set; }

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="jSRuntime">The JS runtime used to run Javascript code.</param>
  protected BaseJsModule(IJSRuntime jSRuntime)
  {
    JSRuntime = jSRuntime;
    CallbackInterops = new List<BaseCallbackInterop>();
    ModuleStatus = JsModuleStatus.NotImported;
  }

  /// <summary>
  /// Import the module defined at <see cref="ModulePath"/>
  /// asynchronously.
  /// </summary>
  /// <remarks>
  /// This only needs to be called once. Calling this method
  /// more than once will do nothing.
  /// </remarks>
  public async Task ImportAsync()
  {
    if (ModuleStatus == JsModuleStatus.Disposed)
    {
      var moduleName = GetType().Name;
      throw new InvalidOperationException($"Module {moduleName} is already disposed.");
    }

    _module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", ModulePath);
    ModuleStatus = JsModuleStatus.Imported;
  }

  /// <inheritdoc/>
  public async ValueTask DisposeAsync()
  {
    await DisposeAsyncCore();
    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Common asynchronous cleanup operation
  /// for subclasses to potentially override.
  /// </summary>
  protected virtual async ValueTask DisposeAsyncCore()
  {
    if (_module is not null)
    {
      await _module.DisposeAsync();
    }

    _module = null;
    ModuleStatus = JsModuleStatus.Disposed;

    if (CallbackInterops is not null)
    {
      foreach (var callbackInterop in CallbackInterops)
      {
        callbackInterop.Dispose();
      }
      CallbackInterops.Clear();
    }

    // Set this to null so that we can release
    // this reference for GC to collect
    CallbackInterops = null!;
  }
}
