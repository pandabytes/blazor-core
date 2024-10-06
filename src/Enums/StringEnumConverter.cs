using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blazor.Core.Enums;

/// <summary>
/// This converter converts an <see cref="StringEnum"/> object
/// to a literal string object.
/// Borrowed idea from https://watzek.dev/posts/2023/09/16/solving-the-dictionary-json-serialization-puzzle-in-.net/.
/// </summary>
/// <typeparam name="TEnum">String enum type.</typeparam>
public class StringEnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : StringEnum
{
  /// <inheritdoc />
  public override bool HandleNull => false;

  /// <inheritdoc />
  public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    try
    {
      // Value will be guaranteed to be not null because
      // we already specify this converter will not handle
      // null via the property HandleNull
      var value = reader.GetString()!;
      return StringEnum.Get<TEnum>(value);
    }
    catch (ArgumentException ex)
    {
      throw new JsonException($"Failed to parse JSON to \"{typeToConvert.FullName}\" " +
                              "because value doesn't exist.", ex);
    }
  }

  /// <inheritdoc />
  public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.Value);
  }

  /// <inheritdoc />
  public override void WriteAsPropertyName(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
  {
    writer.WritePropertyName(value.Value);
  }

  /// <inheritdoc />
  public override TEnum ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    => Read(ref reader, typeToConvert, options);
}
