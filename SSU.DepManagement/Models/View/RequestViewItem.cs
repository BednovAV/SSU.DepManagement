namespace Models.View;

public class RequestViewItem
{
    public int Id { get; set; }
    
    public string NameDiscipline { get; set; }

    public string Direction { get; set; }

    public int Semester { get; set; }
    
    public int BudgetCount { get; set; }
    
    public int CommercialCount { get; set; }
    
    public string GroupNumber { get; set; }

    public string GroupForm { get; set; }

    public int TotalHours { get; set; }

    public int LectureHours { get; set; }

    public int PracticalHours { get; set; }

    public int LaboratoryHours { get; set; }

    public int IndependentWorkHours { get; set; }

    public string Reporting { get; set; }

    public string Note { get; set; }

    public TeacherViewItem Teacher { get; set; }

    public ISet<long> AvailableTeacherIds { get; set; }
}