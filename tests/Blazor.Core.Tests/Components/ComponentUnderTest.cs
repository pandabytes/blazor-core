using Microsoft.JSInterop;

namespace Blazor.Core.Tests.Components;

public class JsModule : BaseJsModule
{
  protected override string ModulePath => "foo.js";

  public JsModule(IJSRuntime jSRuntime) : base(jSRuntime)
  {}
}

#pragma warning disable CS0414 // Remove unused private members  

public class BaseScopeComponentUnderTest : BaseScopeComponent
{

  [InjectScope]
  private readonly ScopeServiceDispose _scopeServiceDispose = null!;

  [InjectScope]
  private readonly ScopeServiceDisposeAsync _scopeServiceDisposeAsync = null!;

  [InjectScope]
  private readonly ScopeServiceDisposeAndDisposeAsync _scopeServiceDisposeAndDisposeAsync = null!;
}

public class BaseScopeComponentWithJsModule : BaseScopeComponent
{
  [InjectScope]
  private readonly JsModule _jsModule = null!;
}

public class BaseScopeComponentWithAutoImport : BaseScopeComponent
{
  [InjectScope, AutoImportJsModule]
  private readonly JsModule _jsModuleAutoImport = null!;
}

public class BaseScopeComponentWithJsModuleNotInject : BaseScopeComponent
{
  [AutoImportJsModule]
  private readonly JsModule _jsModule = null!;
}

public class TestComponent : BaseScopeComponent
{
  [InjectScope]
  private protected readonly ScopeServiceDispose _scopeService = null!;

  public ScopeServiceDispose? ScopeService => _scopeService;
}

public class ChildTestComponent : TestComponent {}

#pragma warning restore CS0414 // Remove unused private members
