using System.Security.AccessControl;

namespace Models.Request;

public class CalculationOfHoursData
{
    public IReadOnlyList<StudyFormData> StudyForms { get; set; }
    
    public string StudyYear { get; set; }
}

public class StudyFormData
{
    public StudyForm StudyForm { get; set; }
    
    public IReadOnlyList<FacultyData> Faculties { get; set; }
}

public class FacultyData
{
    public string Name { get; set; }
    
    public string NameDative { get; set; }
    
    public List<RequestReportData> Requests { get; set; }
}

public class RequestReportData
{
    public int Id { get; set; }
    
    public ReportingForm ReportingForm { get; set; }
    
    public double PreExamConsultation { get; set; }
    
    public double ExamHours { get; set; }
    
    public double TestHours { get; set; }
    
    public string DisciplineName { get; set; }

    public string DirectionName { get; set; }

    public string CourseNumber { get; set; }

    public int Semester { get; set; }
    
    public int StudentsCount { get; set; }

    public int TreadsCount { get; set; }

    public int GroupsCount { get; set; }

    public double? IndependentWorkHours { get; set; }

    public double CheckingTestPaperHours { get; set; }

    public HoursCount HourCounts { get; set; }
}

public class HoursCount
{
    public int? Lectures { get; set; }
    public int Practices { get; set; }
    public int Laboratory { get; set; }

    public double ControlOfIndependentWork { get; set; }
    
    public double PracticeManagement { get; set; }
    public double CourseWorks { get; set; }
    public double QualificationWorks { get; set; }

    public double MastersProgramManagement { get; set; }
    
    public double Gac { get; set; }
    public double AspirantManagement { get; set; }
    public double ApplicantManagement { get; set; }
    public int Other { get; set; }
    
    public double ExtracurricularActivity { get; set; }
}