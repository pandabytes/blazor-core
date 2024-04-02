namespace Blazor.Interop.CallbackInterops;

/// <summary>
/// Base class that represent a callback for JS interop.
/// Borrow from https://remibou.github.io/How-to-send-callback-to-JS-Interop-in-Blazor/
/// </summary>
public abstract class BaseCallbackInterop : IDisposable
{
  private bool _disposed;

  #pragma warning disable CA1822

  /// <summary>
  /// This is only used to help JS know this object is a CallbackInterop instance.
  /// This property always returns true.
  /// </summary>
  public bool IsCallbackInterop => true;

  /// <summary>
  /// This is only used to help JS know this object is a CallbackInterop instance.
  /// This property always "Blazor.Interop".
  /// </summary>
  public string AssemblyName => "Blazor.Interop";

  #pragma warning restore CA1822

  /// <summary>
  /// The dotnet object that contains a referene
  /// to a C# callback.
  /// </summary>
  // We don't want to expose private class JSInterop<T>Wrapper hence we return it as object
  public object? DotNetRef { get; protected set; }

  /// <summary>
  /// Return true if the callback returns a <see cref="Task"/>
  /// in which indicates that it should be "awaited". <see cref="Task"/> 
  /// is equivalent to Promise in JS. This helps JS code know to whether
  /// await the the callback or not.
  /// </summary>
  public abstract bool IsAsync { get; }

  /// <summary>
  /// Constructor.
  /// </summary>
  protected BaseCallbackInterop()
  {
    _disposed = false;
    DotNetRef = null;
  }

  /// <inheritdoc />
  public void Dispose()
  {
    if (_disposed)
    {
      return;
    }

    var disposable = DotNetRef as IDisposable;
    disposable?.Dispose();

    DotNetRef = null;
    _disposed = true;

    GC.SuppressFinalize(this);
  }
}
