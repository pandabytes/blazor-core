namespace Blazor.Core.Tests.Components;

public abstract class ScopeService
{
  public bool IsDisposed { get; protected set; } = false;

  public bool IsDisposeCalled { get; protected set; } = false;

  public bool IsDisposeAsyncCalled { get; protected set; } = false;
}

public class ScopeServiceDispose : ScopeService, IDisposable
{
  public void Dispose()
  {
    Dispose(true);
    IsDisposeCalled = true;
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    IsDisposed = true;
  }
}

public class ScopeServiceDisposeAsync : ScopeService, IAsyncDisposable
{
  public ValueTask DisposeAsync()
  {
    IsDisposeAsyncCalled = true;
    IsDisposed = true;
    GC.SuppressFinalize(this);
    return ValueTask.CompletedTask;
  }
}

public class ScopeServiceDisposeAndDisposeAsync : ScopeServiceDispose, IAsyncDisposable
{
  public ValueTask DisposeAsync()
  {
    IsDisposeAsyncCalled = true;
    Dispose(false);
    GC.SuppressFinalize(this);
    return ValueTask.CompletedTask;
  }
}
