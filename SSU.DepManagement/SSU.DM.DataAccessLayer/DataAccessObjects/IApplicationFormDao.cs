using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.Core.Interface;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects;

public interface IApplicationFormDao : IDao<ApplicationForm, Guid>
{
    void Add(Guid id, DateTimeOffset dateCreated, string fileKey);
    
    void SetFacultyId(Guid appFormId, int facultyId);
}