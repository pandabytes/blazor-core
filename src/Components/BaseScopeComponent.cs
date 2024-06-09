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
  /// Specify a private field to be automatically injected a scoped service
  /// during <see cref="OnInitialized"/>.
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  protected sealed class InjectScopeAttribute : Attribute {}

  /// <summary>
  /// Specify a private field whose type derives from <see cref="BaseJsModule"/>
  /// and automatically import it during <see cref="OnAfterRenderAsync"/>.
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
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
  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    await base.OnAfterRenderAsync(firstRender);

    if (firstRender)
    {
      // Find fields that are marked with AutoImportJsModule
      // and their type must derive from BaseJsModule
      var type = GetType();
      var autoImportFields = type
        .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
        .Where(field => field.GetCustomAttribute<AutoImportJsModuleAttribute>() is not null)
        .Where(field => typeof(BaseJsModule).IsAssignableFrom(field.FieldType));

      foreach (var field in autoImportFields)
      {
        var jsModule = field.GetValue(this) as BaseJsModule;
        if (jsModule is null)
        {
          var fieldName = $"{type.FullName}.{field.Name}";
          throw new InvalidOperationException($"Field \"{fieldName}\" is null. " +
                                               "Please inject a value to this field.");
        }
        await jsModule.ImportAsync();
      }
    }
  }

  /// <inhereitdoc />
  public async ValueTask DisposeAsync()
  {
    await DisposeAsyncCore();
    Dispose(false);
    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Common asynchronous cleanup operation
  /// for subclasses to potentially override.
  /// </summary>
  protected virtual async ValueTask DisposeAsyncCore()
    // See https://github.com/dotnet/aspnetcore/issues/25873#issuecomment-884065550
    => await (ScopedServices as IAsyncDisposable)!.DisposeAsync();

  private IEnumerable<FieldInfo> GetInjectScopeServiceFields()
    => GetType()
         .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
         .Where(field => field.GetCustomAttribute<InjectScopeAttribute>() is not null);
}
