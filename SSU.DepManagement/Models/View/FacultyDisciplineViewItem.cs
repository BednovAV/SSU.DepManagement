namespace Models.View;

public class FacultyDisciplineViewItem// : IEqualityComparer<FacultyDisciplineViewItem>
{
    public CompetenceShortInfo Competence { get; set; }

    public string FacultyName { get; set; }

    public bool Checked { get; set; }
}