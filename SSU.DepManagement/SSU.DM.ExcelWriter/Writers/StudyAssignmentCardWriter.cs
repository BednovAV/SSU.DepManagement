using Models.Request;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.ExcelUtils;
using SSU.DM.ExcelWriter.Abstract;
using SSU.DM.ExcelWriter.Internal;

namespace SSU.DM.ExcelWriter.Writers;

[DataWriter]
public class StudyAssignmentCardWriter : IWriter<StudyAssignmentCardData>
{
    private readonly IFilesStorageDao _filesStorage;

    public StudyAssignmentCardWriter(IFilesStorageDao filesStorageDao)
    {
        _filesStorage = filesStorageDao;
    }
    
    public byte[] GetExcel(StudyAssignmentCardData data)
    {
        using var templateDoc = _filesStorage.ReadExcel("???"); // TODO
        var template = templateDoc.GetFirstWorksheetEditor();

        template.ReplacePlaceholders(BuildPlaceholders(data));
        var startRow = template.GetLastUsedRow() + 1;
        var currentRow = startRow;
        foreach (var teacher in data.Teachers)
        {
            FillTeacher(template, teacher, ref currentRow);
        }

        return templateDoc.GetAsByteArray();
    }

    private void FillTeacher(
        ExcelWorksheetEditor template,
        StudyAssignmentTeacherData teacher,
        ref int currentRow)
    {
        var studyFormCount = teacher.HoursByStudyForm.Count;
        template.Merge($"A{currentRow}:A{currentRow + studyFormCount}", teacher.FioWithJobTitle);
    }

    private IDictionary<string, string> BuildPlaceholders(StudyAssignmentCardData data)
    {
        return new Dictionary<string, string>
        {
            ["studyYear"] = data.StudyYear,
        };
    }
}