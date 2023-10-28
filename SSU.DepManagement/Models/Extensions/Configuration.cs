using Microsoft.Extensions.Configuration;
using Models.ConfigSections;

namespace Models.Extensions;

public static class Configuration
{
    public static T GetSection<T>(this IConfiguration configuration) where T : IConfigSection
        => configuration.GetSection(T.SectionName).Get<T>();
}
