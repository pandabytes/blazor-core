using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Core;

/// <summary>
/// 
/// </summary>
public static class DependencyInjection
{
  /// <summary>
  /// Register the attach handler for serializing/deserializing
  /// C# callback in Javascript.
  /// </summary>
  public static async Task RegisterAttachReviverAsync(this IServiceProvider serviceProvider)
  {
    var jsRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
    await using var dotNetCallbackModule = new DotNetCallbackJsModule(jsRuntime);
    await dotNetCallbackModule.ImportAsync();
    await dotNetCallbackModule.RegisterAttachReviverAsync();
  }
}
