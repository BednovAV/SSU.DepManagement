using Microsoft.EntityFrameworkCore;
using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

public class TeacherDao : BaseDao<Teacher, long>, ITeachersDao
{
    public TeacherDao(ApplicationContext ctx) : base(ctx)
    {
    }

    protected override DbSet<Teacher> SelectDbSet(ApplicationContext db) => db.Teachers;

    protected override long SelectKey(Teacher entity)
        => entity.Id;
}