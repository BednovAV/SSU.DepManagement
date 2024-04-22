namespace Models.View;

public class ApplicationFormViewItem
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public string FacultyName { get; set; }
    
    public DateTimeOffset DateCreated { get; set; }

    public string FileKey { get; set; }

    public IReadOnlyList<AppFormDisciplineViewItem> Disciplines { get; set; }
}