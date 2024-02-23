using Models.Request;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DbEntities;
using SSU.DM.LogicLayer.Interfaces.Reports;
using SSU.DM.Tools.Interface;

namespace SSU.DM.LogicLayer.Reports;

public class CalculationOfHoursBuilder : ICalculationOfHoursBuilder, IEqualityComparer<Faculty>
{
    private readonly IExcelWriter _excelWriter;
    private readonly IApplicationFormDao _applicationFormDao;

    public CalculationOfHoursBuilder(
        IExcelWriter excelWriter,
        IApplicationFormDao applicationFormDao)
    {
        _excelWriter = excelWriter;
        _applicationFormDao = applicationFormDao;
    }

    public byte[] BuildReport(ISet<Guid> appFormIds)
    {
        var appForms = _applicationFormDao.GetAll(x => appFormIds.Contains(x.ApplicationFormId));
        var faculties = appForms
            .GroupBy(x => x.Faculty, x => x.Requests, this)
            .Select(x => new FacultyData
            {
                Name = x.Key?.Name ?? "Неизвестный факультет",
                NameDative = x.Key?.NameDat ?? "неизвестному факультету",
                Requests = x.SelectMany(r => r)
                    .OrderBy(r => r.Discipline.Name)
                    .Select(CreateRequestReportData)
                    .ToList(),
            }).ToList();
        
        var data = new CalculationOfHoursData { Faculties = faculties };
        return _excelWriter.Write(data).FileBytes;
    }

    private RequestReportData CreateRequestReportData(Request request)
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

    private HoursCount CreateHoursCount(Request request)
    {
        return new HoursCount
        {
            Lectures = request.LectureHours,
            Practices = request.PracticalHours,
            Laboratory = request.LaboratoryHours,
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

    public bool Equals(Faculty? x, Faculty? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id;
    }

    public int GetHashCode(Faculty obj) => obj.Id;
}