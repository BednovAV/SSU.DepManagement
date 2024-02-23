namespace SSU.DM.DataAccessLayer.Core.Interface;

public interface IDao<TEntity, TKey> where TEntity : class
{
    TKey Add(TEntity entity);
    
    void Add(IEnumerable<TEntity> entities);
    
    void Update(TEntity entity);
    
    void Update(IEnumerable<TEntity> entities);
    
    IReadOnlyList<TEntity> GetAll(Func<TEntity, bool> predicate = null, int? takeCount = null);
    
    TEntity GetById(TKey key);
    
    void DeleteById(TKey key);
    
    void DeleteByIds(IEnumerable<TKey> keys);
}