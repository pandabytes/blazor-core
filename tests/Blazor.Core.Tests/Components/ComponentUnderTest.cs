using Microsoft.JSInterop;

namespace Blazor.Core.Tests.Components;

public class JsModule : BaseJsModule
{
  protected override string ModulePath => "foo.js";

  public JsModule(IJSRuntime jSRuntime) : base(jSRuntime)
  {}
}

public class BaseScopeComponentUnderTest : BaseScopeComponent
{
#pragma warning disable CS0414 // Remove unused private members  
  [InjectScope]
  private readonly ScopeServiceDispose _scopeServiceDispose = null!;

  [InjectScope]
  private readonly ScopeServiceDisposeAsync _scopeServiceDisposeAsync = null!;

  [InjectScope]
  private readonly ScopeServiceDisposeAndDisposeAsync _scopeServiceDisposeAndDisposeAsync = null!;
#pragma warning restore CS0414 // Remove unused private members
}

public class BaseScopeComponentWithJsModule : BaseScopeComponent
{
#pragma warning disable CS0414 // Remove unused private members  
  [InjectScope, AutoImportJsModule]
  private readonly JsModule _jsModule = null!;
#pragma warning restore CS0414 // Remove unused private members
}
