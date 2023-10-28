using SSU.DM.DataAccessLayer.Core.Interface;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using Models.Extensions;
using Models.Request;
using SSU.DM.LogicLayer.Interfaces.Request;
using SSU.DM.Tools.Interface;

namespace SSU.DM.LogicLayer.Request;

public class RequestEditor : IRequestEditor
{
    private readonly IExcelParser _excelParser;
    private readonly IFilesStorageDao _filesStorage;
    private readonly IApplicationFormDao _applicationFormDao;
    private readonly IRequestDao _requestDao;
    private readonly ITransactionManager _transactionManager;
    
    public RequestEditor(
        IExcelParser excelParser,
        IFilesStorageDao filesStorage,
        IApplicationFormDao applicationFormDao,
        IRequestDao requestDao,
        ITransactionManager transactionManager)
    {
        _excelParser = excelParser;
        _filesStorage = filesStorage;
        _applicationFormDao = applicationFormDao;
        _requestDao = requestDao;
        _transactionManager = transactionManager;
    }

    public async Task<Guid> UploadFromStream(string fileName, Stream stream)
    {
        var bytes = await stream.ReadAllBytesAsync();
        
        var requests = _excelParser.Parse<List<RequestItem>>(bytes);
        var fileKey = Guid.NewGuid().ToString();
        var applicationFormId = Guid.NewGuid();

        await _transactionManager.TransactionScope(t =>
        {
            _filesStorage.Save(fileKey, fileName, bytes);
            _applicationFormDao.Add(applicationFormId, DateTimeOffset.Now.UtcDateTime, fileKey);
            _requestDao.AddRange(requests.Value, applicationFormId);
        });
        
        return applicationFormId;
    }

    public async Task<bool> DeleteAsync(Guid appFormId)
    {
        var appForm = _applicationFormDao.GetById(appFormId);
        try
        {
            await _transactionManager.TransactionScope(t =>
            {
                _requestDao.DeleteByIds(appForm.Requests.Select(r => r.Id));
                _filesStorage.DeleteById(appForm.FileKey);
                _applicationFormDao.DeleteById(appForm.ApplicationFormId);
            });
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }
}
