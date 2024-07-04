using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.JSInterop;

namespace Blazor.Core.Tests.Interop;

public class IJSRuntimeExtensionsTests : TestContext
{
  public class JsRunTimeTest : IJSRuntime
  {
    private JsonSerializerOptions JsonSerializerOptions { get; set; } = new();

    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, object?[]? args)
    {
        throw new NotImplementedException();
    }

    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
    {
        throw new NotImplementedException();
    }
  }

  [Fact]
  public void GetJsonSerializerOptions_JsonSerializerOptionsNotFound_ThrowsException()
  {
    // Arrange
    var jsRuntime = Services.GetRequiredService<IJSRuntime>();
    
    // Act & Assert
    Assert.Throws<ArgumentException>(() => jsRuntime.GetJsonSerializerOptions());
  }

  [Fact]
  public void GetJsonSerializerOptions_JsonSerializerOptionsFound_NotNull()
  {
    // Arrange
    IJSRuntime jsRuntime = new JsRunTimeTest();

    // Act
    var jsonSerializerOpts = jsRuntime.GetJsonSerializerOptions();

    // Assert
    Assert.NotNull(jsonSerializerOpts);
  }
}
