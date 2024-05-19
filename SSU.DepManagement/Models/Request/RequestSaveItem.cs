namespace Models.Request;

public class RequestSaveItem
{
    public long DisciplineId { get; set; }

    public List<string> Direction { get; init; }

    public List<int> Semester { get; init; }

    public List<int> BudgetCount { get; init; }

    public List<int> CommercialCount { get; init; }

    public int StudentsCount => BudgetCount.Sum()/* + CommercialCount.Sum()*/;

    public List<int> GroupNumber { get; set; }

    public int? SubgroupNumber { get; set; }

    public string GroupForm { get; init; }

    public int LessonHours { get; set; }
    
    public List<double?> TestHours { get; set; }
    
    public List<double?> ExamHours { get; set; }
    
    public List<double> CheckingTestPaperHours { get; set; }
    
    public double AspirantManagement { get; set; }
    
    public double ApplicantManagement { get; set; }
    
    public double ExtracurricularActivity { get; set; }
    
    public double MasterManagement { get; set; }
    
    public List<int> IndependentWorkHours { get; set; }
    
    public List<double> ControlOfIndependentWork { get; set; }
    
    public List<double?> PreExamConsultation { get; set; }
    
    public double PracticeManagement { get; set; }
    
    public double CourseWork { get; set; }
    
    public double DiplomaWork { get; set; }
    
    public double Gac { get; set; }
    
    public int Other { get; set; }

    public LessonForm LessonForm { get; set; }
    
    public List<ReportingForm> Reporting { get; set; }

    public string Note { get; set; }
    
    public StudyForm StudyForm { get; set; }
}