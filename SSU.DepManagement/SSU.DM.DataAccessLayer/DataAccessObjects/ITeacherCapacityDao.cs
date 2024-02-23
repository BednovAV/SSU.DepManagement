using SSU.DM.DataAccessLayer.Core.Interface;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects;

public interface ITeacherCapacityDao : IDao<TeacherCapacity, (long, long)>
{
    ISet<long> GetTeacherSemesters(long teacherId);
}