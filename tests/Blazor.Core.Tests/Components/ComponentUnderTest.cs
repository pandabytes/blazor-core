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
  private ScopeServiceDispose ScopeServiceDispose { get; set; } = null!;

  [InjectScope]
  private ScopeServiceDisposeAsync ScopeServiceDisposeAsync { get; set; } = null!;

  [InjectScope]
  public ScopeServiceDisposeAndDisposeAsync ScopeServiceDisposeAndDisposeAsync { get; set; } = null!;
}

public class BaseScopeComponentWithJsModule : BaseScopeComponent
{
  [InjectScope]
  private JsModule JsModule { get; set; } = null!;
}

public class BaseScopeComponentWithAutoImport : BaseScopeComponent
{
  [InjectScope, AutoImportJsModule]
  private JsModule JsModuleAutoImport { get; set; } = null!;
}

public class BaseScopeComponentWithJsModuleNotInject : BaseScopeComponent
{
  [AutoImportJsModule]
  private JsModule JsModule { get; } = null!;
}

public class TestComponent : BaseScopeComponent
{
  [InjectScope]
  public ScopeServiceDispose ScopeService { get; set; } = null!;
}

public class ChildTestComponent : TestComponent {}

#pragma warning restore CS0414 // Remove unused private members
