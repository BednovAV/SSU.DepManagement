using SSU.DM.DataAccessLayer.Core;
using Models.Request;
using SSU.DM.DataAccessLayer.Core.Interface;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects;

public interface IRequestDao : IDao<Request, int>
{
    void AddRange(IEnumerable<RequestItem> requestItems, Guid applicationFormId);
}