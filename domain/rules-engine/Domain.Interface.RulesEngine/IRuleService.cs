using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Domain.RulesEngine.Models;

namespace Domain.RulesEngine.Interface
{
    public interface IRuleService
    {
        List<Rule> GetRules(Guid ruleSetRefNo);
        List<Rule> GetRules();
        Rule GetRule(Rule ruleSetRule);
        Rule GetRule(Guid ruleSetRuleRefNo);
        ValidationResult AddNewRule(Rule ruleSetRule);
        ValidationResult SaveRule(Rule ruleSetRule);
        ValidationResult DeleteRule(Guid ruleSetRuleRefNo);

    }

}
