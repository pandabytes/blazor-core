using Blazor.Core.Enums;

namespace Blazor.Core.Tests.Enums;

public class Color : StringEnum
{
  private Color(string value) : base(value) {}

  public readonly static Color Blue = new("blue");

  public readonly static Color Green = new("green");

  public readonly static Color Red = new("red");
}

public class ComputerColor : StringEnum
{
  private ComputerColor(string value) : base(value) {}

  public static readonly ComputerColor Red = new("red");
}

public class Operation : StringEnum
{
  private Operation(string value) : base(value) {}

  public readonly static Operation Read = new("read");

  public readonly static Operation DuplicateRead = new("read");

  public readonly static Operation Write = new("write");
}
