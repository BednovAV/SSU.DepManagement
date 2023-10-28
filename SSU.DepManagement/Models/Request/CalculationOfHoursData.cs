namespace Models.Request;

public class CalculationOfHoursData
{
    public IReadOnlyList<FacultyData> Faculties { get; set; }
}

public class FacultyData
{
    public string Name { get; set; }
    
    public string NameDative { get; set; }
    
    public IReadOnlyList<RequestReportData> Requests { get; set; }
}

public class RequestReportData
{
    public ReportingForm ReportingForm { get; set; }
    
    public string DisciplineName { get; set; }

    public string DirectionName { get; set; }

    public string CourseNumber { get; set; }

    public string Semester { get; set; }
    
    public int StudentsCount { get; set; }

    public int TreadsCount { get; set; }

    public int GroupsCount { get; set; }

    public int IndependentWorkHours { get; set; }

    public HoursCount HourCounts { get; set; }
}

public class HoursCount
{
    public int Lectures { get; set; }
    public int Practices { get; set; }
    public int Laboratory { get; set; }
    public int DisciplineConsultations { get; set; }
    public int ExamConsultations { get; set; }
    public int Exams { get; set; }
    public int PassTests { get; set; }
    public int PracticeManagement { get; set; }
    public int CourseWorks { get; set; }
    public int QualificationWorks { get; set; }
    public int Gac { get; set; }
    public int CheckControlWorks { get; set; }
    public int GraduateStudents { get; set; }
    public int Applicants { get; set; }
    public int MastersProgramManagement { get; set; }
    public int OptionalClasses { get; set; }
    public int Other { get; set; }
}