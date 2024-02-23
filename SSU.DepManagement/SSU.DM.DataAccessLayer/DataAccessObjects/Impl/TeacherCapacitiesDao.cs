using Microsoft.EntityFrameworkCore;
using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

public class TeacherCapacitiesDao : BaseDao<TeacherCapacity, (long, long)>, ITeacherCapacityDao
{
    public TeacherCapacitiesDao(ApplicationContext ctx) : base(ctx)
    {
    }

    protected override DbSet<TeacherCapacity> SelectDbSet(ApplicationContext db)
        => db.TeacherCapacities;

    protected override (long, long) SelectKey(TeacherCapacity entity)
        => (entity.TeacherId, entity.SemesterId);

    public ISet<long> GetTeacherSemesters(long teacherId)
    {
        return UseContext(db => db.TeacherCapacities
            .Where(capacity => capacity.TeacherId == teacherId)
            .Select(capacity => capacity.SemesterId)
            .ToHashSet());
    }
}