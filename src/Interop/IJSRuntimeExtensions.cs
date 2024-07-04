using System.Reflection;
using System.Text.Json;

namespace Blazor.Core.Interop;

/// <summary>
/// Extension methods for <see cref="IJSRuntime"/>.
/// </summary>
public static class IJSRuntimeExtensions
{
  /// <summary>
  /// Get the <see cref="JsonSerializerOptions"/> from <paramref name="jsRuntime"/>.
  /// The serializer options is not exposed publicly, hence this method
  /// uses reflection to retrieve it. For more context, see
  /// https://github.com/dotnet/aspnetcore/issues/12685#issuecomment-603050776.
  /// </summary>
  /// <param name="jsRuntime"></param>
  /// <exception cref="ArgumentException"></exception>
  /// <returns><see cref="JsonSerializerOptions"/> object.</returns>
  public static JsonSerializerOptions GetJsonSerializerOptions(this IJSRuntime jsRuntime)
  {
    var property = jsRuntime
      .GetType()
      .GetProperty("JsonSerializerOptions", BindingFlags.NonPublic | BindingFlags.Instance);

    if (property?.GetValue(jsRuntime, null) is not JsonSerializerOptions options)
    {
      throw new ArgumentException($"Unable to get {nameof(JsonSerializerOptions)} from {nameof(IJSRuntime)}.");
    }

    return options;
  }
}
