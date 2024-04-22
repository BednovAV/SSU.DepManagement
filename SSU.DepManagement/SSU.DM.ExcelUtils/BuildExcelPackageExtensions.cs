using OfficeOpenXml;
using SSU.DM.DataAccessLayer.DataAccessObjects;

namespace SSU.DM.ExcelUtils;

public static class BuildExcelPackageExtensions
{
    public static ExcelPackage ReadExcel(this IFilesStorageDao dao, string fileKey)
        => dao.GetById(fileKey).Bytes.ReadExcel();
    
    public static ExcelWorksheetEditor GetFirstWorksheetEditor(this ExcelPackage package)
        => new ExcelWorksheetEditor(package.Workbook.Worksheets.First());

    public static ExcelPackage ReadExcel(this byte[] bytes)
    {
        var stream = new MemoryStream(bytes);
        return new ExcelPackage(stream);
    }
}