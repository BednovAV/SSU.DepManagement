using Microsoft.EntityFrameworkCore;
using Models.Request;
using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

public class RequestDao : BaseDao<Request, int>, IRequestDao
{
    public RequestDao(ApplicationContext ctx) : base(ctx)
    {
    }

    protected override DbSet<Request> SelectDbSet(ApplicationContext db)
        => db.Requests;

    public void AddRange(IEnumerable<RequestItem> requestItems, Guid applicationFormId)
        => UseContext(db => db.Requests
            .AddRange(requestItems.Select(x => Request.FromModel(x, applicationFormId))));
}