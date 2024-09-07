namespace Blazor.Core.Tests.Components;

public class AfterRenderExecuterTests
{
  [Fact]
  public async Task ExecuteFuncsAsync_AsyncFuncReturnsTrue_NoRetry()
  {
    // Arrange
    var executer = new AfterRenderExecuter();
    executer.Add(() => Task.FromResult(true));

    // Act
    var (executedCount, failedCount) = await executer.ExecuteFuncsAsync();

    // Assert
    Assert.Equal(1, executedCount);
    Assert.Equal(0, failedCount);
  }

  [Fact]
  public async Task ExecuteFuncsAsync_AsyncFuncReturnsFalse_RetryIsExecuted()
  {
    // Arrange
    var successResult = false;
    var executer = new AfterRenderExecuter();
    executer.Add(() =>
    {
      // Flip the success result so that
      // the 2nd time it'll return true
      var result = Task.FromResult(successResult);
      successResult = !successResult;
      return result;
    });

    // Act
    var (executedCount, failedCount) = await executer.ExecuteFuncsAsync();
    var (retryExecutedCount, retryFailedCount) = await executer.ExecuteFuncsAsync();

    // Assert
    Assert.Equal(1, executedCount);
    Assert.Equal(1, failedCount);

    Assert.Equal(1, retryExecutedCount);
    Assert.Equal(0, retryFailedCount);
  }

  [Fact]
  public void ExecuteFuncs_FuncReturnsTrue_NoRetry()
  {
    // Arrange
    var executer = new AfterRenderExecuter();
    executer.Add(() => true);

    // Act
    var (executedCount, failedCount) = executer.ExecuteFuncs();

    // Assert
    Assert.Equal(1, executedCount);
    Assert.Equal(0, failedCount);
  }

  [Fact]
  public void ExecuteFuncs_FuncReturnsFalse_RetryIsExecuted()
  {
    // Arrange
    var successResult = false;
    var executer = new AfterRenderExecuter();
    executer.Add(() =>
    {
      // Flip the success result so that
      // the 2nd time it'll return true
      var result = successResult;
      successResult = !successResult;
      return result;
    });

    // Act
    var (executedCount, failedCount) = executer.ExecuteFuncs();
    var (retryExecutedCount, retryFailedCount) = executer.ExecuteFuncs();

    // Assert
    Assert.Equal(1, executedCount);
    Assert.Equal(1, failedCount);

    Assert.Equal(1, retryExecutedCount);
    Assert.Equal(0, retryFailedCount);
  }

}
