using Microsoft.EntityFrameworkCore;
using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

public class JobTitleDao : BaseDao<JobTitle, long>, IJobTitleDao
{
    public JobTitleDao(ApplicationContext ctx) : base(ctx)
    {
    }

    protected override DbSet<JobTitle> SelectDbSet(ApplicationContext db)
        => db.JobTitles;

    protected override long SelectKey(JobTitle entity)
        => entity.Id;
}