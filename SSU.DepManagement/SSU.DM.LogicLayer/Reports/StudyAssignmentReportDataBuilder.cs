using Models.Extensions;
using Models.Request;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DbEntities;
using SSU.DM.LogicLayer.Interfaces.Reports;
using SSU.DM.Tools.Interface;

namespace SSU.DM.LogicLayer.Reports;

public class StudyAssignmentReportDataBuilder : IStudyAssignmentReportDataBuilder
{
    private readonly IApplicationFormDao _applicationFormDao;

    public StudyAssignmentReportDataBuilder(
        IApplicationFormDao applicationFormDao)
    {
        _applicationFormDao = applicationFormDao;
    }

    public StudyAssignmentCardData BuildData(ISet<Guid> appFormIds)
    {
        var appForms = _applicationFormDao.GetAll(x => appFormIds.Contains(x.ApplicationFormId));

        var teachersData = appForms.SelectMany(x => x.Requests)
                .GroupBy(x => x.TeacherId)
                .Where(x => x.Key.HasValue)
                .Select(x => BuildStudyAssignmentTeacherData(x.ToList()))
                .ToList();

        var data = new StudyAssignmentCardData
        {
            StudyYear = "2024/2025",
            Teachers = teachersData
        };
        
        return data;
    }

    private StudyAssignmentTeacherData BuildStudyAssignmentTeacherData(List<Request> requests)
    {
        var teacher = requests.First().Teacher;

        return new StudyAssignmentTeacherData
        {
            FioWithJobTitle =
                $"{teacher.LastName} {teacher.FirstName.First()}.{teacher.MiddleName.First()}. {teacher?.JobTitle?.Name} {teacher.Rate} шт.ед",
            HoursByStudyForm = requests.GroupBy(request => request.StudyForm)
                .ToDictionary(
                    x => GetStudyFormString(x.Key),
                    x => GetHours(x.ToList()))
        };
    }

    private string GetStudyFormString(StudyForm studyForm)
    {
        return studyForm switch
        {
            StudyForm.FullTime => "очн",
            StudyForm.Extramural => "заоч",
            StudyForm.PartTime => "очн-заоч",
            _ => throw new ArgumentOutOfRangeException(nameof(studyForm), studyForm, null)
        };
    }
    
    private StudyAssignmentHours GetHours(List<Request> requests)
    {
        return new StudyAssignmentHours
        {
            Lectures = requests.Where(x => x.LessonForm == LessonForm.Lecture).Sum(x => x.LessonHours),
            Practicals = requests.Where(x => x.LessonForm == LessonForm.Practical).Sum(x => x.LessonHours),
            Laboratory = requests.Where(x => x.LessonForm == LessonForm.Laboratory).Sum(x => x.LessonHours),
            ControlOfIndependentWork = requests.Sum(x => x.ControlOfIndependentWork?.Sum() ?? default),
            PreExamConsultation = requests.Sum(x => x.PreExamConsultation?.SumWhereNotNull() ?? default),
            Exam = requests.Sum(x => x.ExamHours.SumWhereNotNull()),
            Test = requests.Sum(x => x.TestHours.SumWhereNotNull()),
            PracticeManagement = requests.Sum(x => x.PracticeManagement),
            CourseWork = requests.Sum(x => x.CourseWork),
            DiplomaWork = requests.Sum(x => x.DiplomaWork),
            Gac = requests.Sum(x => x.Gac),
            CheckingTestPaperHours = requests.Sum(x => x.CheckingTestPaperHours?.Sum() ?? default),
            AspirantManagement = requests.Sum(x => x.AspirantManagement),
            ApplicantManagement = requests.Sum(x => x.ApplicantManagement),
            MasterManagement = requests.Sum(x => x.ApplicantManagement),
            ExtracurricularActivity = requests.Sum(x => x.ExtracurricularActivity),
        };
    }
}
