using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Domain.RulesEngine.Interface;
using RulesEngine.Data.Interface;
using RepoDb;
using RepoDb.Interfaces;

namespace RulesEngine.Data.Repos
{
    public class BaseRepository<TEntity, TKey>

      where TEntity : class
    {
        private readonly ILocalSettings _settings;
        private readonly DbConnection _dbContext;
        private readonly ICache _cache;
        private readonly ITrace _trace;
        private DbTransaction _transaction;

        public BaseRepository(IDatabaseContext<DbConnection> context,
            ICache cache,
            ITrace trace,
            ILocalSettings appSettings
            )
        {
            _dbContext = context.Connection;
            _cache = cache;
            _trace = trace;
            _settings = appSettings;
        }

        /*** Properties ***/

        public IEnumerable<TEntity> GetAll(string cacheKey = null)
        {
            return _dbContext.QueryAll<TEntity>(cacheKey: cacheKey,
                cache: _cache,
                trace: _trace,
                cacheItemExpiration: _settings.CacheItemExpiry,
                commandTimeout: _settings.CommandTimeout);
        }

        public TEntity Get(TKey id)
        {
            return _dbContext.Query<TEntity>(id).FirstOrDefault();
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> expr, string cacheKey = null)
        {
            return _dbContext.Query<TEntity>(expr, cacheKey: cacheKey, cache: _cache, trace: _trace, cacheItemExpiration: _settings.CacheItemExpiry, commandTimeout: _settings.CommandTimeout)
                .FirstOrDefault();
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> expr, string cacheKey)
        {

            return _dbContext.Query<TEntity>(expr, cacheKey: cacheKey, cache: _cache, trace: _trace, cacheItemExpiration: _settings.CacheItemExpiry, commandTimeout: _settings.CommandTimeout);
        }

        public long Count(Expression<Func<TEntity, bool>> expr)
        {
            return _dbContext.Count<TEntity>(expr);
        }

        public int Delete(TKey id)
        {
            return _dbContext.Delete<TEntity>(id, trace: _trace);
        }

        public object Save(TEntity entity)
        {
            return _transaction == null ?  _dbContext.Insert(entity, trace: _trace) : _dbContext.Insert(entity, trace: _trace,transaction:_transaction);
        }

        public object Update(TEntity entity)
        {
            return _transaction == null ?  _dbContext.Update(entity, trace: _trace) :_dbContext.Update(entity, trace: _trace,transaction:_transaction);
        }
        public List<T> RawSQL<T>(string sql, string cacheKey = null)
        {
            return _dbContext.ExecuteQuery<T>(sql,cacheKey:cacheKey).ToList();
        }

        public void BeginTransaction()
        {
            _transaction = _dbContext.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }
    }
}
