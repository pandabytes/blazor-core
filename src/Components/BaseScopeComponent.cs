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
  /// Specify a property to be automatically injected a scoped service
  /// during <see cref="OnInitialized"/>.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  protected sealed class InjectScopeAttribute : Attribute {}

  /// <summary>
  /// Specify a property whose type derives from <see cref="BaseJsModule"/>
  /// and automatically import it during <see cref="OnAfterRenderAsync"/>.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  protected sealed class AutoImportJsModuleAttribute : Attribute {}

  /// <inhereitdoc />
  protected override void OnInitialized()
  {
    base.OnInitialized();

    foreach (var property in GetInjectScopeServiceProperties())
    {
      // Inject any scope service here
      var scopeService = ScopedServices.GetRequiredService(property.PropertyType);
      var hasSetter = property.GetSetMethod(nonPublic: true) is not null;

      if (!hasSetter)
      {
        throw new InvalidOperationException(
          $"Cannot provide a scoped value for property '{property.Name}' on type " +
          $"'{property.DeclaringType!.FullName}' because the property has no setter.");
      }
      property.SetValue(this, scopeService);
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
        .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        .Where(property => property.GetCustomAttribute<AutoImportJsModuleAttribute>() is not null)
        .Where(property => typeof(BaseJsModule).IsAssignableFrom(property.PropertyType));

      foreach (var property in autoImportFields)
      {
        if (property.GetValue(this) is not BaseJsModule jsModule)
        {
          throw new InvalidOperationException($"Property \"{property.Name}\" of type " +
                                              $"{type.Name} is null . Please set a value to this property first.");
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

  private IEnumerable<PropertyInfo> GetInjectScopeServiceProperties()
    => GetType()
         .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
         .Where(field => field.GetCustomAttribute<InjectScopeAttribute>() is not null);
}
