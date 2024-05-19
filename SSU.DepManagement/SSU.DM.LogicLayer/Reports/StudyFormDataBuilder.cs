using Models.Extensions;
using Models.Request;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.LogicLayer.Reports;

public class StudyFormDataBuilder
{
    public static List<StudyFormData> Build(IEnumerable<Request> requests)
    {
        return requests.GroupBy(x => x.StudyForm)
            .Select(x => Build(x.Key, x.ToList()))
            .ToList();
    }

    private static StudyFormData Build(StudyForm studyForm, List<Request> requests)
    {
        return new StudyFormData
        {
            StudyForm = studyForm,
            Faculties = requests.GroupBy(x => x.ApplicationForm.Faculty, new FacultyComparer())
                .Select(x => Build(x.Key, x.ToList()))
                .ToList()
        };
    }
    
    private static FacultyData Build(Faculty faculty, List<Request> requests)
    {
        return new FacultyData
        {
            Name = faculty.Name ?? "Неизвестный факультет",
            NameDative = faculty.NameDat ?? "неизвестному факультету",
            Requests = BuildAggregatedRequests(requests).OrderBy(x => x.DisciplineName).ToList()
        };
    }

    private static List<RequestReportData> BuildAggregatedRequests(List<Request> requests)
    {
        var otherRequests = requests.ToList();
        var result = new List<RequestReportData>();
        foreach (var lectureRequest in requests.Where(x => x.LessonForm == LessonForm.Lecture))
        {
            var requestsSet = requests
                .Where(x => (x.LessonForm is LessonForm.Practical or LessonForm.Laboratory)
                    && x.DisciplineId == lectureRequest.DisciplineId
                    && lectureRequest.Direction.Contains(x.Direction[0])
                    && x.Semester.FirstOrDefault() == lectureRequest.Semester.FirstOrDefault())
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
            var directionGroups = mainRequest.GroupNumbersByDirection.GetAtOrFirst(i);
            var mainDirectionGroup = directionGroups.GetAtOrFirst(0);
            var subRequests = requestsSet
                .Where(x => directionGroups.Contains(x.GroupNumber[0]))
                .ToList();
            
            yield return new RequestReportData
            {
                ReportingForm = reportingForm,
                TestHours = mainRequest.TestHours?.GetAtOrFirst(i) ?? default,
                ExamHours = mainRequest.ExamHours?.GetAtOrFirst(i) ?? default,
                PreExamConsultation = mainRequest.PreExamConsultation?.GetAtOrFirst(i) ?? default,
                DisciplineName = mainRequest.Discipline.Name,
                DirectionName = mainRequest.Direction[i],
                CourseNumber = (mainDirectionGroup / 100).ToString(),
                Semester = mainRequest.Semester.GetAtOrFirst(i),
                StudentsCount = mainRequest.BudgetCount.GetAtOrFirst(i)/* + mainRequest.CommercialCount.GetAtOrFirst(i)*/,
                TreadsCount = directionGroups?.Length ?? default,
                GroupsCount = subRequests.Count(x => x.SubgroupNumber.HasValue),
                IndependentWorkHours = mainRequest.IndependentWorkHours?.GetAtOrFirst(i),
                CheckingTestPaperHours = mainRequest.CheckingTestPaperHours?.GetAtOrFirst(i) ?? default,
                HourCounts = new HoursCount
                {
                    Lectures = i == 0 && mainRequest.LessonForm == LessonForm.Lecture ? mainRequest.LessonHours : null,
                    Practices = subRequests.Append(mainRequest).Where(x => x.LessonForm == LessonForm.Practical).Sum(x => x.LessonHours),
                    Laboratory = subRequests.Append(mainRequest).Where(x => x.LessonForm == LessonForm.Laboratory).Sum(x => x.LessonHours),
                    ControlOfIndependentWork = mainRequest.ControlOfIndependentWork?.GetAtOrFirst(i) ?? default,
                    PracticeManagement = mainRequest.PracticeManagement,
                    CourseWorks = mainRequest.CourseWork,
                    QualificationWorks = mainRequest.DiplomaWork,
                    MastersProgramManagement = mainRequest.MasterManagement,
                    Gac = mainRequest.Gac,
                    AspirantManagement = mainRequest.AspirantManagement,
                    ApplicantManagement = mainRequest.ApplicantManagement,
                    ExtracurricularActivity = mainRequest.ExtracurricularActivity,
                    Other = mainRequest.Other + subRequests.Sum(x => x.Other),
                }
            };
        }
    }

    class FacultyComparer : IEqualityComparer<Faculty?>
    {
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
}