using System;
using System.Collections.Generic;
using System.Linq;
using Domain.RulesEngine.Interface;
using Domain.RulesEngine.Models;
using RulesEngine.Data.Interface;
using Npgsql;
using RepoDb;
using RepoDb.Extensions;
using RepoDb.Interfaces;


namespace RulesEngine.Data.Repos
{
    public class RuleSetRepository : BaseRepository<RuleSet, Guid>, IRuleSetRepository
    {
        private readonly IRulesRepository _rulesRepo;
        private readonly IDatabaseContext<NpgsqlConnection> _dbContext;

        public RuleSetRepository(IEnumerable<IDatabaseContext<NpgsqlConnection>> contexts,
            ICache cache,
            ITrace trace,
            ILocalSettings appSettings,
            IRulesRepository rulesRepo,
            IDatabaseContext<NpgsqlConnection> dbContext

            ) : base(contexts.FirstOrDefault(c => c.DbName == "RulesEngine"), cache, trace, appSettings)
        {
            _rulesRepo = rulesRepo;
            _dbContext = dbContext;
        }
        public List<RuleSet> GetRuleSetsForConflicts(RuleSet rs)
        {
            var rsRules = rs.Rules;

            var RulesetsAndRules = GetRuleSets().Where(x =>
                    (x.Rules.Count == rsRules.Count && x.RuleSetTypeRefNo == rs.RuleSetTypeRefNo && x.RuleSetRanking == rs.RuleSetRanking) &&
                    x.RuleSetRefNo != rs.RuleSetRefNo)
                .ToList();

            return RulesetsAndRules;
        }

        public List<RuleSet> GetRuleSets()
        {
            var rsList = new List<RuleSet>();


            var rulesEngine =
                _dbContext.Connection.QueryMultiple<RuleSet, RuleSetRule, Rule>(
                    rers => rers.RuleSetRefNo != null, 
                    rersr => rersr.RuleSetRuleRefNo != null,
                    rer => rer.RuleRefNo != null);

            rsList = rulesEngine.Item1.AsList();

            rsList.ForEach(rers => rers.Rules = rulesEngine.Item3.Where(
                rer => rulesEngine.Item2.Where(
                        rersr => rersr.RuleSetRefNo == rers.RuleSetRefNo).Select(x => x.RuleRefNo)
                    .Contains(rer.RuleRefNo.Value)).ToList());

            return rsList;
        }

        public List<RuleSet> GetRuleSets(string RuleSetType)
        {

            var rulesEngine =
                _dbContext.Connection.QueryMultiple<RuleSet, RuleSetRule, Rule, RuleSetCategory, RuleSetType>(
                    rs => rs.RuleSetRefNo != null, 
                    rsr => rsr.RuleSetRuleRefNo != null,
                    r => r.RuleRefNo != null,
                    rsc => rsc.RuleSetCategoryName == RuleSetType,
                    rst => rst.RuleSetTypeRefNo != null
                    );

            var ruleSets = rulesEngine.Item1.ToList();
            var ruleSetRules = rulesEngine.Item2.ToList();
            var rules = rulesEngine.Item3.ToList();
            var ruleSetCats = rulesEngine.Item4.ToList();
            var ruleSetTypes = rulesEngine.Item5.Where(y=> ruleSetCats.Select(z=>z.RuleSetCategoryRefNo).Contains(y.RuleSetCategoryRefNo)).ToList();

            ruleSets = ruleSets.Where(rs => ruleSetTypes.Where(rst =>
                ruleSetCats.Select(rsc => rsc.RuleSetCategoryRefNo).Contains(rst.RuleSetCategoryRefNo))
                .Select(x => x.RuleSetTypeRefNo).Contains(rs.RuleSetTypeRefNo)).ToList();

            ruleSets.ForEach(rs =>
            {
                rs.Rules = rules.Where(
                    r => ruleSetRules.Where(
                            rsr => rsr.RuleSetRefNo == rs.RuleSetRefNo).Select(x => x.RuleRefNo)
                        .Contains(r.RuleRefNo.Value)).ToList();

                rs.RuleSetTypeRanking = ruleSetTypes.Find(rst => rst.RuleSetTypeRefNo == rs.RuleSetTypeRefNo)
                    .RuleSetTypeRanking;
            });

            return ruleSets.OrderBy(x => x.RuleSetTypeRanking).ToList();
        }

        public RuleSet GetRuleSet(Guid RuleSetRefNo)
        {
            var rs = _dbContext.Connection.Query<RuleSet>(c => c.RuleSetRefNo == RuleSetRefNo).FirstOrDefault();

            rs.Rules = _rulesRepo.GetRules(rs.RuleSetRefNo);

            return rs;
        }

    }
}
