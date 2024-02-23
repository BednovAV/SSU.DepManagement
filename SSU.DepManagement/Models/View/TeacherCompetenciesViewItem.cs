namespace Models.View;

public class TeacherCompetenciesViewItem
{
    public string DisciplineName { get; set; }

    public List<FacultyDisciplineViewItem> Faculties { get; set; }

    public bool Checked { get; set; }
}