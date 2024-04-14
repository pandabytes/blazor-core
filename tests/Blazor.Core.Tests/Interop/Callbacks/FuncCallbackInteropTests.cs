namespace Blazor.Core.Tests.Interop.Callbacks;

public class FuncCallbackInteropTests : BaseCallbackInteropTests
{
  [Fact]
  public void Func_CanBeInvokedInJs()
  {
    // Arrange
    Func<int> func = () => 0;

    // Act
    using FuncCallbackInterop<int> funcCallbackInterop = func;

    // Assert
    Assert.False(funcCallbackInterop.IsAsync);
    AssertCallbackInterop(funcCallbackInterop, func);
  }

  [Fact]
  public void FuncWithOneParam_CanBeInvokedInJs()
  {
    // Arrange
    Func<int, string> func = (s) => string.Empty;

    // Act
    using FuncCallbackInterop<int, string> funcCallbackInterop = func;

    // Assert
    Assert.False(funcCallbackInterop.IsAsync);
    AssertCallbackInterop(funcCallbackInterop, func);
  }

  [Fact]
  public void FuncWithTwoParams_CanBeInvokedInJs()
  {
    // Arrange
    Func<int, object, string> func = (s, o) => string.Empty;

    // Act
    using FuncCallbackInterop<int, object, string> funcCallbackInterop = func;

    // Assert
    Assert.False(funcCallbackInterop.IsAsync);
    AssertCallbackInterop(funcCallbackInterop, func);
  }

  [Fact]
  public void Func_ReturnsTask_IsAsyncSetToTrue()
  {
    // Arrange
    Func<Task<int>> func1 = () => Task.FromResult(0);
    Func<Task> func2 = () => Task.CompletedTask;

    // Act
    using FuncCallbackInterop<Task<int>> funcCallbackInterop1 = func1;
    using FuncCallbackInterop<Task> funcCallbackInterop2 = func2;

    // Assert
    Assert.True(funcCallbackInterop1.IsAsync);
    Assert.True(funcCallbackInterop2.IsAsync);
    AssertCallbackInterop(funcCallbackInterop1, func1);
    AssertCallbackInterop(funcCallbackInterop2, func2);
  }
}
