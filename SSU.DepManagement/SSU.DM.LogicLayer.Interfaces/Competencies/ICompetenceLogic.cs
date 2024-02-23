using Models.View;

namespace SSU.DM.LogicLayer.Interfaces.Competencies;

public interface ICompetenceLogic
{
    IReadOnlyList<TeacherCompetenciesViewItem> GetTeacherCompetencies(long teacherId);
    
    void SaveTeacherCompetencies(long teacherId, IReadOnlyList<CompetenceShortInfo> competencies);
}