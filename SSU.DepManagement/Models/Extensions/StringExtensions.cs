using System.ComponentModel;

namespace Models.Extensions;

public static class StringExtensions
{
    public static List<int> SplitAndParseToInt(this string source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return new List<int>();
        }
        return source.Split(',', StringSplitOptions.TrimEntries).Select(int.Parse).ToList();
    }
    
    public static List<string> SplitAndTrim(this string source)
    {
        return source.Split(',', StringSplitOptions.TrimEntries).ToList();
    }
    
    public static List<TEnum> SplitAndParseToEnum<TEnum>(this string source) where TEnum : struct
    {
        return source.Split(',', StringSplitOptions.TrimEntries).Select(GetEnumValueFromDescription<TEnum>).ToList();
    }
    
    public static T GetEnumValueFromDescription<T>(this string description)
    {
        var fields = typeof(T).GetFields();
        foreach (var fieldInfo in fields)
        {
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes is { Length: > 0 } && attributes[0].Description == description)
                return (T)Enum.Parse(typeof(T), fieldInfo.Name);
        }
        return default!;
    }
}