using System;
using System.Collections.Generic;
using Domain.RulesEngine.Models;

namespace RulesEngine.Data.Interface
{
    public interface IRuleSetRepository : IRulesEngineRepository<RuleSet,Guid>
    {
        List<RuleSet> GetRuleSetsForConflicts(RuleSet rs);
        List<RuleSet> GetRuleSets();
        List<RuleSet> GetRuleSets(string RuleSetType);

        RuleSet GetRuleSet(Guid RuleSetRefNo);

    }
}
