using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RulesEngine.Data.Interface
{
    public interface IRepository<TEntity, in TKey>
        where TEntity : class
    {
        IEnumerable<TEntity> GetAll(string cacheKey = null);

        TEntity Get(TKey id);

        TEntity FindOne(Expression<Func<TEntity, bool>> expr, string cacheKey);

        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> expr, string cacheKey);

        long Count(Expression<Func<TEntity, bool>> expr);

        int Delete(TKey id);
            
        object Save(TEntity entity);

        object Update(TEntity entity);

        void BeginTransaction();
        
        void Commit();

        List<T> RawSQL<T>(string sql,string cacheKey);
    }
}

  