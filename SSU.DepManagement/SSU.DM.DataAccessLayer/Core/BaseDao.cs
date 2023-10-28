using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.ConfigSections;
using Models.Extensions;
using System.Data;
using SSU.DM.DataAccessLayer.Core.Interface;

namespace SSU.DM.DataAccessLayer.Core;

public abstract class BaseDao<TEntity, TKey> : IDisposable, IDao<TEntity, TKey>, IAsyncDisposable where TEntity : class
{
    private readonly ApplicationContext _ctx;

    protected BaseDao(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    protected abstract DbSet<TEntity> SelectDbSet(ApplicationContext db);
    
    public virtual void Add(TEntity entity)
    {
        UseContext(db => SelectDbSet(db).Add(entity));
    }
    
    public virtual void Update(TEntity entity)
    {
        UseContext(db => SelectDbSet(db).Update(entity));
    }

    public virtual void DeleteById(TKey key)
    {
        UseContext(db => SelectDbSet(db).Remove(SelectDbSet(db).Find(key)));
    }
    
    public virtual void DeleteByIds(IEnumerable<TKey> keys)
    {
        UseContext(db 
            => SelectDbSet(db)
                .RemoveRange(keys.Select(k => SelectDbSet(db).Find(k))));
    }

    public virtual IReadOnlyList<TEntity> GetAll(Func<TEntity, bool> predicate = null, int? takeCount = null)
    {
        return UseContext(db =>
        {
            var query = SelectDbSet(db).AsEnumerable();
            if (predicate != null)
                query = query.Where(predicate);

            if (takeCount.HasValue)
                query = query.Take(takeCount.Value);
            
            //query.Load();
            return query.ToList();
        });
    }

    public virtual TEntity GetById(TKey key)
        => UseContext(db => SelectDbSet(db).Find(key));
    
    protected void UseContext(Action<ApplicationContext> action)
    {
        action(_ctx);
        _ctx.SaveChanges();
    }
    protected T UseContext<T>(Func<ApplicationContext, T> function)
    {
        var result = function(_ctx);
        _ctx.SaveChanges();
        return result;
    }

    protected void ExecuteSqlNonQuery(string sql)
    {
        using var command = _ctx.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        _ctx.Database.OpenConnection();
        command.ExecuteNonQuery();
    }

    public void Dispose()
    {
        _ctx?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_ctx != null) await _ctx.DisposeAsync();
    }
}
