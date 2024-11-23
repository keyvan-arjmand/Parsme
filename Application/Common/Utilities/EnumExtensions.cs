using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Application.Common.Utilities;

public static class EnumExtensions
{
    public static List<EnumValue> GetEnumNameValues<T>()
    {
        List<EnumValue> values = new List<EnumValue>();
        foreach (var itemType in Enum.GetValues(typeof(T)))
        {
            values.Add(new EnumValue()
            {
                Name = Enum.GetName(typeof(T), itemType),
                Value = (int)itemType
            });
        }
        return values;
    }

    public static IEnumerable<T> GetEnumValues<T>(this T input) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException();

        return Enum.GetValues(input.GetType()).Cast<T>();
    }

    public static IEnumerable<T> GetEnumFlags<T>(this T input) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException();

        foreach (var value in Enum.GetValues(input.GetType()))
            if ((input as Enum)!.HasFlag((value as Enum)!))
                yield return (T)value;
    }
    public static string GetDisplayName(this Enum enumValue)
    {
        if (enumValue == null) return string.Empty; // جلوگیری از خطای null reference

        var displayAttribute = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault()
            ?.GetCustomAttribute<DisplayAttribute>();

        return displayAttribute?.Name ?? enumValue.ToString();
    }
    public static string ToDisplay(this Enum value, DisplayProperty property = DisplayProperty.Name)
    {
        var attribute = value.GetType().GetField(value.ToString())
            .GetCustomAttributes<DisplayAttribute>(false).FirstOrDefault();
        if (attribute == null)
            return value.ToString();
        var propValue = attribute.GetType().GetProperty(property.ToString()).GetValue(attribute, null);
        return propValue.ToString();
    }

    public static string ToDescription(this Enum value, DisplayProperty property = DisplayProperty.Description)
    {

        var attribute = value.GetType().GetField(value.ToString())
            .GetCustomAttributes<DescriptionAttribute>(false).FirstOrDefault();

        if (attribute == null)
            return value.ToString();

        var propValue = attribute.GetType().GetProperty(property.ToString()).GetValue(attribute, null);
        return propValue.ToString();
    }

    public static Dictionary<int, string> ToDictionary(this Enum value)
    {
        return Enum.GetValues(value.GetType()).Cast<Enum>().ToDictionary(p => Convert.ToInt32(p), q => q.ToDisplay());
    }
}

public enum DisplayProperty
{
    Description,
    GroupName,
    Name,
    Prompt,
    ShortName,
    Order
}

public class EnumValue
{
    public string? Name { get; set; }
    public int Value { get; set; }
}
