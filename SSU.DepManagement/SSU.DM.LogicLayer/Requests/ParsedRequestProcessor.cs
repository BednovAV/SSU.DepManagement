using Models.Extensions;
using Models.Request;
using SSU.DM.LogicLayer.Interfaces.Discipline;
using SSU.DM.LogicLayer.Interfaces.Request;

namespace SSU.DM.LogicLayer.Requests;

public class ParsedRequestProcessor : IParsedRequestProcessor
{
    private const string GAC_DISCIPLINE_NAME = "ГАК";
    private const string GEC_DISCIPLINE_NAME = "ГЭК";
    private const string COURSE_WORK_DISCIPLINE_NAME = "Курсовая работа";
    private const string NIR_DISCIPLINE_NAME = "Консультация по НИР";
    private const string VCR_DISCIPLINE_NAME = "ВКР";
    private const string MASTER_MANAGEMENT_DISCIPLINE_NAME = "Научное руководство магистратурой";
    private const string COMPUTING_PRACTICE_DISCIPLINE_NAME = "Вычислительная практика (учебная)";
    private const string TECHNOLOGICAL_PRACTICE_DISCIPLINE_NAME = "Технологическая практика (учебная)";
    private const string PRODUCTION_PRACTICE_DISCIPLINE_NAME = "Производственная практика";
    private const string PRE_GRADUATE_PRACTICE_DISCIPLINE_NAME = "Преддипломная практика (производственная)";
    private const string PEDAGOGICAL_PRODUCTION_PRACTICE_DISCIPLINE_NAME = "Педагогическая практика (производственная)";
    private const string PEDAGOGICAL_GRADUATE_PRACTICE_DISCIPLINE_NAME = "Педагогическая практика (аспирантура)";
    private const string RESEARCH_PRACTICE_DISCIPLINE_NAME = "Научно-исследовательская практика (производственная)";
    private const string PRODUCTION_PEDAGOGICAL_PRACTICE_DISCIPLINE_NAME = "Производственно-педагогическая практика (производственная)";
    
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
        if (parsedRequest.TotalHours != default
            && parsedRequest is { LectureHours: 0, PracticalHours: 0, LaboratoryHours: 0 })
        {
            return BuildOther(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(GAC_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildGac(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(GEC_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildGec(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(COURSE_WORK_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildCourseWork(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(NIR_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildNir(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(VCR_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildVcr(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(MASTER_MANAGEMENT_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildMasterManagement(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(COMPUTING_PRACTICE_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildComputingPractice(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(TECHNOLOGICAL_PRACTICE_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildTechnologicalPractice(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(PRODUCTION_PRACTICE_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildProductionPractice(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(PRE_GRADUATE_PRACTICE_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildPreGraduatePractice(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(PEDAGOGICAL_PRODUCTION_PRACTICE_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildPedagogicalProductionPractice(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(PEDAGOGICAL_GRADUATE_PRACTICE_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildPedagogicalGraduatePractice(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(RESEARCH_PRACTICE_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildResearchPractice(parsedRequest);
        }
        if (parsedRequest.NameDiscipline.Trim()
            .Equals(PRODUCTION_PEDAGOGICAL_PRACTICE_DISCIPLINE_NAME, StringComparison.InvariantCultureIgnoreCase))
        {
            return BuildProductionPedagogicalPractice(parsedRequest);
        }
        

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
        var result = BuildSpecialItem(parsedRequest, GAC_DISCIPLINE_NAME, LessonForm.Gac);

        result.Gac = Math.Round(result.StudentsCount / 2d, 1);
        return result;
    }

    private RequestSaveItem BuildGec(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, GEC_DISCIPLINE_NAME, LessonForm.Gec);

        result.Gac = Math.Round(result.StudentsCount / 2d, 1);
        return result;
    }
    
    private RequestSaveItem BuildCourseWork(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, COURSE_WORK_DISCIPLINE_NAME, LessonForm.CourseWork);
        
        var multiplier = (result.GroupNumber[0] / 100) switch
        {
            1 => 15,
            2 => 2,
            3 => 10,
            _ => 0,
        };
        result.CourseWork = result.StudentsCount * multiplier;
        return result;
    }
    
    private RequestSaveItem BuildNir(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, NIR_DISCIPLINE_NAME, LessonForm.Nir);

        result.MasterManagement = result.StudentsCount * 15;
        
        return result;
    }
    
    private RequestSaveItem BuildVcr(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, VCR_DISCIPLINE_NAME, LessonForm.Vcr);

        var multiplier = (result.GroupNumber[0] / 100) switch
        {
            2 => 34,
            4 => 24,
            _ => 0,
        };
        result.DiplomaWork = result.StudentsCount * multiplier;
        return result;
    }
    
    private RequestSaveItem BuildMasterManagement(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, MASTER_MANAGEMENT_DISCIPLINE_NAME, LessonForm.MasterManagement);
        result.MasterManagement = 30;
        return result;
    }
    
    private RequestSaveItem BuildComputingPractice(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, COMPUTING_PRACTICE_DISCIPLINE_NAME, LessonForm.ComputingPractice);
        result.PracticeManagement = 24;
        return result;
    }
    
    private RequestSaveItem BuildTechnologicalPractice(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, TECHNOLOGICAL_PRACTICE_DISCIPLINE_NAME, LessonForm.TechnologicalPractice);
        result.PracticeManagement = 24;
        return result;
    }
    
    private RequestSaveItem BuildProductionPractice(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, PRODUCTION_PRACTICE_DISCIPLINE_NAME, LessonForm.ProductionPractice);
        result.PracticeManagement = result.StudentsCount * 2;
        return result;
    }
    
    private RequestSaveItem BuildPreGraduatePractice(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, PRE_GRADUATE_PRACTICE_DISCIPLINE_NAME, LessonForm.PreGraduatePractice);
        var multiplier = (result.GroupNumber[0] / 100) switch
        {
            2 => 12,
            4 => 2,
            _ => 0,
        };
        
        result.PracticeManagement = result.StudentsCount * multiplier;
        return result;
    }
    
    private RequestSaveItem BuildPedagogicalProductionPractice(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, PEDAGOGICAL_PRODUCTION_PRACTICE_DISCIPLINE_NAME, LessonForm.PedagogicalProductionPractice);
        result.PracticeManagement = result.StudentsCount * 8;
        return result;
    }
    
    private RequestSaveItem BuildPedagogicalGraduatePractice(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, PEDAGOGICAL_GRADUATE_PRACTICE_DISCIPLINE_NAME, LessonForm.PedagogicalGraduatePractice);
        result.PracticeManagement = Math.Round(result.StudentsCount * 0.5d * 6.2d, 1);
        return result;
    }
    
    private RequestSaveItem BuildResearchPractice(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, RESEARCH_PRACTICE_DISCIPLINE_NAME, LessonForm.ResearchPractice);
        result.PracticeManagement = result.StudentsCount * 8;
        return result;
    }
    
    private RequestSaveItem BuildProductionPedagogicalPractice(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, PRODUCTION_PEDAGOGICAL_PRACTICE_DISCIPLINE_NAME, LessonForm.ProductionPedagogicalPractice);
        result.PracticeManagement = result.StudentsCount * 4;
        return result;
    }
    
    private RequestSaveItem BuildOther(ParsedRequest parsedRequest)
    {
        var result = BuildSpecialItem(parsedRequest, parsedRequest.NameDiscipline, LessonForm.Other);
        result.PracticeManagement = parsedRequest.TotalHours;
        return result;
    }
    
    private RequestSaveItem BuildSpecialItem(
        ParsedRequest parsedRequest,
        string disciplineName,
        LessonForm lessonForm)
    {
        var disciplineId = _disciplineLogic.GetOrCreateDisciplineId(disciplineName);

        try
        {
            return new RequestSaveItem
            {
                DisciplineId = disciplineId,
                Direction = parsedRequest.Direction.SplitAndTrim(),
                Semester = parsedRequest.Semester.SplitAndParseToInt(),
                BudgetCount = parsedRequest.BudgetCount.SplitAndParseToInt(),
                CommercialCount = parsedRequest.CommercialCount.SplitAndParseToInt(),
                GroupNumber = parsedRequest.GroupNumber.SplitAndParseToInt(),
                GroupForm = parsedRequest.GroupForm,
                LessonForm = lessonForm,
                Reporting = new List<ReportingForm> { ReportingForm.None },
                Note = parsedRequest.Note,
                StudyForm = parsedRequest.StudyForm
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}