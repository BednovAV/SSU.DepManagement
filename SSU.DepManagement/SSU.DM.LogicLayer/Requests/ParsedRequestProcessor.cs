using Models.Extensions;
using Models.Request;
using SSU.DM.LogicLayer.Interfaces.Discipline;
using SSU.DM.LogicLayer.Interfaces.Request;

namespace SSU.DM.LogicLayer.Requests;

public class ParsedRequestProcessor : IParsedRequestProcessor
{
    private const string GAC_DISCIPLINE_NAME = "ГАК";
    
    private readonly IDisciplineLogic _disciplineLogic;

    public ParsedRequestProcessor(IDisciplineLogic disciplineLogic)
    {
        _disciplineLogic = disciplineLogic;
    }

    public List<RequestSaveItem> Process(IReadOnlyList<ParsedRequest> parsedRequests)
    {
        return parsedRequests
            .Select(ProcessOne)
            .Where(saveItem => saveItem != null)
            .Cast<RequestSaveItem>()
            .ToList();
    }

    private RequestSaveItem? ProcessOne(ParsedRequest parsedRequest)
    {
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(GAC_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildGac(parsedRequest);
        }
        
        // TODO: курсовые, ВКР, практика, ГЭК

        var disciplineId = _disciplineLogic.GetOrCreateDisciplineId(parsedRequest.NameDiscipline);
        if (parsedRequest.LectureHours != 0)
        {
            return BuildLectureItem(disciplineId, parsedRequest);
        }

        if (parsedRequest.PracticalHours != 0)
        {
            return BuildPracticalItem(disciplineId, parsedRequest);
        }

        if (parsedRequest.LaboratoryHours != 0)
        {
            return BuildLaboratoryItem(disciplineId, parsedRequest);
        }

        return null;
    }

    private RequestSaveItem BuildLaboratoryItem(long disciplineId, ParsedRequest parsedRequest)
    {
        var (groupNumber, subGroupNumber) = ParseLaboratoryGroupNumber(parsedRequest.GroupNumber);
        var saveItem = BuildCommonFields(disciplineId, parsedRequest);
        saveItem.GroupNumber = new List<int> { groupNumber };
        saveItem.SubgroupNumber = subGroupNumber;
        saveItem.LessonForm = LessonForm.Laboratory;
        saveItem.LessonHours = parsedRequest.LaboratoryHours;
        // if (parsedRequest.Reporting == ReportingForm.Test)
        // {
        //     saveItem.Reporting = ReportingForm.Test;
        //     saveItem.ReportingHours = Math.Round(parsedRequest.StudentsCount / 3d, 1);
        // }

        return saveItem;
    }

    private (int groupNumber, int? subGroupNumber) ParseLaboratoryGroupNumber(string groupNumber)
    {
        var splittedGroupNumber = groupNumber.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
        if (splittedGroupNumber.Length == 1)
        {
            return (int.Parse(splittedGroupNumber[0]), null);
        }
        return (int.Parse(splittedGroupNumber[0]), int.Parse(splittedGroupNumber[1]));
    }

    private RequestSaveItem BuildPracticalItem(long disciplineId, ParsedRequest parsedRequest)
    {
        
        var saveItem = BuildCommonFields(disciplineId, parsedRequest);
        saveItem.GroupNumber = parsedRequest.GroupNumber.SplitAndParseToInt();
        saveItem.LessonForm = LessonForm.Practical;
        saveItem.LessonHours = parsedRequest.PracticalHours;
        if (!string.IsNullOrEmpty(parsedRequest.IndependentWorkHours))
        {
            saveItem.IndependentWorkHours = parsedRequest.IndependentWorkHours.SplitAndParseToInt();
            saveItem.ControlOfIndependentWork = CalculateControlOfIndependentWork(saveItem);
        }
        saveItem.HasTestPaper = true;

        return saveItem;
    }

