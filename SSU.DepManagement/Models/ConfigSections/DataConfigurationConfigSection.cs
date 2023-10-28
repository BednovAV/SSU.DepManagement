namespace Models.ConfigSections;

public class DataConfigurationConfigSection : IConfigSection
{
    public static string SectionName => "DataConfiguration";

    public string SelectedConnection { get; set; }
}
