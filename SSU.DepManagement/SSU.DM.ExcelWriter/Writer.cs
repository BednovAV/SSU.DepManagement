using OfficeOpenXml;
using SSU.DM.ExcelWriter.Abstract;
using SSU.DM.Tools.Interface;
using SSU.DM.Tools.Interface.Models;

namespace SSU.DM.ExcelWriter;

public class Writer : IExcelWriter
{
    private readonly IReadOnlyList<object?> _writers;

    public Writer(IReadOnlyList<object?> writers)
    {
        _writers = writers;
    }

    public WriteResult Write<T>(T data) where T : new()
    {
        var writer = _writers.FirstOrDefault(x => x is IWriter<T>);
        if (writer == null)
        {
            return new WriteResult
            {
                ErrorType = WriteErrorType.WriterNotFound
            };
        }

        using var package = (writer as IWriter<T>).GetExcel(data);
        var resultPackage = new ExcelPackage();
        foreach (var worksheet in package.Workbook.Worksheets)
        {
            resultPackage.Workbook.Worksheets.Add(worksheet.Name, worksheet);
        }
        
        return new WriteResult
        {
            IsSucceeded = true,
            FileBytes = resultPackage.GetAsByteArray()
        } ;
    }
}