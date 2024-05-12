using System.ComponentModel;

namespace Models.Request;

public class ParsedRequest
{
    public string NameDiscipline { get; init; }

    public string Direction { get; init; }

    public string Semester { get; init; }
    
    public string BudgetCount { get; init; }
    
    public string CommercialCount { get; init; }
    
    public string GroupNumber { get; init; }

    public string GroupForm { get; init; }

    public int TotalHours { get; init; }

    public int LectureHours { get; init; }

    public int PracticalHours { get; init; }

    public int LaboratoryHours { get; init; }

    public string IndependentWorkHours { get; init; }

    public string Reporting { get; init; }

    public string Note { get; init; }

    public StudyForm StudyForm { get; set; }
}

public enum ReportingForm
{
    [Description("экз")]
    Exam,
    [Description("зач")]
    Test,
    [Description("")]
    None
}

public enum StudyForm
{
    [Description("Очная форма")]
    FullTime,
    [Description("Очно-заочная (вечерняя) форма")]
    Extramural,
    [Description("Заочная форма")]
    PartTime
}

public enum LessonForm
{
    Lecture,
    Practical,
    Laboratory,
    Gac,
    Gec,
    CourseWork,
    Nir,
    Vcr,
    MasterManagement,
    ComputingPractice,
    TechnologicalPractice,
    ProductionPractice,
    PreGraduatePractice,
    PedagogicalProductionPractice,
    PedagogicalGraduatePractice,
    ResearchPractice,
    ProductionPedagogicalPractice,
    Other,
}
