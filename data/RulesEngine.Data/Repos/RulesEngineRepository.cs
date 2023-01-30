using System.Collections.Generic;
using System.Linq;
using Domain.RulesEngine.Interface;
using RulesEngine.Data.Interface;
using Npgsql;
using RepoDb.Interfaces;


namespace RulesEngine.Data.Repos
{
    public class RulesEngineRepository<TEntity, TKey> : BaseRepository<TEntity, TKey>, IRulesEngineRepository<TEntity, TKey>
        where TEntity : class
    {
        private readonly IDatabaseContext<NpgsqlConnection> _dbContext;

        public RulesEngineRepository(IEnumerable<IDatabaseContext<NpgsqlConnection>> contexts,
            ICache cache,
            ITrace trace,
            ILocalSettings appSettings,
            IDatabaseContext<NpgsqlConnection> dbContext

            ) : base(contexts.FirstOrDefault(c => c.DbName == "RulesEngine"), cache, trace, appSettings)
        {
            _dbContext = dbContext;
        }

    }
}
