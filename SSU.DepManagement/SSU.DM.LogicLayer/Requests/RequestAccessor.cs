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
        return applicationForms.Select(ConvertToViewItem).ToList();
    }
    
    private ApplicationFormViewItem ConvertToViewItem(ApplicationForm appForm) =>
        new()
        {
            Id = appForm.ApplicationFormId,
            Name = appForm.File.FileName,
            FacultyName = appForm.Faculty?.Name ?? "-",
            DateCreated = appForm.DateTimeCreated,
            FileKey = appForm.FileKey,
            Requests = appForm.Requests
                .OrderBy(r => r.Discipline.Name)
                .ThenBy(r => r.GroupNumber)
                .ThenBy(r => r.TotalHours)
                .Select(RequestConverter.ConvertToViewItem)
                .ToList(),
        };

   
}
