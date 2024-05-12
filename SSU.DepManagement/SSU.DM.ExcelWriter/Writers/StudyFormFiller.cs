using Models.Request;
using SSU.DM.ExcelUtils;

namespace SSU.DM.ExcelWriter.Writers;

public static class StudyFormFiller
{
    public static FilledFacultiesMap FillStudyForms(
        ExcelWorksheetEditor worksheet,
        IReadOnlyList<StudyFormData> studyForms,
        ref int currentRow)
    {
        var resultMap = new FilledFacultiesMap();

        foreach (var studyForm in studyForms)
        {
            worksheet.Merge($"A{currentRow}:Z{currentRow}", GetStudyFormString(studyForm.StudyForm), italic: true);
            currentRow++;

            var facultyTotalRows = new List<int>();
            foreach (var faculty in studyForm.Faculties)
            {
                FillFaculty(worksheet, faculty, ref currentRow);
                facultyTotalRows.Add(currentRow - 1);
            }
            
            worksheet.SetValue($"A{currentRow}", GetStudyFormTotalString(studyForm.StudyForm), bold: true, italic: true);
            for (var i = 'I'; i <= 'Z'; i++)
            {
                worksheet.SetFormula($"{i}{currentRow}",
                    string.Join(" + ", facultyTotalRows.Select(x => $"{i}{x}")),
                    bold: true,
                    italic: true);
            }

            resultMap.StudyFormTotalRows.Add(currentRow++);
        }
        
        return resultMap;
    }

    private static string? GetStudyFormString(StudyForm studyForm)
    {
        return studyForm switch
        {
            StudyForm.FullTime => "Очная форма обучения",
            StudyForm.Extramural => "Очно-заочная форма",
            StudyForm.PartTime => "Заочная форма",
            _ => throw new ArgumentOutOfRangeException(nameof(studyForm), studyForm, null)
        };
    }

    private static object? GetStudyFormTotalString(StudyForm studyForm)
    {
        return studyForm switch
        {
            StudyForm.FullTime => "Итого по очной форме",
            StudyForm.Extramural => "Итого по очно-заочной форме",
            StudyForm.PartTime => "Итого по заочной форме",
            _ => throw new ArgumentOutOfRangeException(nameof(studyForm), studyForm, null)
        };
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
        if (request.HourCounts.Lectures.HasValue)
        {
            worksheet.SetValueRightAlignment($"I{row}", request.HourCounts.Lectures);
        }

        if (hourCounts.Practices != default)
        {
            worksheet.SetValueRightAlignment($"J{row}", hourCounts.Practices);
        }

        if (hourCounts.Laboratory != default)
        {
            worksheet.SetValueRightAlignment($"K{row}", hourCounts.Laboratory);
        }
        worksheet.SetFormula($"L{row}", $"ROUND(H{row}*F{row}*5%;1)");
        if (request.ReportingForm is ReportingForm.Exam)
        {
            worksheet.SetValueRightAlignment($"M{row}", request.PreExamConsultation);
        }

        if (Math.Abs(request.ExamHours - default(double)) > 0.001d)
        {
            worksheet.SetValueRightAlignment($"N{row}", request.ExamHours);
        }

        if (Math.Abs(request.TestHours - default(double)) > 0.001d)
        {
            worksheet.SetValueRightAlignment($"O{row}", request.TestHours);
        }
        
        // worksheet.SetFormula($"P{row}", $"E{row}*{hourCounts.PracticeManagement}");
        // worksheet.SetFormula($"Q{row}", $"E{row}*{hourCounts.CourseWorks}");
        // worksheet.SetFormula($"R{row}", $"E{row}*{hourCounts.QualificationWorks}");
        //worksheet.SetFormula($"S{row}", $"ROUND(E{row}/2;1)");
        if (request.HasTestPaper)
        {
            worksheet.SetFormula($"T{row}", $"ROUND(E{row}/2;1)");
        }
        worksheet.SetFormula($"U{row}", $"");
        worksheet.SetFormula($"V{row}", $"");
        //worksheet.SetFormula($"W{row}", $"E{row}*{hourCounts.MastersProgramManagement}");
        worksheet.SetFormula($"X{row}", $"");
        worksheet.SetFormula($"Y{row}", $"");
        
        worksheet.SetFormula($"Z{row}", $"SUM(I{row}:Y{row})");
    }
}

public class FilledFacultiesMap
{
    public List<int> StudyFormTotalRows { get; set; } = new();
}