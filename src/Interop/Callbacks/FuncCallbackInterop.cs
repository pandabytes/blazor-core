namespace Blazor.Core.Interop.Callbacks;

/// <summary>
/// This class represents a Func callback for JS interop.
/// </summary>
/// <typeparam name="TResult">Return type of the Func callback.</typeparam>
public class FuncCallbackInterop<TResult> : BaseCallbackInterop
{
  private class JSInteropWrapper
  {
    private readonly Func<TResult> _callback;

    public JSInteropWrapper(Func<TResult> callback) => _callback = callback;

    [JSInvokable]
    public TResult Invoke() => _callback.Invoke();
  }

  /// <inheritdoc />
  public sealed override bool IsAsync
  {
    get
    {
      var resultType = typeof(TResult);
      return resultType == typeof(Task) || 
             (resultType.IsGenericType && 
              resultType.GetGenericTypeDefinition() == typeof(Task<>));
    }
  }

  /// <summary>
  /// For derived classes.
  /// </summary>
  protected FuncCallbackInterop() {}

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public FuncCallbackInterop(Func<TResult> callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropWrapper(callback));

  /// <summary>
  /// Easy way to convert to a callback interop object.
  /// Just make sure to dispose it afterwards.
  /// </summary>
  /// <param name="callback">Callback.</param>
  public static implicit operator FuncCallbackInterop<TResult>(Func<TResult> callback)
    => new(callback);
}

/// <summary>
/// This class represents a Func callback for JS interop.
/// </summary>
/// <typeparam name="T">Type of the first parameter.</typeparam>
/// <typeparam name="TResult">Return type of the Func callback.</typeparam>
public sealed class FuncCallbackInterop<T, TResult> : FuncCallbackInterop<TResult>
{
  private class JSInteropWrapper
  {
    private readonly Func<T, TResult> _callback;

    public JSInteropWrapper(Func<T, TResult> callback) => _callback = callback;

    [JSInvokable]
    public TResult Invoke(T arg) => _callback.Invoke(arg);
  }

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public FuncCallbackInterop(Func<T, TResult> callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropWrapper(callback));

  /// <summary>
  /// Easy way to convert to a callback interop object.
  /// Just make sure to dispose it afterwards.
  /// </summary>
  /// <param name="callback">Callback.</param>
  public static implicit operator FuncCallbackInterop<T, TResult>(Func<T, TResult> callback)
    => new(callback);
}

/// <summary>
/// This class represents a Func callback for JS interop.
/// </summary>
/// <typeparam name="T1">Type of the first parameter.</typeparam>
/// <typeparam name="T2">Type of the second parameter.</typeparam>
/// <typeparam name="TResult">Return type of the Func callback.</typeparam>
public sealed class FuncCallbackInterop<T1, T2, TResult> : FuncCallbackInterop<TResult>
{
  private class JSInteropWrapper
  {
    private readonly Func<T1, T2, TResult> _callback;

    public JSInteropWrapper(Func<T1, T2, TResult> callback) => _callback = callback;

    [JSInvokable]
    public TResult Invoke(T1 arg1, T2 arg2) => _callback.Invoke(arg1, arg2);
  }

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public FuncCallbackInterop(Func<T1, T2, TResult> callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropWrapper(callback));

  /// <summary>
  /// Easy way to convert to a callback interop object.
  /// Just make sure to dispose it afterwards.
  /// </summary>
  /// <param name="callback">Callback.</param>
  public static implicit operator FuncCallbackInterop<T1, T2, TResult>(Func<T1, T2, TResult> callback)
    => new(callback);
}

/// <summary>
/// This class represents a Func callback for JS interop.
/// </summary>
/// <typeparam name="T1">Type of the first parameter.</typeparam>
/// <typeparam name="T2">Type of the second parameter.</typeparam>
/// <typeparam name="T3">Type of the third parameter.</typeparam>
/// <typeparam name="TResult">Return type of the Func callback.</typeparam>
public sealed class FuncCallbackInterop<T1, T2, T3, TResult> : FuncCallbackInterop<TResult>
{
  private class JSInteropWrapper
  {
    private readonly Func<T1, T2, T3, TResult> _callback;

    public JSInteropWrapper(Func<T1, T2, T3, TResult> callback) => _callback = callback;

    [JSInvokable]
    public TResult Invoke(T1 arg1, T2 arg2, T3 arg3) => _callback.Invoke(arg1, arg2, arg3);
  }

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public FuncCallbackInterop(Func<T1, T2, T3, TResult> callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropWrapper(callback));

  /// <summary>
  /// Easy way to convert to a callback interop object.
  /// Just make sure to dispose it afterwards.
  /// </summary>
  /// <param name="callback">Callback.</param>
  public static implicit operator FuncCallbackInterop<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> callback)
    => new(callback);
}

/// <summary>
/// This class represents a Func callback for JS interop.
/// </summary>
/// <typeparam name="T1">Type of the first parameter.</typeparam>
/// <typeparam name="T2">Type of the second parameter.</typeparam>
/// <typeparam name="T3">Type of the third parameter.</typeparam>
/// <typeparam name="T4">Type of the fourth parameter.</typeparam>
/// <typeparam name="TResult">Return type of the Func callback.</typeparam>
public sealed class FuncCallbackInterop<T1, T2, T3, T4, TResult> : FuncCallbackInterop<TResult>
{
  private class JSInteropWrapper
  {
    private readonly Func<T1, T2, T3, T4, TResult> _callback;

    public JSInteropWrapper(Func<T1, T2, T3, T4, TResult> callback) => _callback = callback;

    [JSInvokable]
    public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) => _callback.Invoke(arg1, arg2, arg3, arg4);
  }

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public FuncCallbackInterop(Func<T1, T2, T3, T4, TResult> callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropWrapper(callback));

  /// <summary>
  /// Easy way to convert to a callback interop object.
  /// Just make sure to dispose it afterwards.
  /// </summary>
  /// <param name="callback">Callback.</param>
  public static implicit operator FuncCallbackInterop<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> callback)
    => new(callback);
}

/// <summary>
/// This class represents a Func callback for JS interop.
/// </summary>
/// <typeparam name="T1">Type of the first parameter.</typeparam>
/// <typeparam name="T2">Type of the second parameter.</typeparam>
/// <typeparam name="T3">Type of the third parameter.</typeparam>
/// <typeparam name="T4">Type of the fourth parameter.</typeparam>
/// <typeparam name="T5">Type of the fifth parameter.</typeparam>
/// <typeparam name="TResult">Return type of the Func callback.</typeparam>
public sealed class FuncCallbackInterop<T1, T2, T3, T4, T5, TResult> : FuncCallbackInterop<TResult>
{
  private class JSInteropWrapper
  {
    private readonly Func<T1, T2, T3, T4, T5, TResult> _callback;

    public JSInteropWrapper(Func<T1, T2, T3, T4, T5, TResult> callback) => _callback = callback;

    [JSInvokable]
    public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
      => _callback.Invoke(arg1, arg2, arg3, arg4, arg5);
  }

  /// <summary>
  /// Construct with the given <paramref name="callback"/>.
  /// </summary>
  public FuncCallbackInterop(Func<T1, T2, T3, T4, T5, TResult> callback)
    => DotNetRef = DotNetObjectReference.Create(new JSInteropWrapper(callback));

  /// <summary>
  /// Easy way to convert to a callback interop object.
  /// Just make sure to dispose it afterwards.
  /// </summary>
  /// <param name="callback">Callback.</param>
  public static implicit operator FuncCallbackInterop<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> callback)
    => new(callback);
}

