using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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
  public static async Task RegisterCallbackReviverAsync(this WebAssemblyHost app)
  {
    var serviceProvider = app.Services;
    var jsRuntime = serviceProvider.GetRequiredService<IJSRuntime>();

    await using var callbackReviverModule = new CallbackReviverJsModule(jsRuntime);
    await callbackReviverModule.ImportAsync();
    await callbackReviverModule.RegisterReviverAsync();
  }

  /// <summary>
  /// Configure the <see cref="IJSRuntime"/>'s <see cref="JsonSerializerOptions"/>
  /// This will affect the Blazor application globally.
  /// </summary>
  public static WebAssemblyHost ConfigureIJSRuntimeJsonOptions(this WebAssemblyHost webHost, Action<JsonSerializerOptions> configureJsonOpts)
  {
    var jsRuntime = webHost.Services.GetRequiredService<IJSRuntime>();
    var options = GetJsonSerializerOptions(jsRuntime);
    configureJsonOpts(options);
    return webHost;
  }

  /// <summary>
  /// See https://github.com/dotnet/aspnetcore/issues/12685#issuecomment-603050776
  /// </summary>
  /// <param name="jsRuntime"></param>
  /// <exception cref="ArgumentException"></exception>
  private static JsonSerializerOptions GetJsonSerializerOptions(IJSRuntime jsRuntime)
  {
    var property = typeof(JSRuntime).GetProperty("JsonSerializerOptions", BindingFlags.NonPublic | BindingFlags.Instance);
    if (property?.GetValue(jsRuntime, null) is not JsonSerializerOptions options)
    {
      throw new ArgumentException($"Unable to get {nameof(JsonSerializerOptions)} from {nameof(IJSRuntime)}.");
    }

    return options;
  }
}
