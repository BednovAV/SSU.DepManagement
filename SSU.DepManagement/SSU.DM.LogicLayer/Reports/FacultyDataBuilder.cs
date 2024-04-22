using Models.Request;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.LogicLayer.Reports;

public class FacultyDataBuilder
{
    public static FacultyData Build(IGrouping<Faculty, List<Request>> grouping)
    {
        return Build(grouping.Key, grouping.SelectMany(x => x).ToList());
    }
    
    public static FacultyData Build(IGrouping<Faculty, Request> grouping)
    {
        return Build(grouping.Key, grouping.ToList());
    }
    
    public static FacultyData Build(Faculty faculty, List<Request> requests)
    {
        return new FacultyData
        {
            Name = faculty.Name ?? "Неизвестный факультет",
            NameDative = faculty.NameDat ?? "неизвестному факультету",
            Requests = requests
                .Select(CreateRequestReportData)
                .ToList(),
        };
    }

    private static RequestReportData CreateRequestReportData(Request request)
        => new()
        {
            ReportingForm = request.Reporting,
            DisciplineName = request.Discipline.Name,
            DirectionName = request.Direction,
            CourseNumber = int.TryParse(request.GroupNumber.AsSpan(0, 3), out var parsed)
                ? (parsed / 100).ToString() : string.Empty,
            Semester = request.Semester.ToString(),
            StudentsCount = request.BudgetCount,
            TreadsCount = 1, // TODO
            GroupsCount = 1, // TODO
            IndependentWorkHours = request.IndependentWorkHours,
            HourCounts = CreateHoursCount(request) // TODO
        };

    private static HoursCount CreateHoursCount(Request request)
    {
        return new HoursCount
        {
            DisciplineConsultations = 0,// TODO
            ExamConsultations = 0, // TODO
            Exams = 0, // TODO
            PassTests = 0, // TODO
            PracticeManagement = 0, // TODO
            CourseWorks = 0, // TODO
            QualificationWorks = 0, // TODO
            Gac = 0, // TODO
            CheckControlWorks = 0, // TODO
            GraduateStudents = 0, // TODO
            Applicants = 0, // TODO
            MastersProgramManagement = 0, // TODO
            OptionalClasses = 0, // TODO
            Other = 0, // TODO
        };
    }
}