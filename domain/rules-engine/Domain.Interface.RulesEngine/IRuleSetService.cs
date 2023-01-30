using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Domain.RulesEngine.Models;

namespace Domain.RulesEngine.Interface
{
    public interface IRuleSetService
    {
        List<RuleEvaluationReturn> EvaluateRuleSets(RuleSetTypeEvaluationData ruleSetEvaluationData);
        List<RuleEvaluationReturn> EvaluateRuleSet(RuleSetEvaluationData ruleSetEvaluationData);
        List<RuleSet> CheckForPossibleConflicts(RuleSet rs);
        RuleSet GetRuleSet(Guid ruleSetRefNo);
        List<RuleSet> GetRuleSets();
        ValidationResult AddNewRuleSet(RuleSet ruleSet);
        ValidationResult SaveRuleSet(RuleSet ruleSet);
        ValidationResult SaveRuleSets(List<RuleSet> ruleSets);        
    }

}
