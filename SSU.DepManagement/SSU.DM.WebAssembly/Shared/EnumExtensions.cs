using System.ComponentModel;

namespace SSU.DM.WebAssembly.Shared;

public static class EnumExtensions
{
    public static T GetAttribute<T>(this Enum value) where T : Attribute
    {
        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
        return attributes.Length > 0
          ? (T)attributes[0]
          : null;
    }

    public static string GetDescription<T>(this T source) where T : Enum
    {
        var attribute = source.GetAttribute<DescriptionAttribute>();
        return attribute == null ? source.ToString() : attribute.Description;
    }
}
