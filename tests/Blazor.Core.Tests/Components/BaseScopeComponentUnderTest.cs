using Microsoft.JSInterop;

namespace Blazor.Core.Tests.Components;

public class BaseScopeComponentTests : TestContext
{
  [Fact]
  public void BaseScopeComponent_InjectScopeServices_ScopeServicesAreDisposed()
  {
    // Arrange
    var scopeServiceDispose = new ScopeServiceDispose();
    var scopeServiceDisposeAsync = new ScopeServiceDisposeAsync();
    var scopeServiceDisposeAndDisposeAsync  = new ScopeServiceDisposeAndDisposeAsync();
    Services.AddScoped(_ => scopeServiceDispose);
    Services.AddScoped(_ => scopeServiceDisposeAsync);
    Services.AddScoped(_ => scopeServiceDisposeAndDisposeAsync);

    // Act
    RenderComponent<BaseScopeComponentUnderTest>();
    DisposeComponents();

    // Assert
    Assert.True(scopeServiceDispose.IsDisposed);
    Assert.True(scopeServiceDispose.IsDisposeCalled);
    Assert.False(scopeServiceDispose.IsDisposeAsyncCalled);
    
    Assert.True(scopeServiceDisposeAsync.IsDisposed);
    Assert.True(scopeServiceDisposeAsync.IsDisposeAsyncCalled);
    Assert.False(scopeServiceDisposeAsync.IsDisposeCalled);

    Assert.True(scopeServiceDisposeAndDisposeAsync.IsDisposed);
    Assert.True(scopeServiceDisposeAndDisposeAsync.IsDisposeAsyncCalled);
    Assert.False(scopeServiceDisposeAndDisposeAsync.IsDisposeCalled);
  }

  [Fact]
  public void BaseScopeComponent_UseAutoImport_JsModuleIsImported()
  {
    // Arrange
    JsModule? jsModule = null;
    Services.AddScoped(sp =>
    {
      var jsRuntime = sp.GetRequiredService<IJSRuntime>();
      jsModule = new JsModule(jsRuntime);
      return jsModule;
    });

    JSInterop.SetupModule("foo.js");

    // Act
    RenderComponent<BaseScopeComponentWithAutoImport>();

    // Assert
    Assert.NotNull(jsModule);
    Assert.Equal(JsModuleStatus.Imported, jsModule.ModuleStatus);
    
    DisposeComponents();
    Assert.Equal(JsModuleStatus.Disposed, jsModule.ModuleStatus);
  }

  [Fact]
  public void BaseScopeComponent_NotUseAutoImport_JsModuleIsNotImported()
  {
    // Arrange
    JsModule? jsModule = null;
    Services.AddScoped(sp =>
    {
      var jsRuntime = sp.GetRequiredService<IJSRuntime>();
      jsModule = new JsModule(jsRuntime);
      return jsModule;
    });

    JSInterop.SetupModule("foo.js");

    // Act
    RenderComponent<BaseScopeComponentWithJsModule>();

    // Assert
    Assert.NotNull(jsModule);
    Assert.Equal(JsModuleStatus.NotImported, jsModule.ModuleStatus);
    
    DisposeComponents();
    Assert.Equal(JsModuleStatus.Disposed, jsModule.ModuleStatus);
  }

  [Fact]
  public void BaseScopeComponent_AutoImportFieldNotSet_ThrowsException()
  {
    // Arrange
    Services.AddScoped(sp =>
    {
      var jsRuntime = sp.GetRequiredService<IJSRuntime>();
      return new JsModule(jsRuntime);
    });

    JSInterop.SetupModule("foo.js");

    // Act & Assert
    Assert.Throws<InvalidOperationException>(()
      => RenderComponent<BaseScopeComponentWithJsModuleNotInject>());
  }
}
