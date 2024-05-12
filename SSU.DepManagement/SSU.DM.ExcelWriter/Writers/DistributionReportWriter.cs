using Microsoft.VisualBasic;
using Models.Request;
using OfficeOpenXml;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.ExcelUtils;
using SSU.DM.ExcelWriter.Abstract;
using SSU.DM.ExcelWriter.Internal;

namespace SSU.DM.ExcelWriter.Writers;

[DataWriter]
public class DistributionReportWriter : IWriter<DistributionReportData>
{
    private readonly IFilesStorageDao _filesStorage;

    public DistributionReportWriter(IFilesStorageDao filesStorageDao)
    {
        _filesStorage = filesStorageDao;
    }

    public ExcelPackage GetExcel(DistributionReportData data)
    {
        var excelPackage = new ExcelPackage();
        excelPackage.Workbook.Worksheets.Add("c1");
        var worksheetEditor = excelPackage.GetFirstWorksheetEditor();
        
        var currentRow = 1;
        foreach (var teacher in data.Teachers)
        {
            FillTeacher(worksheetEditor, teacher, ref currentRow);
        }
        
        SetColumnsWidth(worksheetEditor);
        
        return excelPackage;
    }
    
    private void SetColumnsWidth(ExcelWorksheetEditor excelWorksheetEditor)
    {
        excelWorksheetEditor.SetColumnWidth(1, 27.4d);
        excelWorksheetEditor.SetColumnWidth(2, 15d);
        for (int i = 3; i <= 25; i++)
        {
            excelWorksheetEditor.SetColumnWidth(i, 5);
        }
        excelWorksheetEditor.SetColumnWidth(26, 10);
    }

    private void FillTeacher(
        ExcelWorksheetEditor worksheet,
        TeacherData teacherData,
        ref int currentRow)
    {
        using var templateDoc = _filesStorage.ReadExcel("DISTRIBUTION_REPORT_TEMPLATE");
        var template = templateDoc.GetFirstWorksheetEditor();
        
        template.ReplacePlaceholders(BuildPlaceholders(teacherData));
        FillSemester(template, teacherData.FirstSemester, "facultiesFirstSemester");
        FillSemester(template, teacherData.SecondSemester, "facultiesSecondSemester");
        worksheet.CopyFrom(template, $"A{currentRow}", true);

        currentRow += template.FilledRowsCount() + 3;
    }

    private static void FillSemester(
        ExcelWorksheetEditor worksheet,
        List<StudyFormData> semesterData,
        string semesterMark)
    {
        var tmpWorksheet = WorksheetTools.CreateTmpWorksheet();
        var tmpCurrentRow = 1;
        var map = StudyFormFiller.FillStudyForms(tmpWorksheet, semesterData, ref tmpCurrentRow);
        var firstSemesterRow = worksheet.GetRowByMark(semesterMark);
        if (firstSemesterRow.HasValue)
        {
            var facultiesRows = tmpCurrentRow - 2;
            if (facultiesRows > 1)
            {
                worksheet.InsertEmptyRows(firstSemesterRow.Value, facultiesRows);
            }
            worksheet.CopyFrom(tmpWorksheet, $"A{firstSemesterRow}", true);
            for (var i = 'I'; i <= 'Z'; i++)
            {
                worksheet.SetFormula($"{i}{firstSemesterRow + facultiesRows + 1}",
                    string.Join(" + ", map.StudyFormTotalRows.Select(x => $"{i}{x + firstSemesterRow - 1}")),
                    bold: true);
            }
        }
        

    }

    private IDictionary<string, string> BuildPlaceholders(TeacherData teacherData)
    {
        return new Dictionary<string, string>
        {
            ["teacherName"] = teacherData.TeacherName,
            ["studyYear"] = teacherData.StudyYear,
            ["budgetForm"] = teacherData.BudgetForm,
            ["jobTitle"] = teacherData.JobTitle,
            ["rate"] = teacherData.Rate,
        };
    }
}