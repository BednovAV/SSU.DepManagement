using Models.Request;
using Models.View;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DbEntities;
using SSU.DM.LogicLayer.Interfaces.Request;

namespace SSU.DM.LogicLayer.Requests;

public class RequestAccessor : IRequestAccessor
{
    private const string REQUEST_TEMPLATE_KEY = "REQUEST_TEMPLATE";

    private readonly IFilesStorageDao _filesStorageDao;
    private readonly IApplicationFormDao _applicationFormDao;
    private readonly ITeachersDao _teachersDao;

    public RequestAccessor(
        IFilesStorageDao filesStorageDao,
        IApplicationFormDao applicationFormDao,
        ITeachersDao teachersDao)
    {
        _filesStorageDao = filesStorageDao;
        _applicationFormDao = applicationFormDao;
        _teachersDao = teachersDao;
    }

    public (string name, byte[] bytes) GetFile(string? fileKey = null)
    {
        fileKey ??= REQUEST_TEMPLATE_KEY;
        var dbItem = _filesStorageDao.GetById(fileKey);
        return (dbItem.FileName, dbItem.Bytes);
    }
    
    public IReadOnlyList<ApplicationFormViewItem> GetApplicationForms()
    {
        var applicationForms = _applicationFormDao.GetAll();
        return applicationForms.Select(ConvertToViewItem).OrderByDescending(x => x.DateCreated).ToList();
    }
    
    private ApplicationFormViewItem ConvertToViewItem(ApplicationForm appForm) =>
        new()
        {
            Id = appForm.ApplicationFormId,
            Name = appForm.File.FileName,
            FacultyName = appForm.Faculty?.Name ?? "-",
            DateCreated = appForm.DateTimeCreated,
            FileKey = appForm.FileKey,
            Disciplines = MapAppFormDisciplines(appForm.Requests),
        };

    private IReadOnlyList<AppFormDisciplineViewItem> MapAppFormDisciplines(IEnumerable<Request> appFormRequests)
    {
        return appFormRequests.GroupBy(x => new
            {
                x.Discipline.Name, x.StudyForm
            })
            .OrderBy(x => x.Key.StudyForm)
            .ThenBy(x => x.Key.Name)
            .Select(x => new AppFormDisciplineViewItem
            {
                Name = x.Key.Name,
                Groups = GetGroups(x),
                Semesters = string.Join(", ", x.Select(x => x.YearSemester).Distinct()),
                TotalHours = GetTotalHours(x),
                Requests = GetRequests(x),
                StudyForm = GetStudyFormString(x.Key.StudyForm)
            })
            .ToList();
    }

    private string GetStudyFormString(StudyForm studyForm)
    {
        return studyForm switch
        {
            StudyForm.FullTime => "очн",
            StudyForm.Extramural => "очн-заоч",
            StudyForm.PartTime => "заоч",
            _ => throw new ArgumentOutOfRangeException(nameof(studyForm), studyForm, null)
        };
    }

    private IReadOnlyList<int> GetGroups(IEnumerable<Request> requests)
    {
        return requests.SelectMany(x => x.GroupNumber).Distinct().Order().ToList();
    }

    private double GetTotalHours(IEnumerable<Request> requests)
    {
        return requests.Sum(x => x.TotalHours);
    }

    private IReadOnlyList<RequestViewItem> GetRequests(IEnumerable<Request> requests)
    {
        return requests
            .OrderBy(x => x.LessonForm == LessonForm.Lecture)
            .ThenBy(x => x.YearSemester)
            .ThenBy(x => x.GroupNumberString)
            .Select(MapToRequestViewItem)
            .ToList();
    }
    
    private RequestViewItem MapToRequestViewItem(Request request)
    {
        return new RequestViewItem
        {
            Id = request.Id,
            Direction = string.Join(", ", request.Direction),
            GroupNumber = request.GroupNumberString,
            TotalHours = request.TotalHours,
            LessonForm = GetLessonFromName(request.LessonForm!.Value),
            Teacher = request.Teacher?.ToViewItem(),
            YearSemester = request.YearSemester,
            AvailableTeacherIds = GetAvailableTeacherIds(request)
        };
    }

    private List<long> GetAvailableTeacherIds(Request request)
    {
        if (request.LessonForm is not LessonForm.Lecture and not LessonForm.Practical and not LessonForm.Laboratory)
        {
            return _teachersDao.GetAll().Select(x => x.Id).ToList();
        }
        
        return request.Discipline.Competencies
            .Where(x => x.FacultyId == request.ApplicationForm.FacultyId && x.LessonForm == request.LessonForm)
            .OrderByDescending(x => x.Priority)
            .Select(x => x.TeacherId)
            .ToList();
    }

    private static string GetLessonFromName(LessonForm lessonForm)
    {
        return lessonForm switch
        {
            LessonForm.Lecture => "Лекции",
            LessonForm.Practical => "Практики",
            LessonForm.Laboratory => "Лабораторные",
            _ => string.Empty
        };
    }
}
