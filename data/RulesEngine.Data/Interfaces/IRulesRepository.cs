using System;
using System.Collections.Generic;
using Domain.RulesEngine.Models;

namespace RulesEngine.Data.Interface
{
    public interface IRulesRepository : IRulesEngineRepository<Rule,Guid>
    {
        List<Rule> GetRules(Guid? ruleSetRefNo);

    }
}
