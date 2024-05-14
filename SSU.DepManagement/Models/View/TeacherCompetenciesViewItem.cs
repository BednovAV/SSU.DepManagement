namespace Models.View;

public class TeacherCompetenciesViewItem
{
    public long DisciplineId { get; set; }
    
    public string DisciplineName { get; set; }

    public List<FacultyDisciplineViewItem> Faculties { get; set; }

    public bool Checked { get; set; }

    public int Priority { get; set; }
}