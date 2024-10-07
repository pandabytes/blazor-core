using Blazor.Core.Enums;

namespace Blazor.Core.Tests.Enums;

public class StringEnumTests
{
  [Fact]
  public void AccessStringEnum_HasDuplicateValues_ThrowsException()
  {
    Assert.Throws<TypeInitializationException>(() => Operation.Write);
  }

  [InlineData("red")]
  [InlineData("blue")]
  [InlineData("green")]
  [Theory]
  public void Contains_ContainsValue_ReturnsTrue(string value)
  {
    Assert.True(StringEnum.Contains<Color>(value));
  }

  [Fact]
  public void Contains_DoesNotContainValue_ReturnsFalse()
  {
    Assert.False(StringEnum.Contains<Color>(string.Empty));
  }

  [Fact]
  public void Contains_DoesNotContainValueFromAnotherStringEnum_ReturnsFalse()
  {
    Assert.False(StringEnum.Contains<ComputerColor>(Color.Blue));
  }

  [Fact]
  public void Contains_DuplicateValues_ThrowsException()
  {
    Assert.Throws<InvalidOperationException>(() =>
      StringEnum.Contains<Operation>("read")
    );
  }

  [Fact]
  public void Get_ValidValue_Success()
  {
    Assert.Equal(Color.Blue, StringEnum.Get<Color>("blue"));
  }

  [Fact]
  public void Get_InvalidValue_ThrowsException()
  {
    Assert.Throws<ArgumentException>(() =>
      StringEnum.Get<Color>(string.Empty)
    );
  }

  [Fact]
  public void Get_DuplicateValues_ThrowsException()
  {
    Assert.Throws<InvalidOperationException>(() =>
      StringEnum.Contains<Operation>("write")
    );
  }

  [Fact]
  public void Equals_SameReference_ReturnsTrue()
  {
    var red = Color.Red;
    Assert.True(Color.Red.Equals(red));
  }

  [Fact]
  public void Equals_DifferentColor_ReturnsFalse()
    => Assert.False(Color.Red.Equals(Color.Blue));

  [Fact]
  public void Equals_SameColorButDifferentEnum_ReturnsFalse()
    => Assert.False(Color.Red.Equals(ComputerColor.Red));

  [InlineData("")]
  [InlineData(' ')]
  [InlineData(100)]
  [InlineData(null)]
  [Theory]
  public void Equals_DifferentObjectType_ReturnsFalse(object? obj)
    => Assert.False(Color.Red.Equals(obj));

  [Fact]
  public void GetAllStringEnums_TypeParam_ReturnsAllStringEnums()
  {
    // Act
    var colorEnums = StringEnum.GetAllStringEnums<Color>();

    // Assert
    Assert.Equal(3, colorEnums.Count());
    foreach (var colorEnum in colorEnums)
    {
      Assert.NotNull(colorEnum);
      Assert.NotEmpty(colorEnum.Name);
      Assert.NotEmpty(colorEnum.Value);
    }
  }

  [Fact]
  public void GetAllStringEnums_PassInValidType_ReturnsAllStringEnums()
  {
    var colorEnums = StringEnum.GetAllStringEnums(typeof(Color));
    Assert.Equal(3, colorEnums.Count());

    foreach (var colorEnum in colorEnums)
    {
      Assert.NotNull(colorEnum);
      Assert.NotEmpty(colorEnum.Name);
      Assert.NotEmpty(colorEnum.Value);
    }
  }

  [Fact]
  public void GetAllStringEnums_InvalidType_ThrowsException()
  {
    Assert.Throws<ArgumentException>(() => StringEnum.GetAllStringEnums(typeof(StringEnumConverterTests)));
  }

  [Fact]
  public void NameProperty_AccessViaStaticField_PropertyIsSetCorrectly()
  {
    Assert.Equal(nameof(Color.Blue), Color.Blue.Name);
    Assert.Equal(nameof(ComputerColor.Red), ComputerColor.Red.Name);
  }

  [Fact]
  public void NameProperty_AccessAfterCallingGet_PropertyIsSetCorrectly()
  {
    var blue = StringEnum.Get<Color>("blue");

    Assert.Equal(nameof(Color.Blue), Color.Blue.Name);
    Assert.Equal(nameof(Color.Blue), blue.Name);
  }
}
