using System.Reflection;

namespace Blazor.Core.Enums;

/// <summary>
/// Represent a string enum.
/// </summary>
public abstract class StringEnum
{
  /// <summary>
  /// Key is a type that inherits <see cref="StringEnum"/>.
  /// Inner key is the string value and inner value is the corresponding 
  /// <see cref="StringEnum"/> object.
  /// </summary>
  private static readonly IDictionary<Type, IDictionary<string, StringEnum>> StringEnumValuesMapping =
    new Dictionary<Type, IDictionary<string, StringEnum>>();

  private string _name;

  /// <summary>
  /// The enum value.
  /// </summary>
  public string Value { get; }

  /// <summary>
  /// Name of the string enum. For example, if the
  /// string enum value is defined like this:
  /// `public readonly static Color Red = new("red");`
  /// Then <see cref="Name"/> is "Red" and <see cref="Value"/>
  /// is "red".
  /// </summary>
  public string Name
  {
    get
    {
      if (string.IsNullOrWhiteSpace(_name))
      {
        InitializeAllStringEnums(GetType());
      }
      return _name;
    }
    private set => _name = value;
  }

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="value">Enum value.</param>
  protected StringEnum(string value)
  {
    var type = GetType();

    if (!StringEnumValuesMapping.ContainsKey(type))
    {
      StringEnumValuesMapping.Add(type, new Dictionary<string, StringEnum>());
    }

    if (StringEnumValuesMapping[type].ContainsKey(value))
    {
      throw new ArgumentException($"String enum \"{type.FullName}\" has duplicate value \"{value}\".");
    }
  
    StringEnumValuesMapping[type].Add(value, this);
    Value = value;

    // Temporarily set to empty string.
    // Actual value will be set later
    _name = string.Empty;
  }

  /// <inheritdoc/>
  public override string ToString() => Value;

  /// <inheritdoc/>
  public override bool Equals(object? obj)
  {
    if (ReferenceEquals(this, obj))
    {
      return true;
    }

    if (obj is null || obj.GetType() != GetType())
    {
      return false;
    }

    return ((StringEnum)obj).Value == Value;
  }

  /// <inheritdoc/>
  public override int GetHashCode() => Value.GetHashCode();

  /// <summary>
  /// Determines whether the <paramref name="value"/> is valid
  /// string enum value of <typeparamref name="TStringEnum"/>.
  /// </summary>
  /// <typeparam name="TStringEnum">
  /// Type that inherits <see cref="StringEnum"/>.
  /// </typeparam>
  /// <param name="value">String value to check for.</param>
  /// <exception cref="InvalidOperationException">
  /// Thrown when <typeparamref name="TStringEnum"/> is
  /// unable to initialized all of its string enums.
  /// </exception>
  /// <returns>
  /// True if <paramref name="value"/> is a string enum value
  /// of <typeparamref name="TStringEnum"/>. False otherwise.
  /// </returns>
  public static bool Contains<TStringEnum>(string value) where TStringEnum : StringEnum
  {
    var type = typeof(TStringEnum);
    InitializeAllStringEnums(type);
    return StringEnumValuesMapping[type].ContainsKey(value);
  }

  /// <summary>
  /// Get the corresponding <typeparamref name="TStringEnum"/> given
  /// <paramref name="value"/>.
  /// </summary>
  /// <typeparam name="TStringEnum">
  /// Type that inherits <see cref="StringEnum"/>.
  /// </typeparam>
  /// <param name="value">String value.</param>
  /// <returns>The corresponding <typeparamref name="TStringEnum"/> object.</returns>
  /// <exception cref="ArgumentException">
  /// Thrown when <paramref name="value"/> is not a string enum value
  /// of <typeparamref name="TStringEnum"/>.
  /// </exception>
  /// <exception cref="InvalidOperationException">
  /// Thrown when <typeparamref name="TStringEnum"/> is
  /// unable to initialized all of its string enums.
  /// </exception>
  public static TStringEnum Get<TStringEnum>(string value) where TStringEnum : StringEnum
  {
    var type = typeof(TStringEnum);
    InitializeAllStringEnums(type);

    if (Contains<TStringEnum>(value))
    {
      return (TStringEnum)StringEnumValuesMapping[type][value];
    }

    throw new ArgumentException($"Value \"{value}\" not found in string enum \"{type.FullName}\".");
  }

