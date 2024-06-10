namespace Blazor.Core.Interop.Callbacks;

/// <summary>
/// Base class that represent an <see cref="Action"/> callback for JS interop.
/// </summary>
public class ActionCallbackInterop : BaseCallbackInterop
{
  private class JSInteropWrapper
  {
    private readonly Action _callback;

    public JSInteropWrapper(Action callback) => _callback = callback;

    [JSInvokable]
    public void Invoke() => _callback.Invoke();
  }

  /// <inheritdoc/>
  public sealed override bool IsAsync => false;

  /// <summary>
  /// For derived classes.
  /// </summary>
  protected ActionCallbackInterop() {}

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public ActionCallbackInterop(Action callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropWrapper(callback));

  /// <summary>
  /// Easy way to convert to a callback interop object.
  /// Just make sure to dispose it afterwards.
  /// </summary>
  /// <param name="callback">Callback.</param>
  public static implicit operator ActionCallbackInterop(Action callback) => new(callback);
}

/// <summary>
/// This class represents an Action callback for JS interop.
/// </summary>
/// <typeparam name="T">
/// Type parameter of an <see cref="Action"/> that only accepts 1 parameter
/// </typeparam>
public sealed class ActionCallbackInterop<T> : ActionCallbackInterop
{
  private class JSInteropWrapper
  {
    private readonly Action<T> _callback;

    public JSInteropWrapper(Action<T> callback) => _callback = callback;

    [JSInvokable]
    public void Invoke(T arg) => _callback.Invoke(arg);
  }

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public ActionCallbackInterop(Action<T> callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropWrapper(callback));

  /// <summary>
  /// Easy way to convert to a callback interop object.
  /// Just make sure to dispose it afterwards.
  /// </summary>
  /// <param name="callback">Callback.</param>
  public static implicit operator ActionCallbackInterop<T>(Action<T> callback)
    => new(callback);
}

/// <summary>
/// This class represents an Action callback for JS interop.
/// </summary>
/// <typeparam name="T1">Type of the first parameter.</typeparam>
/// <typeparam name="T2">Type of the second parameter.</typeparam>
public sealed class ActionCallbackInterop<T1, T2> : ActionCallbackInterop
{
  private class JSInteropWrapper
  {
    private readonly Action<T1, T2> _callback;

    public JSInteropWrapper(Action<T1, T2> callback) => _callback = callback;

    [JSInvokable]
    public void Invoke(T1 arg1, T2 arg2) => _callback.Invoke(arg1, arg2);
  }

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public ActionCallbackInterop(Action<T1, T2> callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropWrapper(callback));

  /// <summary>
  /// Easy way to convert to a callback interop object.
  /// Just make sure to dispose it afterwards.
  /// </summary>
  /// <param name="callback">Callback.</param>
  public static implicit operator ActionCallbackInterop<T1, T2>(Action<T1, T2> callback)
    => new(callback);
}

/// <summary>
/// This class represents an Action callback for JS interop.
/// </summary>
/// <typeparam name="T1">Type of the first parameter.</typeparam>
/// <typeparam name="T2">Type of the second parameter.</typeparam>
/// <typeparam name="T3">Type of the third parameter.</typeparam>
public sealed class ActionCallbackInterop<T1, T2, T3> : ActionCallbackInterop
{
  private class JSInteropWrapper
  {
    private readonly Action<T1, T2, T3> _callback;

    public JSInteropWrapper(Action<T1, T2, T3> callback) => _callback = callback;

    [JSInvokable]
    public void Invoke(T1 arg1, T2 arg2, T3 arg3) => _callback.Invoke(arg1, arg2, arg3);
  }

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public ActionCallbackInterop(Action<T1, T2, T3> callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropWrapper(callback));

  /// <summary>
  /// Easy way to convert to a callback interop object.
  /// Just make sure to dispose it afterwards.
  /// </summary>
  /// <param name="callback">Callback.</param>
  public static implicit operator ActionCallbackInterop<T1, T2, T3>(Action<T1, T2, T3> callback)
    => new(callback);
}

/// <summary>
/// This class represents an Action callback for JS interop.
/// </summary>
/// <typeparam name="T1">Type of the first parameter.</typeparam>
/// <typeparam name="T2">Type of the second parameter.</typeparam>
/// <typeparam name="T3">Type of the third parameter.</typeparam>
/// <typeparam name="T4">Type of the fourth parameter.</typeparam>
public sealed class ActionCallbackInterop<T1, T2, T3, T4> : ActionCallbackInterop
{
  private class JSInteropWrapper
  {
    private readonly Action<T1, T2, T3, T4> _callback;

    public JSInteropWrapper(Action<T1, T2, T3, T4> callback) => _callback = callback;

    [JSInvokable]
    public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) => _callback.Invoke(arg1, arg2, arg3, arg4);
  }

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public ActionCallbackInterop(Action<T1, T2, T3, T4> callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropWrapper(callback));

  /// <summary>
  /// Easy way to convert to a callback interop object.
  /// Just make sure to dispose it afterwards.
  /// </summary>
  /// <param name="callback">Callback.</param>
  public static implicit operator ActionCallbackInterop<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
    => new(callback);
}

/// <summary>
/// This class represents an Action callback for JS interop.
/// </summary>
/// <typeparam name="T1">Type of the first parameter.</typeparam>
/// <typeparam name="T2">Type of the second parameter.</typeparam>
/// <typeparam name="T3">Type of the third parameter.</typeparam>
/// <typeparam name="T4">Type of the fourth parameter.</typeparam>
/// <typeparam name="T5">Type of the fifth parameter.</typeparam>
public sealed class ActionCallbackInterop<T1, T2, T3, T4, T5> : ActionCallbackInterop
{
  private class JSInteropWrapper
  {
    private readonly Action<T1, T2, T3, T4, T5> _callback;

    public JSInteropWrapper(Action<T1, T2, T3, T4, T5> callback) => _callback = callback;

    [JSInvokable]
    public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
      => _callback.Invoke(arg1, arg2, arg3, arg4, arg5);
  }

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public ActionCallbackInterop(Action<T1, T2, T3, T4, T5> callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropWrapper(callback));

  /// <summary>
  /// Easy way to convert to a callback interop object.
  /// Just make sure to dispose it afterwards.
  /// </summary>
  /// <param name="callback">Callback.</param>
  public static implicit operator ActionCallbackInterop<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> callback)
    => new(callback);
}
