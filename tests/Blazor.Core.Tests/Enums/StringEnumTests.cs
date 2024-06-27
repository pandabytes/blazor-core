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
}
