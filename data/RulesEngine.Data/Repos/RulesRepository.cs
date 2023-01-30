using System;
using System.Collections.Generic;
using System.Linq;
using Domain.RulesEngine.Interface;
using Domain.RulesEngine.Models;
using RulesEngine.Data.Interface;
using Npgsql;
using RepoDb;
using RepoDb.Interfaces;


namespace RulesEngine.Data.Repos
{
    public class RulesRepository : BaseRepository<Rule, Guid>, IRulesRepository
    {
        private readonly IDatabaseContext<NpgsqlConnection> _dbContext;

        public RulesRepository(IEnumerable<IDatabaseContext<NpgsqlConnection>> contexts,
            ICache cache,
            ITrace trace,
            ILocalSettings appSettings,
            IDatabaseContext<NpgsqlConnection> dbContext

            ) : base(contexts.FirstOrDefault(c => c.DbName == "RulesEngine"), cache, trace, appSettings)
        {
            _dbContext = dbContext;
        }

        public List<Rule> GetRules(Guid? ruleSetRefNo)
        {
            var rules = new List<Rule>();

            if (ruleSetRefNo.HasValue)
            {

                var RulesEngineQuery = _dbContext.Connection.QueryMultiple<Rule, RuleSetRule>(
                    rer => rer.RuleRefNo != null, rersr => rersr.RuleSetRefNo == ruleSetRefNo.Value);


                rules = RulesEngineQuery.Item1.Where(rer =>
                        RulesEngineQuery.Item2.Select(rersr => rersr.RuleRefNo).ToList().Contains(rer.RuleRefNo.Value))
                    .ToList();

            }

            return rules;
        }

    }
}
