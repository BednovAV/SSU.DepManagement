using Microsoft.EntityFrameworkCore;
using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

public class ApplicationFormDao : BaseDao<ApplicationForm, Guid>, IApplicationFormDao
{
    public ApplicationFormDao(ApplicationContext ctx)
        : base(ctx) { }
    
    protected override DbSet<ApplicationForm> SelectDbSet(ApplicationContext db)
        => db.ApplicationForms;

    protected override Guid SelectKey(ApplicationForm entity)
        => entity.ApplicationFormId;

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

    public void SetFacultyId(Guid appFormId, int facultyId)
    {
        var entity = GetById(appFormId);
        entity.FacultyId = facultyId;
        UseContext(ctx =>
        {
            ctx.ApplicationForms.Update(entity);
        });
    }
}