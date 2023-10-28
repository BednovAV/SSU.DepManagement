namespace SSU.DM.DataAccessLayer.Core.Interface;

public interface IDao<TEntity, in TKey> where TEntity : class
{
    void Add(TEntity entity);
    
    void Update(TEntity entity);
    
    IReadOnlyList<TEntity> GetAll(Func<TEntity, bool> predicate = null, int? takeCount = null);
    
    TEntity GetById(TKey key);
    
    void DeleteById(TKey key);
    
    void DeleteByIds(IEnumerable<TKey> keys);
}