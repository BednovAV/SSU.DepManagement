using Models.Enums;
using Models.Extensions;
using Models.Request;
using SSU.DM.DataAccessLayer.Core.Interface;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.LogicLayer.Interfaces.Discipline;
using SSU.DM.LogicLayer.Interfaces.Request;
using SSU.DM.LogicLayer.Interfaces.Teachers;
using SSU.DM.Tools.Interface;
using SSU.DM.Tools.Interface.Models;

namespace SSU.DM.LogicLayer.Requests;

public class RequestEditor : IRequestEditor
{
    private readonly IExcelParser _excelParser;
    private readonly IFilesStorageDao _filesStorage;
    private readonly IApplicationFormDao _applicationFormDao;
    private readonly IRequestDao _requestDao;
    private readonly ITeachersDao _teachersDao;
    private readonly ITeacherLogic _teachersLogic;
    private readonly IDisciplineLogic _disciplineLogic;
    private readonly ITransactionManager _transactionManager;
    
    public RequestEditor(
        IExcelParser excelParser,
        IFilesStorageDao filesStorage,
        IApplicationFormDao applicationFormDao,
        IRequestDao requestDao,
        ITeachersDao teachersDao,
        ITeacherLogic teachersLogic,
        IDisciplineLogic disciplineLogic,
        ITransactionManager transactionManager)
    {
        _excelParser = excelParser;
        _filesStorage = filesStorage;
        _applicationFormDao = applicationFormDao;
        _requestDao = requestDao;
        _teachersDao = teachersDao;
        _teachersLogic = teachersLogic;
        _disciplineLogic = disciplineLogic;
        _transactionManager = transactionManager;
    }

    public async Task<Guid> UploadFromStream(string fileName, Stream stream)
    {
        var bytes = await stream.ReadAllBytesAsync();
        
        var requests = ProcessRequests(_excelParser.Parse<List<ParsedRequest>>(bytes));
        //var requests = _excelParser.Parse<List<ParsedRequest>>(bytes);
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

    private List<RequestSaveItem> ProcessRequests(ParseResult<List<ParsedRequest>> parsed)
    {
        return parsed.Value.GroupBy(request => new { request.NameDiscipline, request.Semester })
            .SelectMany(ProcessRequest).ToList();
    }

    private List<RequestSaveItem> ProcessRequest(IEnumerable<ParsedRequest> requests)
    {
        requests = requests.ToList();
        
        var result = new List<RequestSaveItem>();
        var disciplineId = _disciplineLogic.GetOrCreateDisciplineId(requests.First().NameDiscipline);

        result.AddRange(GetLectures(requests, disciplineId));
        result.AddRange(GetPracticals(requests, disciplineId));
        result.AddRange(GetLaboratories(requests, disciplineId));

        return result;
    }

    private static List<RequestSaveItem> GetLectures(IEnumerable<ParsedRequest> requests, long disciplineId)
    {
        return requests.Where(x => x.LectureHours != 0)
            .Select(lecture => new RequestSaveItem
            {
                DisciplineId = disciplineId,
                Direction = lecture.Direction,
                Semester = lecture.Semester,
                BudgetCount = lecture.BudgetCount,
                CommercialCount = lecture.CommercialCount,
                GroupNumber = lecture.GroupNumber,
                GroupForm = lecture.GroupForm,
                LessonHours = lecture.LectureHours,
                ControlOfIndependentWorkHours = CalculateControlOfIndependentWorkHours(lecture), // TODO:
                PreExamConsultationHours = 0, // TODO
                CheckingTestPaperHours = 0, // TODO
                ExamHours = 0, // TODO
                //TotalHours = 0, // TODO:
                IndependentWorkHours = lecture.IndependentWorkHours,
                LessonForm = LessonForm.Lecture,
                Reporting = lecture.Reporting,
                Note = lecture.Note
            }).ToList();
    }

    private static double? CalculateControlOfIndependentWorkHours(ParsedRequest request)
    {
        return null;
        //return request.IndependentWorkHours * request.Co
    }

    private IEnumerable<RequestSaveItem> GetPracticals(IEnumerable<ParsedRequest> requests, long disciplineId)
    {
        return requests.Where(x => x.PracticalHours != 0)
            .Select(lecture => new RequestSaveItem
            {
                DisciplineId = disciplineId,
                Direction = lecture.Direction,
                Semester = lecture.Semester,
                BudgetCount = lecture.BudgetCount,
                CommercialCount = lecture.CommercialCount,
                GroupNumber = lecture.GroupNumber,
                GroupForm = lecture.GroupForm,
                LessonHours = lecture.PracticalHours,
                TotalHours = 0, // TODO:
                IndependentWorkHours = lecture.IndependentWorkHours,
                LessonForm = LessonForm.Practical,
                Reporting = lecture.Reporting,
                Note = lecture.Note
            }).ToList();
    }

    private IEnumerable<RequestSaveItem> GetLaboratories(IEnumerable<ParsedRequest> requests, long disciplineId)
    {
        return requests.Where(x => x.LaboratoryHours != 0)
            .Select(lecture => new RequestSaveItem
            {
                DisciplineId = disciplineId,
                Direction = lecture.Direction,
                Semester = lecture.Semester,
                BudgetCount = lecture.BudgetCount,
                CommercialCount = lecture.CommercialCount,
                GroupNumber = lecture.GroupNumber,
                GroupForm = lecture.GroupForm,
                LessonHours = lecture.LaboratoryHours,
                TotalHours = 0, // TODO:
                IndependentWorkHours = lecture.IndependentWorkHours,
                LessonForm = LessonForm.Laboratory,
                Reporting = lecture.Reporting,
                Note = lecture.Note
            }).ToList();
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

    public string? CreateTeacherLink(int requestId, long? teacherId)
    {
        if (!teacherId.HasValue)
        {
            _requestDao.SetTeacherId(requestId, teacherId);
            return null;
        }
        
        var teacher = _teachersDao.GetById(teacherId.Value);
        var request = _requestDao.GetById(requestId);

        var allocatedTeacherHours = teacher.Requests
            //.Where(semesterId)
            .Sum(r => r.TotalHours);
        var totalTeacherHours = teacher.Capacities
            ?.FirstOrDefault() //Where(semesterId)
            ?.Hours ?? 0;

        if (request.TotalHours + allocatedTeacherHours > totalTeacherHours)
        {
            return
                $"Для назначения на данную дисциплину требуется {request.TotalHours} а.ч., " +
                $"преподаватель \"{teacher.ToViewItem()}\" имеет {totalTeacherHours - allocatedTeacherHours} свободных а.ч.";
        }
        
        _requestDao.SetTeacherId(requestId, teacherId);
        return null;
    }

    public void AssignTeachers(HashSet<Guid> appFromIds)
    {
        var requests = _requestDao.GetAll(request 
                => !request.TeacherId.HasValue && appFromIds.Contains(request.ApplicationFormId))
            .Select(RequestConverter.MapToRequestViewItem)
            .OrderByDescending(request => request.TotalHours)
            .ThenBy(request => request.AvailableTeacherIds.Count)
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
            var teacherId = freeHoursByTeacherId
                .OrderBy(x => x.Value)
                .Cast<KeyValuePair<long, float?>?>()
                .FirstOrDefault(x =>
                    request.AvailableTeacherIds.Contains(x?.Key ?? -1)
                    && request.TotalHours <= x?.Value)
                ?.Key;

            if (teacherId.HasValue)
            {
                _requestDao.SetTeacherId(request.Id, teacherId);
                freeHoursByTeacherId[teacherId.Value] -= request.TotalHours;
            }
        }
        
    }
}
