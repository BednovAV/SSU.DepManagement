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

    public RequestAccessor(
        IFilesStorageDao filesStorageDao,
        IApplicationFormDao applicationFormDao)
    {
        _filesStorageDao = filesStorageDao;
        _applicationFormDao = applicationFormDao;
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
                x.Discipline.Name
            })
            .Select(x => new AppFormDisciplineViewItem
            {
                Name = x.Key.Name,
                Groups = GetGroups(x),
                Semester = x.First().YearSemester,
                TotalHours = GetTotalHours(x),
                Requests = GetRequests(x)
            })
            .OrderBy(x => x.Name)
            .ToList();
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
            .OrderBy(x => x.LessonForm)
            .ThenBy(x => x.GroupNumberString)
            .Select(RequestConverter.MapToRequestViewItem)
            .ToList();
    }
}
