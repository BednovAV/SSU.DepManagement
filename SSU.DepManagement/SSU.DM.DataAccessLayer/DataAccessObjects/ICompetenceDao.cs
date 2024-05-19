using Models.Request;
using Models.View;
using SSU.DM.DataAccessLayer.Core.Interface;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects;

public interface ICompetenceDao : IDao<Competence, (long, long, int)>
{
    ISet<CompetenceShortInfo> GetTeacherCompetencies(long teacherId);
    
    void SetForTeacher(long teacherId, IReadOnlyList<CompetenceShortInfo> competencies, List<PriorityItem> priorityItems);

    void DeleteForTeacher(long teacherId);
}