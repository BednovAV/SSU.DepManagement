using Models.Request;
using Models.View;
using SSU.DM.WebAssembly.Shared.Models;

namespace SSU.DM.LogicLayer.Interfaces.Competencies;

public interface ICompetenceLogic
{
    IDictionary<LessonForm, IReadOnlyList<TeacherCompetenciesViewItem>> GetTeacherCompetencies(long teacherId);
    
    void SaveTeacherCompetencies(long teacherId, IReadOnlyList<CompetenceShortInfo> competencies,
        List<PriorityItem> priorities);
}