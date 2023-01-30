using System;
using System.Collections.Generic;
using Domain.RulesEngine.Models;
using FluentValidation.Results;

namespace Domain.RulesEngine.Interface
{
    public interface IRuleSetCategoryService
    {
        /// <summary>
        /// Retrieve a specific rule based on a specific set of rule conditions
        /// </summary>
        /// <param name="ruleSetCategory"></param>
        /// <returns></returns>
        RuleSetCategory GetRuleSetCategory(RuleSetCategory ruleSetCategory);

        /// <summary>
        /// Retrieve a specific rule based on rule reference number
        /// </summary>
        /// <param name="ruleSetCategoryRefNo"></param>
        /// <returns></returns>
        RuleSetCategory GetRuleSetCategory(Guid ruleSetCategoryRefNo);

        /// <summary>
        /// List all the rules in the rules repository
        /// </summary>
        /// <returns></returns>
        List<RuleSetCategory> GetRuleSetCategories();

        /// <summary>
        /// Add a new rule to the rules repository
        /// </summary>
        /// <param name="ruleSetCategory"></param>
        /// <returns></returns>
        ValidationResult AddNewRuleSetCategory(RuleSetCategory ruleSetCategory);

        /// <summary>
        /// Save an existing rule's field or value
        /// </summary>
        /// <param name="ruleSetCategory"></param>
        /// <returns></returns>
        ValidationResult SaveRuleSetCategory(RuleSetCategory ruleSetCategory);

        /// <summary>
        /// Delete a rule for the provided Ref Number
        /// </summary>
        /// <param name="ruleSetCategoryRefNo"></param>
        /// <returns></returns>
        ValidationResult DeleteRuleSetCategory(Guid ruleSetCategoryRefNo);

        
    }
}