  /// <summary>
  /// Get all <see cref="StringEnum"/> objects belonging
  /// to <typeparamref name="TStringEnum"/>.
  /// </summary>
  /// <typeparam name="TStringEnum">
  /// String enum that we want to get values from.
  /// </typeparam>
  /// <returns><see cref="StringEnum"/> objects.</returns>
  public static IEnumerable<TStringEnum> GetAllStringEnums<TStringEnum>() where TStringEnum : StringEnum
    => GetAllStringEnums(typeof(TStringEnum)).OfType<TStringEnum>();

  /// <summary>
  /// Get all <see cref="StringEnum"/> objects belonging
  /// to <paramref name="type"/>.
  /// </summary>
  /// <param name="type">String enum type.</param>
  /// <returns><see cref="StringEnum"/> objects.</returns>
  /// <exception cref="ArgumentException">
  /// Thrown when <paramref name="type"/> does not inheri
  /// <see cref="StringEnum"/>.
  /// </exception>
  public static IEnumerable<StringEnum> GetAllStringEnums(Type type)
  {
    if (!type.IsAssignableTo(typeof(StringEnum)))
    {
      throw new ArgumentException($"Type \"{type.FullName}\" does not inherit {nameof(StringEnum)}.");
    }
    InitializeAllStringEnums(type);
    return StringEnumValuesMapping[type].Values;
  }

  /// <summary>
  /// Implicitly convert <paramref name="stringEnum"/> to string.
  /// </summary>
  /// <param name="stringEnum">String enum object.</param>
  public static implicit operator string(StringEnum stringEnum) => stringEnum.Value;

  /// <summary>
  /// Compare 2 string enums.
  /// </summary>
  /// <param name="stringEnum1"></param>
  /// <param name="stringEnum2"></param>
  /// <returns>True if equal, false otherwise.</returns>
  public static bool operator ==(StringEnum stringEnum1, StringEnum stringEnum2)
    => stringEnum1.Equals(stringEnum2);

  /// <summary>
  /// The invert of "==".
  /// </summary>
  /// <param name="stringEnum1"></param>
  /// <param name="stringEnum2"></param>
  /// <returns>True if not equal, false otherwise.</returns>
  public static bool operator !=(StringEnum stringEnum1, StringEnum stringEnum2)
    => !(stringEnum1.Value == stringEnum2.Value);

  /// <summary>
  /// This will call the constructor of string enum
  /// to initialize them all. This method can be called
  /// multiple times without affecting anything. 
  /// </summary>
  private static void InitializeAllStringEnums(Type type)
  {
    var stringEnumType = typeof(StringEnum);
    try
    {
      var fields = GetStringEnumStaticFields(type);
      var fieldWithStringEnumArray = fields
        // This is where we call the instance constructor
        .Select(field => (field, (StringEnum)field.GetValue(null)!))
        .ToArray();

      // After we have initialized the string
      // enum objects we then set their Name
      foreach (var (field, stringEnum) in fieldWithStringEnumArray)
      {
        stringEnum.Name = field.Name;
      }
    }
    catch (TargetInvocationException ex)
    {
      // Throw the 2nd inner exception because this means
      // we fail to initialize the enum string,
      // and so we want to surface this exception
      // directly to client
      throw new InvalidOperationException("Failed to initilize all string enums.", ex.InnerException ?? ex);
    }
  }

  private static IEnumerable<FieldInfo> GetStringEnumStaticFields(Type type)
  {
    var stringEnumType = typeof(StringEnum);
    return type
      .GetFields(BindingFlags.Public | BindingFlags.Static)
      .Where(field => field.IsInitOnly)
      .Where(field => field.FieldType == type)
      .Where(field => stringEnumType.IsAssignableFrom(field.FieldType));
  }
}
