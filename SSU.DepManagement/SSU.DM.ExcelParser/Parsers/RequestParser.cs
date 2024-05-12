using Models.Request;
using OfficeOpenXml;
using SSU.DM.ExcelParser.Abstract;
using SSU.DM.ExcelUtils;

namespace SSU.DM.ExcelParser.Parsers;

internal class RequestParser : AbstractParserLite<List<ParsedRequest>>
{
    protected override List<ParsedRequest>? MapToResult(ExcelWorkbook workBook)
    {
        var workSheet = workBook.Worksheets.First();
        
        var fullTimeStartRow = workSheet.GetRowByText("Очная форма");
        var extramuralStartRow = workSheet.GetRowByText("Очно-заочная (вечерняя) форма");
        var partTimeStartRow = workSheet.GetRowByText("Заочная форма");
        
        var endRow = workSheet.GetLastUsedRow() + 1;
        var fullTimeEndRow = extramuralStartRow ?? partTimeStartRow ?? endRow;
        var extramuralEndRow = partTimeStartRow ?? endRow;
        var partTimeEndRow = endRow;
        

        var result = new List<ParsedRequest>();
        if (fullTimeStartRow.HasValue)
        {
            for (var i = fullTimeStartRow.Value + 1; i < fullTimeEndRow; i++)
            {
                result.Add(ReadRow(workSheet, i, StudyForm.FullTime));
            }
        }
        if (extramuralStartRow.HasValue)
        {
            for (var i = extramuralStartRow.Value + 1; i < extramuralEndRow; i++)
            {
                result.Add(ReadRow(workSheet, i, StudyForm.Extramural));
            }
        }
        if (partTimeStartRow.HasValue)
        {
            for (var i = partTimeStartRow.Value + 1; i < partTimeEndRow; i++)
            {
                result.Add(ReadRow(workSheet, i, StudyForm.PartTime));
            }
        }
        
        return result;
    }

    private ParsedRequest ReadRow(ExcelWorksheet workSheet, int rowNumber, StudyForm studyForm)
    {
        return new()
        {
            NameDiscipline = GetField<string>(workSheet.Cells[rowNumber, 1]),
            Direction = GetField<string>(workSheet.Cells[rowNumber, 2]),
            Semester = GetField<string>(workSheet.Cells[rowNumber, 3]),
            BudgetCount = GetField<string>(workSheet.Cells[rowNumber, 4]),
            CommercialCount = GetField<string>(workSheet.Cells[rowNumber, 5]),
            GroupNumber = GetField<string>(workSheet.Cells[rowNumber, 6]),
            GroupForm = GetField<string>(workSheet.Cells[rowNumber, 7]),
            TotalHours = GetField<int>(workSheet.Cells[rowNumber, 8]),
            LectureHours = GetField<int>(workSheet.Cells[rowNumber, 9]),
            PracticalHours = GetField<int>(workSheet.Cells[rowNumber, 10]),
            LaboratoryHours = GetField<int>(workSheet.Cells[rowNumber, 11]),
            IndependentWorkHours = GetField<string>(workSheet.Cells[rowNumber, 12]),
            Reporting = GetField<string>(workSheet.Cells[rowNumber, 13]),
            Note = GetField<string>(workSheet.Cells[rowNumber, 14]),
            StudyForm = studyForm
        };
    }
}