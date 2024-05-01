using OfficeOpenXml;

namespace SSU.DM.ExcelUtils;

public class WorksheetTools
{
    private static readonly ExcelPackage TmpPackage = new();
    private static int _worksheetsCnt = 0;
    
    public static ExcelWorksheetEditor CreateTmpWorksheet()
    {
        var sheetName = $"Tmp{_worksheetsCnt++}";
        TmpPackage.Workbook.Worksheets.Add(sheetName);
        var worksheet = TmpPackage.Workbook.Worksheets[sheetName];
        return new ExcelWorksheetEditor(worksheet);
    }
    
    public static byte[] GetTmpAsByteArray()
    {
        return TmpPackage.GetAsByteArray();
    }
}