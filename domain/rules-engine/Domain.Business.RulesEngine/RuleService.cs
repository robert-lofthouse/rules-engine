using System;
using System.Collections.Generic;
using System.Linq;
using Domain.RulesEngine.Enums;
using Domain.RulesEngine.Interface;
using Domain.RulesEngine.Models;
using RulesEngine.Data.Interface;
using FluentValidation.Results;

namespace Domain.RulesEngine.Business
{

    public class RuleService : IRuleService
    {
        private readonly ICacherService _cacherService;
        private readonly IRulesRepository _rulesRepo;
        private readonly IRulesEngineRepository<RuleSetRule, Guid> _ruleSetRuleRepo;


        public RuleService(ICacherService cacherService,
            IRulesRepository rulesRepo,
            IRulesEngineRepository<RuleSetRule, Guid> ruleSetRuleRepo
            )
        {

            _cacherService = cacherService;
            _ruleSetRuleRepo = ruleSetRuleRepo;
            _rulesRepo = rulesRepo;
        }

        /// <summary>
        /// Retrieve a specific rule based on a specific set of rule conditions
        /// </summary>
        /// <param name="ruleSetRule"></param>
        /// <returns></returns>
        public Rule GetRule(Rule ruleSetRule)
        {
            return _rulesRepo.FindOne(x =>
                x.RuleField == ruleSetRule.RuleField &&
                x.RuleOperator == ruleSetRule.RuleOperator &&
                x.RuleValue == ruleSetRule.RuleValue, null);
        }

        /// <summary>
        /// Retrieve a specific rule based on rule reference number
        /// </summary>
        /// <param name="ruleSetRuleRefNo"></param>
        /// <returns></returns>
        public Rule GetRule(Guid ruleSetRuleRefNo)
        {
            return _rulesRepo.FindOne(x =>
                x.RuleRefNo == ruleSetRuleRefNo, null);
        }

        /// <summary>
        /// List the rules for the specified Rule set reference number
        /// </summary>
        /// <param name="ruleSetRefNo"></param>
        /// <returns></returns>
        public List<Rule> GetRules(Guid ruleSetRefNo)
        {

            return _rulesRepo.GetRules(ruleSetRefNo);
        }

        /// <summary>
        /// List all the rules in the rules repository
        /// </summary>
        /// <returns></returns>
        public List<Rule> GetRules()
        {
            return _rulesRepo.GetAll().ToList();
        }

        /// <summary>
        /// Add a new rule to the rules repository
        /// </summary>
        /// <param name="ruleSetRule"></param>
        /// <returns></returns>
        public ValidationResult AddNewRule(Rule ruleSetRule)
        {
            var validationResult = new ValidationResult();

            try
            {
                _rulesRepo.BeginTransaction();

                if (_rulesRepo.FindOne(c =>
                    c.RuleField == ruleSetRule.RuleField && c.RuleOperator == ruleSetRule.RuleOperator &&
                    c.RuleValue == ruleSetRule.RuleValue, null) != null)
                    throw new ApplicationException("A rule with these conditions already exists");

                ruleSetRule.RuleRefNo = Guid.NewGuid();

                _rulesRepo.Save(ruleSetRule);

                _rulesRepo.Commit();

            }
            catch (Exception ex)
            {
                validationResult.Errors.Add(new ValidationFailure("New Rule Request", ex.Message));
            }

            return validationResult;
        }

        /// <summary>
        /// Save an existing rule's field or value
        /// </summary>
        /// <param name="ruleSetRule"></param>
        /// <returns></returns>
        public ValidationResult SaveRule(Rule ruleSetRule)
        {
            var validationResult = new ValidationResult();
            try
            {
                _rulesRepo.Update(ruleSetRule);

            }
            catch (Exception ex)
            {
                validationResult.Errors.Add(new ValidationFailure("Update Rule Request", ex.Message));
            }
            return validationResult;
        }

        /// <summary>
        /// Delete a rule for the provided Ref Number
        /// </summary>
        /// <param name="ruleSetRuleRefNo"></param>
        /// <returns></returns>
        public ValidationResult DeleteRule(Guid ruleSetRuleRefNo)
        {
            var validationResult = new ValidationResult();
            try
            {
                var ruleSetRule = _rulesRepo.FindOne(c => c.RuleRefNo == ruleSetRuleRefNo, null);
                if (ruleSetRule != null && ruleSetRule.RuleRefNo.HasValue)
                {
                    _rulesRepo.Delete(ruleSetRule.RuleRefNo.Value);
                }

            }
            catch (Exception ex)
            {
                validationResult.Errors.Add(new ValidationFailure("Delete Rule Request", ex.Message));
            }
            return validationResult;
        }

    }
}
