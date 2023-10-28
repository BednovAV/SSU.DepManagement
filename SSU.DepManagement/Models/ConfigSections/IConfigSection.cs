namespace Models.ConfigSections;

public interface IConfigSection
{
    static virtual string SectionName { get; }
}
