using Microsoft.AspNetCore.Components;

namespace Blazor.Core.Tests.Interop.Callbacks;

public class EventCallbackInteropTests : BaseCallbackInteropTests
{
  [Fact]
  public void EventCallback_CanBeInvokedInJs()
  {
    // Act
    using EventCallbackInterop eventCallbackInterop = EventCallback.Empty;

    // Assert
    Assert.True(eventCallbackInterop.IsAsync);
    AssertCallbackInterop(eventCallbackInterop, EventCallback.Empty);
  }

  [Fact]
  public void EventCallbackWithReturnType_CanBeInvokedInJs()
  {
    // Arrange
    var eventCallback = new EventCallback<int>();

    // Act
    using EventCallbackInterop<int> eventCallbackInterop = eventCallback;

    // Assert
    Assert.True(eventCallbackInterop.IsAsync);
    AssertCallbackInterop(eventCallbackInterop, eventCallback);
  }
}
