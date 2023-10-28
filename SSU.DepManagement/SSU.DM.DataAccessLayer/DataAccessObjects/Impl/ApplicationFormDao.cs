using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

public class ApplicationFormDao : BaseDao<ApplicationForm, Guid>, IApplicationFormDao
{
    public ApplicationFormDao(ApplicationContext ctx)
        : base(ctx) { }
    
    protected override DbSet<ApplicationForm> SelectDbSet(ApplicationContext db)
        => db.ApplicationForms;

    public void Add(Guid id, DateTimeOffset dateCreated, string fileKey)
    {
        var entity = new ApplicationForm()
        {
            ApplicationFormId = id,
            DateTimeCreated = dateCreated,
            FileKey = fileKey,
        };
        Add(entity);
    }
}