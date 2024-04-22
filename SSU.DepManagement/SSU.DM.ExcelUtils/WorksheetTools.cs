using OfficeOpenXml;

namespace SSU.DM.ExcelUtils;

public class WorksheetTools
{
    private static readonly ExcelWorkbook TmpWorkbook = new ExcelPackage().Workbook;
    private static int _worksheetsCnt = 0;
    
    public static ExcelWorksheetEditor CreateTmpWorksheet()
    {
        var sheetName = $"Tmp{_worksheetsCnt++}";
        TmpWorkbook.Worksheets.Add(sheetName);
        var worksheet = TmpWorkbook.Worksheets[sheetName];
        return new ExcelWorksheetEditor(worksheet);
    }
}