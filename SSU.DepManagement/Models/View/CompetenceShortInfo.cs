using Models.Request;

namespace Models.View;

public record CompetenceShortInfo(long DisciplineId, int FacultyId, LessonForm LessonForm, int Priority);