using System.Text.Json;
using Blazor.Core.Enums;
using Blazor.Core.Tests.Converters;

namespace Blazor.Core.Tests.Enums;

public class StringEnumConverterTests
{
  private readonly StringEnumConverter<Color> _stringEnumConverter;

  public StringEnumConverterTests()
  {
    _stringEnumConverter = new();
  }

  [Fact]
  public void Write_SerializeStringEnum_ReturnsStringLiteral()
  {
    // Act
    var jsonStr = _stringEnumConverter.Write(Color.Red);

    // Assert
    Assert.Equal($"\"{Color.Red}\"", jsonStr);
  }

  [Fact]
  public void Write_SerializeNullStringEnum_ReturnsNullLiteral()
  {
    // Act
    var jsonStr = _stringEnumConverter.Write(null);

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
    var redColor = _stringEnumConverter.Read("\"red\"");

    // Assert
    Assert.Equal(Color.Red, redColor);
  }

  [Fact]
  public void Read_InvalidStringEnumValue_ThrowsException()
  {
    Assert.Throws<JsonException>(() => _stringEnumConverter.Read("\"cyan\""));
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
}