    private static double CalculateControlOfIndependentWork(RequestSaveItem saveItem)
    {
        var result = 0d;
        for (var i = 0; i < saveItem.Direction.Count; i++)
        {
            var independentWorkHours = saveItem.IndependentWorkHours.GetAtOrFirst(i);
            
            result += Math.Round(independentWorkHours * 0.05d, 1);
        }
        return result;
    }

    private RequestSaveItem BuildLectureItem(long disciplineId, ParsedRequest parsedRequest)
    {
        var groups = parsedRequest.GroupNumber.SplitAndParseToInt();
        var saveItem = BuildCommonFields(disciplineId, parsedRequest);
        saveItem.GroupNumber = groups;
        saveItem.LessonForm = LessonForm.Lecture;
        saveItem.LessonHours = parsedRequest.LectureHours;
        saveItem.IndependentWorkHours = parsedRequest.IndependentWorkHours.SplitAndParseToInt();
        saveItem.ControlOfIndependentWork = CalculateControlOfIndependentWork(saveItem);
        var reportingHours = CalculateReportingHours(saveItem);
        saveItem.PreExamConsultation = reportingHours.PreExamConsultation;
        saveItem.TestHours = reportingHours.TestHours;
        saveItem.ExamHours = reportingHours.ExamHours;

        //saveItem.CheckingTestPaperHours = Math.Round(parsedRequest.StudentsCount / 3d, 1);
                
        return saveItem;
    }

    private (double PreExamConsultation, double TestHours, double ExamHours) CalculateReportingHours(
        RequestSaveItem saveItem)
    {
        var preExamConsultation = 0d;
        var testHours = 0d;
        var examHours = 0d;
        
        for (var i = 0; i < saveItem.Direction.Count; i++)
        {
            var budgetCount = saveItem.BudgetCount.GetAtOrFirst(i);
            var commercialCount = saveItem.CommercialCount.GetAtOrFirst(i);
            var reportingForm = saveItem.Reporting.GetAtOrFirst(i);
            
            if (reportingForm == ReportingForm.Exam)
            {
                preExamConsultation += 2;
                examHours += Math.Round((budgetCount + commercialCount) / 2d, 1);
            }
            else
            {
                testHours += Math.Round((budgetCount + commercialCount) / 3d, 1);
            }
        }
        return (preExamConsultation, testHours, examHours);
    }

    private RequestSaveItem BuildCommonFields(long disciplineId, ParsedRequest parsedRequest)
    {
        return new RequestSaveItem
        {
            DisciplineId = disciplineId,
            Direction = parsedRequest.Direction.SplitAndTrim(),
            Semester = parsedRequest.Semester.SplitAndParseToInt(),
            BudgetCount = parsedRequest.BudgetCount.SplitAndParseToInt(),
            CommercialCount = parsedRequest.CommercialCount.SplitAndParseToInt(),
            GroupForm = parsedRequest.GroupForm,
            Reporting = parsedRequest.Reporting.SplitAndParseToEnum<ReportingForm>(),
            Note = parsedRequest.Note,
            StudyForm = parsedRequest.StudyForm
        };
    }

    private RequestSaveItem BuildGac(ParsedRequest parsedRequest)
    {
        var disciplineId = _disciplineLogic.GetOrCreateDisciplineId(GAC_DISCIPLINE_NAME);
        var result = new RequestSaveItem
        {
            DisciplineId = disciplineId,
            Direction = parsedRequest.Direction.SplitAndTrim(),
            Semester = parsedRequest.Semester.SplitAndParseToInt(),
            BudgetCount = parsedRequest.BudgetCount.SplitAndParseToInt(),
            CommercialCount = parsedRequest.CommercialCount.SplitAndParseToInt(),
            GroupNumber = parsedRequest.GroupNumber.SplitAndParseToInt(),
            GroupForm = parsedRequest.GroupForm,
            LessonForm = LessonForm.Gac,
            Reporting = new List<ReportingForm> { ReportingForm.None },
            Note = parsedRequest.Note,
            StudyForm = parsedRequest.StudyForm
        };

        result.Gac = Math.Round(result.StudentsCount / 2d, 1);
        
        return result;
    }
}