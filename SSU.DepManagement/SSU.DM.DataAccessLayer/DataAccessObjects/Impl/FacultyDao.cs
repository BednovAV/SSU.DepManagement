using Microsoft.EntityFrameworkCore;
using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

public class FacultyDao : BaseDao<Faculty, int>, IFacultyDao
{
    public FacultyDao(ApplicationContext ctx) : base(ctx)
    {
    }

    protected override DbSet<Faculty> SelectDbSet(ApplicationContext db) => db.Faculties;

    protected override int SelectKey(Faculty entity)
        => entity.Id;
}