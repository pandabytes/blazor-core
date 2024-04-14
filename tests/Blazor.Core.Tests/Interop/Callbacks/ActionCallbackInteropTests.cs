namespace Blazor.Core.Tests.Interop.Callbacks;

public class ActionCallbackInteropTests : BaseCallbackInteropTests
{
  [Fact]
  public void Action_CanBeInvokedInJs()
  {
    // Arrange
    Action action = () => {};

    // Act
    using ActionCallbackInterop actionCallbackInterop = action;

    // Assert
    Assert.False(actionCallbackInterop.IsAsync);
    AssertCallbackInterop(actionCallbackInterop, action);
  }

  [Fact]
  public void ActionWithOneParam_CanBeInvokedInJs()
  {
    // Arrange
    Action<string> action = (s) => {};

    // Act
    using ActionCallbackInterop<string> actionCallbackInterop = action;

    // Assert
    Assert.False(actionCallbackInterop.IsAsync);
    AssertCallbackInterop(actionCallbackInterop, action);
  }

  [Fact]
  public void ActionWithTwoParams_CanBeInvokedInJs()
  {
    // Arrange
    Action<string, int> action = (s, i) => {};

    // Act
    using ActionCallbackInterop<string, int> actionCallbackInterop = action;

    // Assert
    Assert.False(actionCallbackInterop.IsAsync);
    AssertCallbackInterop(actionCallbackInterop, action);
  }

}
