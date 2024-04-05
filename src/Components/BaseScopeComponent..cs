using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Core.Components;

/// <summary>
/// Base class for scope components.
/// This class provides functionality to
/// inject scope services via <see cref="InjectScopeAttribute"/>
/// and auto import JS module via <see cref="AutoImportJsModuleAttribute"/>.
/// </summary>
public abstract class BaseScopeComponent : OwningComponentBase, IAsyncDisposable
{
  /// <summary>
  /// Specify a field to be automatically injected a scoped service
  /// during <see cref="OnInitialized"/>.
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  protected sealed class InjectScopeAttribute : Attribute {}

  /// <summary>
  /// Specify a field whose type derives from <see cref="BaseJsModule"/>
  /// and automatically load it during <see cref="OnInitializedAsync"/>.
  /// This attribute is only valid on fields marked with <see cref="InjectScopeAttribute"/>.
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  protected sealed class AutoImportJsModuleAttribute : Attribute {}

  /// <inhereitdoc />
  protected override void OnInitialized()
  {
    base.OnInitialized();

    foreach (var field in GetInjectScopeServiceFields())
    {
      // Inject any scope service here
      var scopeService = ScopedServices.GetRequiredService(field.FieldType);
      field.SetValue(this, scopeService);
    }
  }

  /// <inhereitdoc />
  protected override async Task OnInitializedAsync()
  {
    await base.OnInitializedAsync();

    // Find fields that are marked with AutoImportJsModule
    // and their type must derive from BaseJsModule
    var autoImportFields = GetInjectScopeServiceFields()
      .Where(field => field.GetCustomAttribute<AutoImportJsModuleAttribute>() is not null)
      .Where(field => typeof(BaseJsModule).IsAssignableFrom(field.FieldType));

    foreach (var field in autoImportFields)
    {
      var jsModule = (BaseJsModule)field.GetValue(this)!;
      await jsModule.ImportAsync();
    }
  }

  /// <inhereitdoc />
  async ValueTask IAsyncDisposable.DisposeAsync()
  {
    // See https://github.com/dotnet/aspnetcore/issues/25873#issuecomment-884065550
    await (ScopedServices as IAsyncDisposable)!.DisposeAsync();
    GC.SuppressFinalize(this);
  }

  private IEnumerable<FieldInfo> GetInjectScopeServiceFields()
    => GetType()
         .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
         .Where(field => field.GetCustomAttribute<InjectScopeAttribute>() is not null);
}