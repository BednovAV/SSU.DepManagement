using Models.Extensions;
using Models.Request;
using Models.View;
using SSU.DM.DataAccessLayer.Core.Interface;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DbEntities;
using SSU.DM.LogicLayer.Interfaces.Competencies;
using SSU.DM.LogicLayer.Interfaces.Request;
using SSU.DM.LogicLayer.Interfaces.Teachers;
using SSU.DM.Tools.Interface;
using SSU.DM.WebAssembly.Shared.Models;

namespace SSU.DM.LogicLayer.Requests;

public class RequestEditor : IRequestEditor
{
    private readonly IExcelParser _excelParser;
    private readonly IFilesStorageDao _filesStorage;
    private readonly IApplicationFormDao _applicationFormDao;
    private readonly IRequestDao _requestDao;
    private readonly ITeachersDao _teachersDao;
    private readonly ITeacherLogic _teachersLogic;
    private readonly ITransactionManager _transactionManager;
    private readonly IParsedRequestProcessor _parsedRequestProcessor;
    private readonly ICompetenceDao _competenceDao;

    public RequestEditor(
        IExcelParser excelParser,
        IFilesStorageDao filesStorage,
        IApplicationFormDao applicationFormDao,
        IRequestDao requestDao,
        ITeachersDao teachersDao,
        ITeacherLogic teachersLogic,
        ITransactionManager transactionManager,
        IParsedRequestProcessor parsedRequestProcessor,
        ICompetenceDao competenceDao)
    {
        _excelParser = excelParser;
        _filesStorage = filesStorage;
        _applicationFormDao = applicationFormDao;
        _requestDao = requestDao;
        _teachersDao = teachersDao;
        _teachersLogic = teachersLogic;
        _transactionManager = transactionManager;
        _parsedRequestProcessor = parsedRequestProcessor;
        _competenceDao = competenceDao;
    }

    public async Task<Guid> UploadFromStream(string fileName, Stream stream)
    {
        var bytes = await stream.ReadAllBytesAsync();

        var parseResult = _excelParser.Parse<List<ParsedRequest>>(bytes);
        var requests = _parsedRequestProcessor.Process(parseResult.Value);
        var fileKey = Guid.NewGuid().ToString();
        var applicationFormId = Guid.NewGuid();

        await _transactionManager.TransactionScope(t =>
        {
            _filesStorage.Save(fileKey, fileName, bytes);
            _applicationFormDao.Add(applicationFormId, DateTimeOffset.Now.UtcDateTime, fileKey);
            _requestDao.AddRange(requests, applicationFormId);
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

    public void CreateFacultyLinkAsync(Guid appFormId, int facultyId)
    {
        _applicationFormDao.SetFacultyId(appFormId, facultyId);
    }

    public CreateTeacherLinkResponse CreateTeacherLink(int requestId, long? teacherId)
    {
        if (!teacherId.HasValue)
        {
            _requestDao.SetTeacherId(requestId, teacherId);
            return new CreateTeacherLinkResponse
            {
                Type = CreateTeacherLinkResponse.ResponseType.Success,
                Message = "Назначение успешно удалено!"
            };
        }

        var teacher = _teachersDao.GetById(teacherId.Value);
        var request = _requestDao.GetById(requestId);

        var allocatedTeacherHours = teacher.Requests
            //.Where(semesterId)
            .Sum(r => r.TotalHours);
        var totalTeacherUpperHours = teacher.Bounds.Upper;
        var totalTeacherLowerHours = teacher.Bounds.Lower;

        if (request.TotalHours + allocatedTeacherHours > totalTeacherUpperHours)
        {
            return new CreateTeacherLinkResponse
            {
                Type = CreateTeacherLinkResponse.ResponseType.Error,
                Message = $"Чтобы назначить преподавателя на дисциплину требуется {request.TotalHours} свободных ч." +
                          $"преподаватель \"{teacher.ToViewItem()}\" имеет {Math.Round(totalTeacherUpperHours - allocatedTeacherHours, 1)} свободных а.ч."
            };
        }
        
        _requestDao.SetTeacherId(requestId, teacherId);
        if (request.TotalHours + allocatedTeacherHours > totalTeacherLowerHours)
        {
            return new CreateTeacherLinkResponse
            {
                Type = CreateTeacherLinkResponse.ResponseType.Warning,
                Message = $"Для преподавателя \"{teacher.ToViewItem()}\" была превышена нижняя граница часов!"
            };
        }

        return new CreateTeacherLinkResponse
        {
            Type = CreateTeacherLinkResponse.ResponseType.Success,
            Message = $"Преподаватель \"{teacher.ToViewItem()}\" успешно назначен на дисциплину!"
        };
    }

    public void AssignTeachers(HashSet<Guid> appFromIds)
    {
        var requests = _requestDao.GetAll(request
                => !request.TeacherId.HasValue 
                   && appFromIds.Contains(request.ApplicationFormId)
                   && request.LessonForm is LessonForm.Lecture or LessonForm.Laboratory or LessonForm.Practical)
            .OrderByDescending(request => request.TotalHours)
            .ToList();

        var freeHoursByTeacherId = _teachersLogic.GetTeacherCapacities().ToDictionary(
            item => item.Teacher.Id,
            item =>
            {
                var capacity = item.CapacityBySemester.First().Value;
                return item.Teacher.JobTitle.UpperBoundHours * item.Teacher.Rate - capacity;
            });

        foreach (var request in requests)
        {
            var teacherId = SelectTeacherIdForRequest(freeHoursByTeacherId, request);

            if (teacherId.HasValue)
            {
                _requestDao.SetTeacherId(request.Id, teacherId);
                freeHoursByTeacherId[teacherId.Value] -= request.TotalHours;
            }
        }
        
    }

    private long? SelectTeacherIdForRequest(
        Dictionary<long, double?> freeHoursByTeacherId,
        Request request)
    {
        var teachersByPriority = _competenceDao.GetAll()
            .Where(x =>
                x.DisciplineId == request.DisciplineId
                && x.FacultyId == request.ApplicationForm.FacultyId
                && x.LessonForm == request.LessonForm)
            .ToLookup(x => x.Priority, x => x.TeacherId);

        foreach (var teacherIds in teachersByPriority.OrderByDescending(x => x.Key))
        {
            var teacherId = freeHoursByTeacherId
                //.OrderBy(x => x.Value)
                .OrderByDescending(x => x.Value) // эксперимент
                .Cast<KeyValuePair<long, double?>?>()
                .FirstOrDefault(x => x != null
                                    && teacherIds.Contains(x.Value.Key)
                                     && request.TotalHours <= x.Value.Value)
                ?.Key;

            if (teacherId.HasValue)
            {
                return teacherId;
            }
        }

        return null;
    }
}