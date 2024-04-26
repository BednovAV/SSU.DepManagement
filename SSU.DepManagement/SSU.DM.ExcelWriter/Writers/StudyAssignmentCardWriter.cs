using Models.Request;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.ExcelUtils;
using SSU.DM.ExcelWriter.Abstract;
using SSU.DM.ExcelWriter.Internal;

namespace SSU.DM.ExcelWriter.Writers;

[DataWriter]
public class StudyAssignmentCardWriter : IWriter<StudyAssignmentCardData>
{
    private readonly IFilesStorageDao _filesStorage;

    public StudyAssignmentCardWriter(IFilesStorageDao filesStorageDao)
    {
        _filesStorage = filesStorageDao;
    }
    
    public byte[] GetExcel(StudyAssignmentCardData data)
    {
        var tmpWorksheet = WorksheetTools.CreateTmpWorksheet();
        var currentRow = 1;
        var teacherSumRows = new List<int>();
        foreach (var teacher in data.Teachers)
        {
            FillTeacher(tmpWorksheet, teacher, ref currentRow);
            teacherSumRows.Add(currentRow);
        }
        tmpWorksheet.SetValue($"A{currentRow}", "Итого по кафедре", bold: true);
        for (var currentColumn = 'C'; currentColumn <= 'T'; currentColumn++)
        {
            tmpWorksheet.SetFormula($"{currentColumn}{currentRow}", string.Join('+', teacherSumRows));
        }

        using var templateDoc = _filesStorage.ReadExcel("???"); // TODO
        var template = templateDoc.GetFirstWorksheetEditor();

        template.ReplacePlaceholders(BuildPlaceholders(data));

        const string hoursMark = "???";
        var hoursRow = template.GetRowByMark(hoursMark);
        if (hoursRow.HasValue)
        {
            var hoursRowsCount = currentRow;
            if (currentRow > 1)
            {
                template.InsertEmptyRows(hoursRow.Value, hoursRowsCount);
            }
            template.CopyFrom(tmpWorksheet, $"A{hoursRow}");
        }

        return templateDoc.GetAsByteArray();
    }

    private void FillTeacher(
        ExcelWorksheetEditor template,
        StudyAssignmentTeacherData teacher,
        ref int currentRow)
    {
        var teacherFirstRow = currentRow;

        var studyFormCount = teacher.HoursByStudyForm.Count;
        template.Merge($"A{currentRow}:A{currentRow + studyFormCount}", teacher.FioWithJobTitle);
        foreach (var studyForm in teacher.HoursByStudyForm)
        {
            template.SetValue($"B{currentRow}", studyForm.Key);
            FillTeacherStudyFormHours(template, currentRow++, studyForm.Value);
        }

        template.SetValue($"A{currentRow}", "Всего", bold: true, italic: true);
        for (var currentColumn = 'C'; currentColumn <= 'T'; currentColumn++)
        {
            template.SetFormula($"{currentColumn}{currentRow}",
                $"SUM({currentColumn}{teacherFirstRow}:{currentColumn}{currentColumn - 1})", bold: true);
        }
        currentRow++;
    }

    private void FillTeacherStudyFormHours(
        ExcelWorksheetEditor template,
        int currentRow,
        StudyAssignmentHours studyFormHours)
    {
        template.SetValue($"C{currentRow}", studyFormHours.Lectures);
        template.SetValue($"D{currentRow}", studyFormHours.Practicals);
        template.SetValue($"E{currentRow}", studyFormHours.Laboratory);
        template.SetValue($"F{currentRow}", studyFormHours.ControlOfIndependentWork);
        template.SetValue($"G{currentRow}", studyFormHours.PreExamConsultation);
        template.SetValue($"H{currentRow}", studyFormHours.Exam);
        template.SetValue($"I{currentRow}", studyFormHours.Test);
        template.SetValue($"J{currentRow}", studyFormHours.PracticeManagement);
        template.SetValue($"K{currentRow}", studyFormHours.CourseWork);
        template.SetValue($"L{currentRow}", studyFormHours.DiplomaWork);
        template.SetValue($"M{currentRow}", studyFormHours.Gac);
        template.SetValue($"N{currentRow}", studyFormHours.CheckingTestPaperHours);
        template.SetValue($"O{currentRow}", studyFormHours.AspirantManagement);
        template.SetValue($"P{currentRow}", studyFormHours.ApplicantManagement);
        template.SetValue($"Q{currentRow}", studyFormHours.MasterManagement);
        template.SetValue($"R{currentRow}", studyFormHours.ExtracurricularActivity);

        template.SetFormula($"T{currentRow}", $"SUM(C{currentRow}: S{currentRow})");
    }

    private IDictionary<string, string> BuildPlaceholders(StudyAssignmentCardData data)
    {
        return new Dictionary<string, string>
        {
            ["studyYear"] = data.StudyYear,
        };
    }
}