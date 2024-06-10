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

  [Fact]
  public void ActionWithThreeParams_CanBeInvokedInJs()
  {
    // Arrange
    Action<string, int, float> action = (s, i, f) => {};

    // Act
    using ActionCallbackInterop<string, int, float> actionCallbackInterop = action;

    // Assert
    Assert.False(actionCallbackInterop.IsAsync);
    AssertCallbackInterop(actionCallbackInterop, action);
  }

  [Fact]
  public void ActionWithFourParams_CanBeInvokedInJs()
  {
    // Arrange
    Action<string, int, float, object> action = (s, i, f, o) => {};

    // Act
    using ActionCallbackInterop<string, int, float, object> actionCallbackInterop = action;

    // Assert
    Assert.False(actionCallbackInterop.IsAsync);
    AssertCallbackInterop(actionCallbackInterop, action);
  }

  [Fact]
  public void ActionWithFiveParams_CanBeInvokedInJs()
  {
    // Arrange
    Action<string, int, float, object, double> action = (s, i, f, o, d) => {};

    // Act
    using ActionCallbackInterop<string, int, float, object, double> actionCallbackInterop = action;

    // Assert
    Assert.False(actionCallbackInterop.IsAsync);
    AssertCallbackInterop(actionCallbackInterop, action);
  }
}
