namespace Models.Request;

public class RequestSaveItem
{
    public long DisciplineId { get; set; }

    public string Direction { get; init; }

    public int Semester { get; init; }

    public int BudgetCount { get; init; }

    public int CommercialCount { get; init; }

    public string GroupNumber { get; init; }

    public string GroupForm { get; init; }

    public int LessonHours { get; init; }
    
    public double? ControlOfIndependentWorkHours { get; set; }
    
    public double? PreExamConsultationHours { get; set; }

    public double? ExamHours { get; set; }
    
    public double? CheckingTestPaperHours { get; set; }

    public double TotalHours { get; init; }
    
    public int IndependentWorkHours { get; set; }

    public LessonForm LessonForm { get; init; }
    
    public ReportingForm Reporting { get; init; }

    public string Note { get; init; }
}