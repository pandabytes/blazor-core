namespace Blazor.Core.Interop.Callbacks;

/// <summary>
/// Base class that represent an <see cref="Action"/> callback for JS interop.
/// </summary>
public abstract class ActionCallbackInterop : BaseCallbackInterop
{
  /// <inheritdoc/>
  public sealed override bool IsAsync => false;
}

/// <summary>
/// This class represents an Action callback for JS interop.
/// </summary>
/// <typeparam name="T">
/// Type parameter of an <see cref="Action"/> that only accepts 1 parameter
/// </typeparam>
public sealed class ActionCallbackInterop<T> : ActionCallbackInterop
{
  private class JSInteropActionWrapper
  {
    private readonly Action<T> _callback;

    public JSInteropActionWrapper(Action<T> callback) => _callback = callback;

    [JSInvokable]
    public void Invoke(T arg) => _callback.Invoke(arg);
  }

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public ActionCallbackInterop(Action<T> callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropActionWrapper(callback));
}

/// <summary>
/// This class represents an Action callback for JS interop.
/// </summary>
/// <typeparam name="T1">Type of the first parameter.</typeparam>
/// <typeparam name="T2">Type of the second parameter.</typeparam>
public sealed class ActionCallbackInterop<T1, T2> : ActionCallbackInterop
{
  private class JSInteropActionWrapper
  {
    private readonly Action<T1, T2> _callback;

    public JSInteropActionWrapper(Action<T1, T2> callback) => _callback = callback;

    [JSInvokable]
    public void Invoke(T1 arg1, T2 arg2) => _callback.Invoke(arg1, arg2);
  }

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public ActionCallbackInterop(Action<T1, T2> callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropActionWrapper(callback));
}
