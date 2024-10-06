using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blazor.Core.Tests.Converters;

/// <summary>
/// Taken from: https://khalidabuhakmeh.com/systemtextjson-jsonconverter-test-helpers
/// </summary>
public static class JsonConverterTestExtensions
{
  public static TResult? Read<TResult>(
    this JsonConverter<TResult> converter, 
    string token,
    JsonSerializerOptions? options = null
  )
  {
    options ??= JsonSerializerOptions.Default;
    var bytes = Encoding.UTF8.GetBytes(token);
    var reader = new Utf8JsonReader(bytes);
    
    // Advance to token
    reader.Read();
    return converter.Read(ref reader, typeof(TResult), options);
  }

  public static string Write<T>(
    this JsonConverter<T> converter, 
    T value,
    JsonSerializerOptions? options = null
  )
  {
    options ??= JsonSerializerOptions.Default;
    using var memoryStream = new MemoryStream();
    using var writer = new Utf8JsonWriter(memoryStream);

    converter.Write(writer, value, options);
    writer.Flush();
    return Encoding.UTF8.GetString(memoryStream.ToArray());
  }

  public static TResult? ReadAsPropertyName<TResult>(
    this JsonConverter<TResult> converter, 
    string token,
    JsonSerializerOptions? options = null
  )
  {
    options ??= JsonSerializerOptions.Default;
    var bytes = Encoding.UTF8.GetBytes(token);
    var reader = new Utf8JsonReader(bytes);
    
    // Advance to token
    reader.Read();
    return converter.ReadAsPropertyName(ref reader, typeof(TResult), options);
  }

  public static string WriteAsPropertyName<T>(
    this JsonConverter<T> converter, 
    T key,
    string value,
    JsonSerializerOptions? options = null
  ) where T : notnull
  {
    options ??= JsonSerializerOptions.Default;
    using var memoryStream = new MemoryStream();
    using var writer = new Utf8JsonWriter(memoryStream);

    writer.WriteStartObject();
    converter.WriteAsPropertyName(writer, key, options);
    writer.WriteStringValue(value);
    writer.WriteEndObject();

    writer.Flush();
    return Encoding.UTF8.GetString(memoryStream.ToArray());
  }
}
