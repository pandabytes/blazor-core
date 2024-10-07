using System.Text.Json;
using Blazor.Core.Enums;
using Blazor.Core.Tests.Converters;

namespace Blazor.Core.Tests.Enums;

public class StringEnumConverterTests
{
  private readonly StringEnumConverter<Color> _stringEnumConverter;

  private readonly JsonSerializerOptions _jsonOpts;

  public StringEnumConverterTests()
  {
    _stringEnumConverter = new();
    _jsonOpts = new();
    _jsonOpts.Converters.Add(_stringEnumConverter);
  }

  [Fact]
  public void Write_SerializeStringEnum_ReturnsStringLiteral()
  {
    // Act
    var jsonStr = Serialize(Color.Red);

    // Assert
    Assert.Equal($"\"{Color.Red}\"", jsonStr);
  }

  [Fact]
  public void Write_SerializeNullStringEnum_ReturnsNullLiteral()
  {
    // Act
    var jsonStr = Serialize<Color?>(null);

    // Assert
    Assert.Equal("null", jsonStr);
  }

  [Fact]
  public void WritePropertyName_SerializeStringEnumAsKey_ReturnsStringEnumValueAsKey()
  {
    // Act
    var jsonStr = _stringEnumConverter.WriteAsPropertyName(Color.Red, "red-value");

    // Assert
    Assert.Equal("{\"red\":\"red-value\"}", jsonStr);
  }

  [Fact]
  public void Read_ValidStringEnumValue_DeserializesCorrectly()
  {
    // Act
    var redColor = Deserialize<Color>("\"red\"");

    // Assert
    Assert.NotNull(redColor);
    Assert.Equal(Color.Red, redColor);
  }

  [Fact]
  public void Read_InvalidStringEnumValue_ThrowsException()
  {
    Assert.Throws<JsonException>(() => Deserialize<Color>("\"cyan\""));
  }

  [Fact]
  public void Read_NullLiteral_ReturnsNullLiteral()
  {
    // Act
    var jsonStr = Deserialize<Color?>("null");

    // Assert
    Assert.Null(jsonStr);
  }

  [Fact]
  public void ReadAsPropertyName_ValidStringEnumValue_DeserializesCorrectly()
  {
    // Act
    var redColor = _stringEnumConverter.ReadAsPropertyName("\"red\"");

    // Assert
    Assert.Equal(Color.Red, redColor);
  }

  [Fact]
  public void ReadAsPropertyName_InvalidStringEnumValue_ThrowsException()
  {
    Assert.Throws<JsonException>(() => _stringEnumConverter.ReadAsPropertyName("\"cyan\""));
  }

  private string Serialize<TEnum>(TEnum stringEnum)
    => JsonSerializer.Serialize(stringEnum, _jsonOpts);

  private TEnum? Deserialize<TEnum>(string jsonStr)
    => JsonSerializer.Deserialize<TEnum>(jsonStr, _jsonOpts);
}
