using System;
using System.Collections.Generic;
using Domain.RulesEngine.Models;
using FluentValidation.Results;

namespace Domain.RulesEngine.Interface
{
    public interface IRuleSetTypeService
    {
        /// <summary>
        /// Retrieve a specific rule based on a specific set of rule conditions
        /// </summary>
        /// <param name="ruleSetTypeRule"></param>
        /// <returns></returns>
        RuleSetType GetRuleSetType(RuleSetType ruleSetType);

        /// <summary>
        /// Retrieve a specific rule based on rule reference number
        /// </summary>
        /// <param name="ruleSetTypeRefNo"></param>
        /// <returns></returns>
        RuleSetType GetRuleSetType(Guid ruleSetTypeRefNo);

        /// <summary>
        /// List all the rules in the rules repository
        /// </summary>
        /// <returns></returns>
        List<RuleSetType> GetRuleSetTypes();

        /// <summary>
        /// Add a new rule to the rules repository
        /// </summary>
        /// <param name="ruleSetRule"></param>
        /// <returns></returns>
        ValidationResult AddNewRuleSetType(RuleSetType ruleSetType);

        /// <summary>
        /// Save an existing rule's field or value
        /// </summary>
        /// <param name="ruleSetRule"></param>
        /// <returns></returns>
        ValidationResult SaveRuleSetType(RuleSetType ruleSetRule);

        /// <summary>
        /// Delete a rule for the provided Ref Number
        /// </summary>
        /// <param name="ruleSetTypeRefNo"></param>
        /// <returns></returns>
        ValidationResult DeleteRuleSetType(Guid ruleSetTypeRefNo);
    }
}