using Models.Request;
using OfficeOpenXml.Style;
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

    public byte[] GetExcel(CalculationOfHoursData data)
    {
        using var excelDoc = _filesStorageDao.ReadExcel("CALCULATION_OF_HOURS_TEMPLATE");
        var workBook = excelDoc.Workbook;
        var firstWorksheet = new ExcelWorksheetEditor(workBook.Worksheets.FirstOrDefault());
        var currentRow = 11;
        firstWorksheet.Merge($"A{currentRow}:Z{currentRow}", "Очная форма обучения", italic: true);
        currentRow++;
        
        foreach (var faculty in data.Faculties)
        {
            FillFaculty(firstWorksheet, ref currentRow, faculty);
        }

        return excelDoc.GetAsByteArray();
    }

    private void FillFaculty(ExcelWorksheetEditor worksheet, ref int currentRow, FacultyData faculty)
    {
        worksheet.Merge($"A{currentRow}:Z{currentRow}", faculty.Name, italic: true, bold: true);
        worksheet.SetStandardTableStyle($"A{currentRow}:Z{currentRow + faculty.Requests.Count + 1}");
        foreach (var request in faculty.Requests)
        {
            FillRequests(worksheet, request, ++currentRow);
        }
        currentRow++;
        FillSum(worksheet, faculty, currentRow++);
        worksheet.Merge($"A{currentRow}:Z{currentRow}");
        currentRow++;
    }

    private void FillSum(ExcelWorksheetEditor worksheet, FacultyData faculty, int currentRow)
    {
        worksheet.SetValue($"A{currentRow}", $"Итого по {faculty.NameDative}", bold: true, italic: true);
        worksheet.Merge($"B{currentRow}:H{currentRow}");
        for (var i = 'I'; i <= 'Z'; i++)
        {
            FillColumnSum(worksheet, faculty.Requests.Count, currentRow, i.ToString());
        }
    }

    private static void FillColumnSum(ExcelWorksheetEditor worksheet, int requestsCount, int currentRow, string column)
    {
        worksheet.SetFormula($"{column}{currentRow}",
            $"SUM({column}{currentRow - requestsCount}:{column}{currentRow - 1})");
    }

    private void FillRequests(ExcelWorksheetEditor worksheet, RequestReportData request, int row)
    {
        worksheet.SetValue($"A{row}", request.DisciplineName);
        worksheet.SetValue($"B{row}", request.DirectionName);
        worksheet.SetValueRightAlignment($"C{row}", request.CourseNumber);
        worksheet.SetValueRightAlignment($"D{row}", request.Semester);
        worksheet.SetValueRightAlignment($"E{row}", request.StudentsCount);
        worksheet.SetValueRightAlignment($"F{row}", request.TreadsCount);
        worksheet.SetValueRightAlignment($"G{row}", request.GroupsCount);
        worksheet.SetValueRightAlignment($"H{row}", request.IndependentWorkHours);

        FillHourCounts(worksheet, request, row);
    }

    private void FillHourCounts(ExcelWorksheetEditor worksheet, RequestReportData request, int row)
    {
        var hourCounts = request.HourCounts;
        worksheet.SetValueRightAlignment($"I{row}", request.HourCounts.Lectures);
        worksheet.SetFormula($"J{row}", $"{hourCounts.Practices}*F{row}");
        worksheet.SetFormula($"K{row}", $"{hourCounts.Laboratory}*G{row}");
        worksheet.SetFormula($"L{row}", $"ROUND(H{row}*F{row}*5%;1)");
        if (request.ReportingForm is ReportingForm.Exam)
        {
            worksheet.SetFormula($"M{row}", $"2*F{row}");
            worksheet.SetFormula($"N{row}", $"ROUND(E{row}/2;1)");
        }
        else
        {
            worksheet.SetFormula($"O{row}", $"ROUND(E{row}/3;1)");
        }
        worksheet.SetFormula($"P{row}", $"E{row}*{hourCounts.PracticeManagement}");
        worksheet.SetFormula($"Q{row}", $"E{row}*{hourCounts.CourseWorks}");
        worksheet.SetFormula($"R{row}", $"E{row}*{hourCounts.QualificationWorks}");
        worksheet.SetFormula($"S{row}", $"ROUND({hourCounts.Gac}E{row}/2;1)");
        worksheet.SetFormula($"T{row}", $"ROUND(E{row}/2;1)");
        worksheet.SetFormula($"U{row}", $"");
        worksheet.SetFormula($"V{row}", $"");
        worksheet.SetFormula($"W{row}", $"E{row}*{hourCounts.MastersProgramManagement}");
        worksheet.SetFormula($"X{row}", $"");
        worksheet.SetFormula($"Y{row}", $"");
        worksheet.SetFormula($"Z{row}", $"SUM(I{row}:Y{row})");
    }
}