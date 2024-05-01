using Models.Request;
using Models.View;

namespace SSU.DM.LogicLayer.Requests;

public class RequestConverter
{
    public static RequestViewItem MapToRequestViewItem(DataAccessLayer.DbEntities.Request request)
    {
        return new RequestViewItem
        {
            Id = request.Id,
            Direction = string.Join(", ", request.Direction),
            GroupNumber = request.GroupNumberString,
            TotalHours = request.TotalHours,// TODO
            LessonForm = GetLessonFromName(request.LessonForm!.Value),
            Teacher = request.Teacher?.ToViewItem(),
            AvailableTeacherIds = request.Discipline.Competencies
                .Where(x => x.FacultyId == request.ApplicationForm.FacultyId && x.LessonForm == request.LessonForm)
                .Select(x => x.TeacherId)
                .ToHashSet()
        };
    }
    
    private static string GetLessonFromName(LessonForm lessonForm)
    {
        return lessonForm switch
        {
            LessonForm.Lecture => "Лекции",
            LessonForm.Practical => "Практики",
            LessonForm.Laboratory => "Лабораторные",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}