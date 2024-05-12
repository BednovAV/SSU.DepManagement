using Models.Request;
using OfficeOpenXml;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.ExcelUtils;
using SSU.DM.ExcelWriter.Abstract;
using SSU.DM.ExcelWriter.Internal;

namespace SSU.DM.ExcelWriter.Writers;

[DataWriter]
public class CalculationOfHoursWriter : IWriter<CalculationOfHoursData>
{
    private const string TEMPLATE_KEY = "CALCULATION_OF_HOURS_TEMPLATE";
    
    private readonly IFilesStorageDao _filesStorageDao;

    public CalculationOfHoursWriter(IFilesStorageDao filesStorageDao)
    {
        _filesStorageDao = filesStorageDao;
    }
    
    public ExcelPackage GetExcel(CalculationOfHoursData data)
    {
        using var excelPackage = new ExcelPackage();
        excelPackage.Workbook.Worksheets.Add("tmp");
        var worksheetEditor = excelPackage.GetFirstWorksheetEditor();
         
        var currentRow = 1;
        var map = StudyFormFiller.FillStudyForms(worksheetEditor, data.StudyForms, ref currentRow);
        
        var templateDoc = _filesStorageDao.ReadExcel(TEMPLATE_KEY);
        var templateEditor = templateDoc.GetFirstWorksheetEditor();
        templateEditor.ReplacePlaceholders(BuildPlaceholders(data));
        templateEditor.RenameSheet("p1");
        
        const string hoursMark = "insertHours";
        var hoursRow = templateEditor.GetRowByMark(hoursMark);
        if (hoursRow.HasValue)
        {
            var hoursRowsCount = currentRow;
            if (currentRow > 1)
            {
                templateEditor.InsertEmptyRows(hoursRow.Value, hoursRowsCount - 1);
            }
            templateEditor.CopyFrom(worksheetEditor, $"A{hoursRow}");
            
            var totalRow = hoursRow + hoursRowsCount - 1;
            templateEditor.SetValue($"A{totalRow}", "Итого по кафедре", bold: true, italic: true);
            for (var i = 'I'; i <= 'Z'; i++)
            {
                templateEditor.SetFormula($"{i}{totalRow}",
                    string.Join(" + ", map.StudyFormTotalRows.Select(x => $"{i}{x + hoursRow - 1}")),
                    bold: true,
                    italic: true);
            }
        }
        
        SetColumnsWidth(templateEditor);
        return templateDoc;
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
    
    private IDictionary<string, string> BuildPlaceholders(CalculationOfHoursData data)
    {
        return new Dictionary<string, string>
        {
            ["studyYear"] = data.StudyYear,
        };
    }
}