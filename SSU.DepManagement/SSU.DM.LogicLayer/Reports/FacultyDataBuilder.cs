using Models.Extensions;
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
            Requests = BuildAggregatedRequests(requests),
        };
    }

    private static IReadOnlyList<RequestReportData> BuildAggregatedRequests(List<Request> requests)
    {
        var otherRequests = requests.ToList();
        var result = new List<RequestReportData>();
        foreach (var lectureRequest in requests.Where(x => x.LessonForm == LessonForm.Lecture))
        {
            var requestsSet = requests
                .Where(x => (x.LessonForm is LessonForm.Practical or LessonForm.Laboratory)
                    && x.DisciplineId == lectureRequest.DisciplineId
                    && lectureRequest.Direction.Contains(x.Direction[0]))
                .ToList();

            otherRequests = otherRequests.Except(requestsSet.Append(lectureRequest)).ToList();
            result.AddRange(BuildStandardReportData(lectureRequest, requestsSet));
        }

        foreach (var grouping in otherRequests.GroupBy(x => new { Direction = x.Direction[0], x.DisciplineId }))
        {
            result.AddRange(BuildStandardReportData(grouping.First(), grouping.Skip(1).ToList()));
        }

        return result;
    }

    private static IEnumerable<RequestReportData> BuildStandardReportData(
        Request mainRequest,
        List<Request> requestsSet)
    {
        for (var i = 0; i < mainRequest.Direction.Count; i++)
        {
            var reportingForm = mainRequest.Reporting.GetAtOrFirst(i);
            var directionGroups = mainRequest.GroupNumbersByDirection[i];
            var mainDirectionGroup = directionGroups[0];
            var subRequests = requestsSet
                .Where(x => directionGroups.Contains(x.GroupNumber[0]))
                .ToList();
            
            yield return new RequestReportData
            {
                ReportingForm = reportingForm,
                DisciplineName = mainRequest.Discipline.Name,
                DirectionName = mainRequest.Direction[i],
                CourseNumber = (mainDirectionGroup / 100).ToString(),
                Semester = mainRequest.Semester.GetAtOrFirst(i),
                StudentsCount = mainRequest.BudgetCount.GetAtOrFirst(i) + mainRequest.CommercialCount.GetAtOrFirst(i),
                TreadsCount = directionGroups.Length,
                GroupsCount = subRequests.Count(x => x.SubgroupNumber.HasValue),
                IndependentWorkHours = mainRequest.IndependentWorkHours?.GetAtOrFirst(i),
                HasTestPaper = mainRequest.HasTestPaper || subRequests.Any(x => x.HasTestPaper),
                HourCounts = new HoursCount
                {
                    Lectures = i == 0 && mainRequest.LessonForm == LessonForm.Lecture ? mainRequest.LessonHours : null,
                    Practices = subRequests.Append(mainRequest).Where(x => x.LessonForm == LessonForm.Practical).Sum(x => x.LessonHours),
                    Laboratory = subRequests.Append(mainRequest).Where(x => x.LessonForm == LessonForm.Laboratory).Sum(x => x.LessonHours),
                }
            };
        }
    }
}