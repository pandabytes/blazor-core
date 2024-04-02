using System.Reflection;
using Blazor.Interop.CallbackInterops;

namespace Blazor.Interop;

/// <summary>
/// Base class that represent a JS module.
/// </summary>
public abstract class BaseJsModule : IAsyncDisposable
{
  /// <summary>
  /// Name of this library so that
  /// derived classes knows where to
  /// load the JS files.
  /// </summary>
  protected string LibraryName =>
    GetType().Assembly.GetName().Name ??
    throw new InvalidOperationException("Fail to get library name.");

  /// <summary>
  /// The prefix path to where the module is located.
  /// </summary>
  protected string ModulePrefixPath => $"./_content/{LibraryName}";

  private IJSObjectReference? _module;

  private bool _disposed = false;

  /// <summary>
  /// The JS runtime used to run Javascript code.
  /// </summary>
  protected readonly IJSRuntime _jSRuntime;

  /// <summary>
  /// Some JS functions accept callbacks and since
  /// the implementation of callback for JS interop
  /// is IDispose, we need to keep track of these
  /// callback objects so that we dispose them.
  /// </summary>
  protected readonly IList<BaseCallbackInterop> _callbackInterops;

  /// <summary>
  /// The Javascript module that contains exported variables,
  /// classes, functions, etc...
  /// </summary>
  /// <exception cref="InvalidOperationException">
  /// Thrown when the module is null (i.e. not loaded yet).
  /// </exception>
  protected virtual IJSObjectReference Module
  {
    get
    {
      if (_module is null)
      {
        var moduleName = GetType().Name;
        var message = _disposed ? $"Module {moduleName} is already disposed." :
          $"Module at \"{ModulePath}\" is not loaded. " +
          $"Please use the method {nameof(LoadModuleAsync)} to load the module.";
        throw new InvalidOperationException(message);
      }

      return _module;
    }
  }

  /// <summary>
  /// Path to where the Javascript module is located.
  /// It can be a file path or URL to the module
  /// hosted on CDN.
  /// </summary>
  protected abstract string ModulePath { get; }

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="jSRuntime">The JS runtime used to run Javascript code.</param>
  protected BaseJsModule(IJSRuntime jSRuntime)
  {
    _jSRuntime = jSRuntime;
    _callbackInterops = new List<BaseCallbackInterop>();
  }

  /// <summary>
  /// Load the module defined at <see cref="ModulePath"/>
  /// asynchronously.
  /// </summary>
  /// <remarks>
  /// This only needs to be called once. Calling this method
  /// more than once will do nothing.
  /// </remarks>
  /// <returns>Empty task</returns>
  public virtual async Task LoadModuleAsync()
    => _module ??= await _jSRuntime.InvokeAsync<IJSObjectReference>("import", ModulePath);

  /// <inheritdoc/>
  public async ValueTask DisposeAsync()
  {
    // Component disposal can happen before/during component
    // initialization according to:
    // https://learn.microsoft.com/en-us/aspnet/core/blazor/components/lifecycle?view=aspnetcore-7.0#component-disposal-with-idisposable-and-iasyncdisposable
    // Hence we must explicitly check for null here
    if (_module is not null)
    {
      await _module.DisposeAsync();
      GC.SuppressFinalize(this);
      _disposed = true;
      _module = null;
    }

    foreach (var callbackInterop in _callbackInterops)
    {
      callbackInterop.Dispose();
    }
    _callbackInterops.Clear();
  }
}
