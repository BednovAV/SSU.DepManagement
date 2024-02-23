using Microsoft.EntityFrameworkCore;
using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

public class FilesStorageDao : BaseDao<SavedFile, string>, IFilesStorageDao
{
    public FilesStorageDao(ApplicationContext ctx)
        : base(ctx) { }
    
    protected override DbSet<SavedFile> SelectDbSet(ApplicationContext db)
        => db.SavedFiles;

    protected override string SelectKey(SavedFile entity)
        => entity.Key;
    
    public void Save(string key, string fileName, byte[] bytes)
    {
        var entity = new SavedFile()
        {
            Key = key,
            FileName = fileName,
            Bytes = bytes
        };
        Add(entity);
    }
}
