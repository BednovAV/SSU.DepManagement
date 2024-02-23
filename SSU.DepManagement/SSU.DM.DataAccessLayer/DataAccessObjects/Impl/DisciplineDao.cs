using Microsoft.EntityFrameworkCore;
using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

public class DisciplineDao : BaseDao<Discipline, long>, IDisciplineDao
{
    public DisciplineDao(ApplicationContext ctx) : base(ctx)
    {
    }

    protected override DbSet<Discipline> SelectDbSet(ApplicationContext db)
        => db.Disciplines;

    protected override long SelectKey(Discipline entity)
        => entity.Id;
}