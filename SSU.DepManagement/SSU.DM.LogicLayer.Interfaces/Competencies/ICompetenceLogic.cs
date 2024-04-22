using Models.Request;
using Models.View;

namespace SSU.DM.LogicLayer.Interfaces.Competencies;

public interface ICompetenceLogic
{
    IDictionary<LessonForm, IReadOnlyList<TeacherCompetenciesViewItem>> GetTeacherCompetencies(long teacherId);
    
    void SaveTeacherCompetencies(long teacherId, IReadOnlyList<CompetenceShortInfo> competencies);
}