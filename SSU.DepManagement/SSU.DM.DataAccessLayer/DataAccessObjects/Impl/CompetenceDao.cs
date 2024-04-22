using Microsoft.EntityFrameworkCore;
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
                competence.LessonForm))
            .ToHashSet());
    }

    public void SetForTeacher(long teacherId, IReadOnlyList<CompetenceShortInfo> competencies)
    {
        UseContext(db =>
        {
            db.Competencies.Where(competence => competence.TeacherId == teacherId).ExecuteDelete();
            db.Competencies.AddRange(competencies.Select(info => new Competence
            {
                TeacherId = teacherId,
                DisciplineId = info.DisciplineId,
                FacultyId = info.FacultyId,
                LessonForm = info.LessonForm
            }));
        });
    }
}