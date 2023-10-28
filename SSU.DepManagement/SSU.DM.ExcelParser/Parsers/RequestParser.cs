using Models.Request;
using OfficeOpenXml;
using SSU.DM.ExcelParser.Abstract;

namespace SSU.DM.ExcelParser.Parsers;

internal class RequestParser : AbstractParserLite<List<RequestItem>>
{
    protected override List<RequestItem>? MapToResult(ExcelWorkbook workBook)
    {
        var workSheet = workBook.Worksheets.FirstOrDefault();
        var endRow = 5;
        while (workSheet?.GetValue<string>(endRow, 1) != default)
            endRow++;
        
        return Enumerable.Range(5, endRow - 5).Select(x => ReadRow(workSheet, x)).ToList();
    }

    private RequestItem ReadRow(ExcelWorksheet workSheet, int rowNumber)
    {
        return new()
        {
            NameDiscipline = GetField<string>(workSheet.Cells[rowNumber, 1]),
            Direction = GetField<string>(workSheet.Cells[rowNumber, 2]),
            Semester = GetField<int>(workSheet.Cells[rowNumber, 3]),
            BudgetCount = GetField<int>(workSheet.Cells[rowNumber, 4]),
            CommercialCount = GetField<int>(workSheet.Cells[rowNumber, 5]),
            GroupNumber = GetField<string>(workSheet.Cells[rowNumber, 6]),
            GroupForm = GetField<string>(workSheet.Cells[rowNumber, 7]),
            TotalHours = GetField<int>(workSheet.Cells[rowNumber, 8]),
            LectureHours = GetField<int>(workSheet.Cells[rowNumber, 9]),
            PracticalHours = GetField<int>(workSheet.Cells[rowNumber, 10]),
            LaboratoryHours = GetField<int>(workSheet.Cells[rowNumber, 11]),
            IndependentWorkHours = GetField<int>(workSheet.Cells[rowNumber, 12]),
            Reporting = GetField<ReportingForm>(workSheet.Cells[rowNumber, 13]),
            Note = GetField<string>(workSheet.Cells[rowNumber, 14]),
        };
    }
}
