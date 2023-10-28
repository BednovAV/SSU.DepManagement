using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.Core.Interface;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects;

public interface IFilesStorageDao : IDao<SavedFile, string>
{
    void Save(string key, string fileName, byte[] bytes);
}
