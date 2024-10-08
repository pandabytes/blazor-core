using Microsoft.AspNetCore.Components;

namespace Blazor.Core.Components;

/// <summary>
/// This class provides a way to execute a list of <see cref="Func{TResult}"/> 
/// once during <see cref="ComponentBase.OnAfterRenderAsync(bool)"/> or
/// <see cref="ComponentBase.OnAfterRender(bool)"/>.
/// Insipired by https://swimburger.net/blog/dotnet/how-to-run-code-after-blazor-component-has-rendered.
/// 
/// The signature of the func takes no parameters and returns a boolean,
/// where returning false means the func needs to be tried again.
/// </summary>
public class AfterRenderExecuter
{
  /// <summary>
  /// List of async funcs to run during <see cref="ComponentBase.OnAfterRenderAsync(bool)"/>.
  /// Each of these funcs will run only once. The bool value indicates the
  /// followings: true means the func is considered succeeded, and false means
  /// the func fails and it needs to be retried.
  /// </summary>
  protected IList<Func<Task<bool>>> AsyncFuncsToRunAfterRender { get; }

  /// <summary>
  /// List of async funcs to run during <see cref="ComponentBase.OnAfterRender(bool)"/>.
  /// Each of these funcs will run only once. The bool value indicates the
  /// followings: true means the func is considered succeeded, and false means
  /// the func fails and it needs to be retried.
  /// </summary>
  protected IList<Func<bool>> FuncsToRunAfterRender { get; }

  /// <summary>
  /// Constructor.
  /// </summary>
  public AfterRenderExecuter()
  {
    AsyncFuncsToRunAfterRender = new List<Func<Task<bool>>>();
    FuncsToRunAfterRender = new List<Func<bool>>();
  }

  /// <summary>
  /// Add a func to be executed.
  /// </summary>
  /// <param name="func">Func.</param>
  public void Add(Func<Task<bool>> func) => AsyncFuncsToRunAfterRender.Add(func);

  /// <summary>
  /// Add a func to be executed.
  /// </summary>
  /// <param name="func">Func.</param>
  public void Add(Func<bool> func) => FuncsToRunAfterRender.Add(func);

  /// <summary>
  /// Execute the funcs in the order they were added, and
  /// remove them after they execute. If any one of the
  /// funcs returns false, it means they need to be retried
  /// the next time this method is called.
  /// </summary>
  /// <returns>A tuple where 1st item is the number of funcs executed
  /// and 2nd item is the number of funcs to be retried.</returns>
  public virtual async Task<(int, int)> ExecuteFuncsAsync()
  {
    var failedFuncs = new List<Func<Task<bool>>>();

    // Execute each sequentially
    foreach (var func in AsyncFuncsToRunAfterRender)
    {
      var succeeded = await func();
      if (!succeeded)
      {
        failedFuncs.Add(func);
      }
    }

    // Clear the list so that we only
    // run each func once
    int funcsCount = AsyncFuncsToRunAfterRender.Count;
    AsyncFuncsToRunAfterRender.Clear();

    // Retry the failed funcs
    foreach (var failedFunc in failedFuncs)
    {
      AsyncFuncsToRunAfterRender.Add(failedFunc);
    }

    return (funcsCount, failedFuncs.Count);
  }

  /// <summary>
  /// Execute the funcs in the order they were added, and
  /// remove them after they execute. If any one of the
  /// funcs returns false, it means they need to be retried
  /// the next time this method is called.
  /// </summary>
  /// <returns>A tuple where 1st item is the number of funcs executed
  /// and 2nd item is the number of funcs to be retried.</returns>
  public virtual (int, int) ExecuteFuncs()
  {
    var failedFuncs = new List<Func<bool>>();

    // Execute each sequentially
    foreach (var func in FuncsToRunAfterRender)
    {
      if (!func())
      {
        failedFuncs.Add(func);
      }
    }

    // Clear the list so that we only
    // run each func once
    int funcsCount = FuncsToRunAfterRender.Count;
    FuncsToRunAfterRender.Clear();

    // Retry the failed funcs
    foreach (var failedFunc in failedFuncs)
    {
      FuncsToRunAfterRender.Add(failedFunc);
    }

    return (funcsCount, failedFuncs.Count);
  }
}
