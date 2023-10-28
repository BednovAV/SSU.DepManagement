using System.ComponentModel;

namespace Models.Request;

public class RequestItem
{
    public string NameDiscipline { get; init; }

    public string Direction { get; init; }

    public int Semester { get; init; }
    
    public int BudgetCount { get; init; }
    
    public int CommercialCount { get; init; }
    
    public string GroupNumber { get; init; }

    public string GroupForm { get; init; }

    public int TotalHours { get; init; }

    public int LectureHours { get; init; }

    public int PracticalHours { get; init; }

    public int LaboratoryHours { get; init; }

    public int IndependentWorkHours { get; init; }

    public ReportingForm Reporting { get; init; }

    public string Note { get; init; }
}

public enum ReportingForm
{
    [Description("экз")]
    Exam,
    [Description("зач")]
    Test
}
