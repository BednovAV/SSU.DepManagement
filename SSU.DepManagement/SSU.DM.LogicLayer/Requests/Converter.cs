using Models.View;

namespace SSU.DM.LogicLayer.Requests;

public class RequestConverter
{
    public static RequestViewItem ConvertToViewItem(DataAccessLayer.DbEntities.Request request) =>
        new()
        {
            Id = request.Id,
            NameDiscipline = request.Discipline.Name,
            Direction = request.Direction,
            Semester = request.Semester,
            BudgetCount = request.BudgetCount,
            CommercialCount = request.CommercialCount,
            GroupNumber = request.GroupNumber,
            GroupForm = request.GroupForm,
            TotalHours = request.TotalHours,
            LectureHours = request.LectureHours,
            PracticalHours = request.PracticalHours,
            LaboratoryHours = request.LaboratoryHours,
            IndependentWorkHours = request.IndependentWorkHours,
            Reporting = request.Reporting.ToString(),
            Note = request.Note,
            Teacher = request.Teacher?.ToViewItem(),
            AvailableTeacherIds = request.Discipline.Competencies
                .Where(x => x.FacultyId == request.ApplicationForm.FacultyId)
                .Select(x => x.TeacherId)
                .ToHashSet()
        };
}