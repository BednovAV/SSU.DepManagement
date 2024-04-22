using Models.Request;
using SSU.DM.ExcelUtils;

namespace SSU.DM.ExcelWriter.Writers;

public static class FacultyFiller
{
    public static void FillFaculties(
        ExcelWorksheetEditor worksheet,
        IReadOnlyList<FacultyData> faculties,
        ref int currentRow)
    {
        worksheet.Merge($"A{currentRow}:Z{currentRow}", "Очная форма обучения", italic: true);
        currentRow++;

        foreach (var faculty in faculties)
        {
            FillFaculty(worksheet, faculty, ref currentRow);
        }
    }
    
    private static void FillFaculty(
        ExcelWorksheetEditor worksheet,
        FacultyData faculty,
        ref int currentRow)
    {
        worksheet.Merge($"A{currentRow}:Z{currentRow}", faculty.Name, italic: true, bold: true);
        worksheet.SetStandardTableStyle($"A{currentRow}:Z{currentRow + faculty.Requests.Count + 1}");
        foreach (var request in faculty.Requests)
        {
            FillRequests(worksheet, request, ++currentRow);
        }
        currentRow++;
        FillSum(worksheet, faculty, currentRow++);
    }

    private static void FillSum(
        ExcelWorksheetEditor worksheet,
        FacultyData faculty,
        int row)
    {
        worksheet.Merge($"A{row}:H{row}");
        worksheet.SetValue($"A{row}", $"Итого по {faculty.NameDative}", bold: true, italic: true);
        for (var i = 'I'; i <= 'Z'; i++)
        {
            FillColumnSum(worksheet, faculty.Requests.Count, row, i.ToString());
        }
    }

    private static void FillColumnSum(
        ExcelWorksheetEditor worksheet,
        int requestsCount,
        int row,
        string column)
    {
        worksheet.SetFormula($"{column}{row}",
            $"SUM({column}{row - requestsCount}:{column}{row - 1})");
    }

    private static void FillRequests(
        ExcelWorksheetEditor worksheet,
        RequestReportData request,
        int row)
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

    private static void FillHourCounts(
        ExcelWorksheetEditor worksheet,
        RequestReportData request,
        int row)
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
        worksheet.SetFormula($"S{row}", $"ROUND(E{row}/2;1)");
        worksheet.SetFormula($"T{row}", $"ROUND(E{row}/2;1)");
        worksheet.SetFormula($"U{row}", $"");
        worksheet.SetFormula($"V{row}", $"");
        worksheet.SetFormula($"W{row}", $"E{row}*{hourCounts.MastersProgramManagement}");
        worksheet.SetFormula($"X{row}", $"");
        worksheet.SetFormula($"Y{row}", $"");
        worksheet.SetFormula($"Z{row}", $"SUM(I{row}:Y{row})");
    }
}