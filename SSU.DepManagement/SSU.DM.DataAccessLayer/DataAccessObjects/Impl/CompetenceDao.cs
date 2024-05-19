using Microsoft.EntityFrameworkCore;
using Models.Request;
using Models.View;
using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

public class CompetenceDao : BaseDao<Competence, (long, long, int)>, ICompetenceDao
{
    public CompetenceDao(ApplicationContext ctx) : base(ctx)
    {
    }

    protected override DbSet<Competence> SelectDbSet(ApplicationContext db)
        => db.Competencies;

    protected override (long, long, int) SelectKey(Competence entity)
        => (entity.TeacherId, entity.DisciplineId, entity.FacultyId);

    public ISet<CompetenceShortInfo> GetTeacherCompetencies(long teacherId)
    {
        return UseContext(db => db.Competencies
            .Where(competence => competence.TeacherId == teacherId)
            .Select(competence => new CompetenceShortInfo(
                competence.DisciplineId,
                competence.FacultyId,
                competence.LessonForm,
                competence.Priority!.Value))
            .ToHashSet());
    }

    public void SetForTeacher(long teacherId, IReadOnlyList<CompetenceShortInfo> competencies, List<PriorityItem> priorityItems)
    {
        var prioritiesByDisciplineId = priorityItems.ToLookup(x => x.DisciplineId);
        UseContext(db =>
        {
            db.Competencies.Where(competence => competence.TeacherId == teacherId).ExecuteDelete();
            db.Competencies.AddRange(competencies.Select(info => new Competence
            {
                TeacherId = teacherId,
                DisciplineId = info.DisciplineId,
                FacultyId = info.FacultyId,
                LessonForm = info.LessonForm,
                Priority = prioritiesByDisciplineId[info.DisciplineId].FirstOrDefault(x => x.LessonForm == info.LessonForm)?.Value,
            }));
        });
    }

    public void DeleteForTeacher(long teacherId)
    {
        UseContext(db => db.Competencies.Where(x => x.TeacherId == teacherId).ExecuteDelete());
    }
}