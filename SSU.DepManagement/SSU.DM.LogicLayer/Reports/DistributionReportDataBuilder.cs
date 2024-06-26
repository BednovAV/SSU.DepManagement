﻿using Models.Request;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DbEntities;
using SSU.DM.LogicLayer.Interfaces.Reports;
using SSU.DM.Tools.Interface;

namespace SSU.DM.LogicLayer.Reports;

public class DistributionReportDataBuilder : IDistributionReportDataBuilder, IEqualityComparer<Faculty>
{
    private readonly IApplicationFormDao _applicationFormDao;

    public DistributionReportDataBuilder(
        IApplicationFormDao applicationFormDao)
    {
        _applicationFormDao = applicationFormDao;
    }

    public DistributionReportData BuildData(ISet<Guid> appFormIds)
    {
        var appForms = _applicationFormDao.GetAll(x => appFormIds.Contains(x.ApplicationFormId));

        var teachers = appForms.SelectMany(x => x.Requests)
            .GroupBy(x => x.TeacherId)
            .Where(x => x.Key.HasValue)
            .Select(x => BuildTeacherData(x.ToList()))
            .ToList();

        var data = new DistributionReportData
        {
            Teachers = teachers
        };

        return data;
    }

    private TeacherData BuildTeacherData(List<Request> requests)
    {
        var teacher = requests.First().Teacher;

        return new TeacherData
        {
            TeacherName = $"{teacher.LastName} {teacher.FirstName} {teacher.FirstName}",
            StudyYear = "2024/2025",
            BudgetForm = "Бюджетная",
            JobTitle = teacher.JobTitle?.Name ?? string.Empty,
            Rate = teacher.Rate?.ToString() ?? string.Empty,
            FirstSemester = BuildSemesterData(requests.Where(x => x.YearSemester == 1)),
            SecondSemester = BuildSemesterData(requests.Where(x => x.YearSemester == 2))
        };
    }

    private List<StudyFormData> BuildSemesterData(IEnumerable<Request> requests)
    {
        return StudyFormDataBuilder.Build(requests);
    }

    public bool Equals(Faculty? x, Faculty? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id;
    }

    public int GetHashCode(Faculty obj)
    {
        return HashCode.Combine(obj.Id, obj.Name, obj.NameDat, obj.Competencies);
    }
}