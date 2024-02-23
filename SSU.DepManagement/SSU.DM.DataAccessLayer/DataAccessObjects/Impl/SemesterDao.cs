using Microsoft.EntityFrameworkCore;
using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

public class SemesterDao : BaseDao<Semester, long>, ISemesterDao
{
    public SemesterDao(ApplicationContext ctx) : base(ctx)
    {
    }

    protected override DbSet<Semester> SelectDbSet(ApplicationContext db)
        => db.Semesters;

    protected override long SelectKey(Semester entity)
        => entity.Id;
}