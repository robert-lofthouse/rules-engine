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

    public class RuleSetService : IRuleSetService
    {
        private readonly ICacherService _cacherService;
        private readonly IRuleSetRepository _ruleSetRepo;
        private readonly IRulesEngineRepository<RuleSetRule, Guid> _ruleSetRuleRepo;
        private readonly IRuleService _ruleService;


        public RuleSetService(ICacherService cacherService,
            IRuleSetRepository ruleSetRepo,
            IRulesEngineRepository<RuleSetRule, Guid> ruleSetRuleRepo,
            IRuleService ruleService
            )
        {

            _cacherService = cacherService;
            _ruleSetRuleRepo = ruleSetRuleRepo;
            _ruleService = ruleService;
            _ruleSetRepo = ruleSetRepo;
        }


        /// <summary>
        /// Evaluates the input data against the configured rulesets and returns a list of entity instance numbers linked to the ruleset
        /// </summary>
        /// <param name="ruleSetTypeEvaluationData"></param>
        /// <returns></returns>
        public List<RuleEvaluationReturn> EvaluateRuleSets(RuleSetTypeEvaluationData ruleSetTypeEvaluationData)
        {

            List<RuleSet> evalRuleSets = _ruleSetRepo.GetRuleSets(ruleSetTypeEvaluationData.RuleSetType);
            List<RuleEvaluationReturn> evalReturn = new List<RuleEvaluationReturn>();

            foreach (var rs in evalRuleSets.OrderByDescending(c => c.Rules.Count).ThenBy(x => x.RuleSetRanking).ToList())
            {
                if (rs.Rules.Count > 0)
                {
                    var compiledRules = PrecompiledRules.CompileRule<RuleSetTypeEvaluationData>(rs.Rules);

                    if (compiledRules.TakeWhile(rule => rule(ruleSetTypeEvaluationData)).Count() ==
                        rs.Rules.Count)
                    {
                        evalReturn.Add(new RuleEvaluationReturn
                            {RuleSetRefNo = rs.RuleSetRefNo.Value, RuleSetName = rs.RuleSetName});
                        if (ruleSetTypeEvaluationData.EvaluationType != RulesEngineEvaluationType.MatchAll)
                            break;
                    }
                }
            }

            return evalReturn;
        }        
        
        public List<RuleEvaluationReturn> EvaluateRuleSet(RuleSetEvaluationData ruleSetEvaluationData)
        {

            RuleSet evalRuleSet = _ruleSetRepo.GetRuleSet(ruleSetEvaluationData.RuleSetRefNoType);
            List<RuleEvaluationReturn> evalReturn = new List<RuleEvaluationReturn>();

            if (evalRuleSet.Rules.Count > 0)
            {
                var compiledRules = PrecompiledRules.CompileRule<RuleSetEvaluationData>(evalRuleSet.Rules);

                if (compiledRules.TakeWhile(rule => rule(ruleSetEvaluationData)).Count() ==
                    evalRuleSet.Rules.Count)
                {
                    evalReturn.Add(new RuleEvaluationReturn
                        {RuleSetRefNo = evalRuleSet.RuleSetRefNo.Value, RuleSetName = evalRuleSet.RuleSetName});
                }
            }

            return evalReturn;
        }

        /// <summary>
        /// Checks if there are any possible ruleset conflicts (rulesets that might evaluate to true in addition to the ruleset passed in)
        /// Conflicts occur if
        ///     - The number of rules are the same AND
        ///     -   of the rules that are not exactly the same, there are no match fields
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public List<RuleSet> CheckForPossibleConflicts(RuleSet rs)
        {
            var conflictList = new List<RuleSet>();
            var rulesToCheck = rs.Rules;
            var allDifferent = true;
            var allSame = true;
            var someSame = false;



            foreach (var ruleSet in _ruleSetRepo.GetRuleSetsForConflicts(rs))
            {
                var dbRules = ruleSet.Rules;
                someSame = false;

                var matchingDBRules = dbRules.Where(x => rulesToCheck.Any(y => y.RuleField == x.RuleField && y.RuleValue == x.RuleValue)).ToList();
                var matchingRulesToCheck = rulesToCheck.Where(x => dbRules.Any(y => y.RuleField == x.RuleField && y.RuleValue == x.RuleValue)).ToList();

                var filterDbRules = dbRules.Where(x => !matchingDBRules.Contains(x)).ToList();
                var filterRulesToCheck = rulesToCheck.Where(x => !matchingRulesToCheck.Contains(x)).ToList();

                if (filterDbRules.Any(dbRule =>
                {

                    if (!someSame)
                        someSame = filterRulesToCheck.Any(ruleToCheck => ruleToCheck.RuleField == dbRule.RuleField);

                    //first check if all the fields are different - this is a conflict
                    allDifferent = allDifferent && filterRulesToCheck.All(ruleToCheck => ruleToCheck.RuleField != dbRule.RuleField);
                    if (allDifferent) return true;

                    //then check if all the fields are the same (regardless of value) - this is not a conflict
                    allSame = allSame && filterRulesToCheck.Any(ruleToCheck => ruleToCheck.RuleField == dbRule.RuleField);
                    if (allSame) return false;

                    return !someSame;
                }))
                {
                    conflictList.Add(ruleSet);
                    //break;
                }
            }

            return conflictList;
        }


        /// <summary>
        /// Returns the ruleset based on the Ruleset ref no passed in
        /// </summary>
        /// <param name="ruleSetRefNo"></param>
        /// <returns></returns>
        public RuleSet GetRuleSet(Guid ruleSetRefNo)
        {
            var rs = _ruleSetRepo.GetRuleSet(ruleSetRefNo);
            return rs;
        }

        /// <summary>
        /// Returns a list of all the rulesets configured
        /// </summary>
        /// <param name="RuleSetRefNo"></param>
        /// <returns></returns>
        public List<RuleSet> GetRuleSets()
        {
            return _ruleSetRepo.GetRuleSets();
        }


        /// <summary>
        /// inserts a new ruleset and links relevant rules to the ruleset
        /// If the entity instance ref no is provided, it will be linked
        /// </summary>
        /// <param name="ruleSet"></param>
        /// <returns></returns>
        public ValidationResult AddNewRuleSet(RuleSet ruleSet)
        {
            var validationResult = new ValidationResult();
            try
            {
                ruleSet.RuleSetRefNo = Guid.NewGuid();

                _ruleSetRepo.BeginTransaction();

                _ruleSetRepo.Save(ruleSet);

                validationResult = LinkRulesToRuleSet(ruleSet);

                if (validationResult.IsValid)
                    _ruleSetRepo.Commit();

            }
            catch (Exception ex)
            {
                validationResult.Errors.Add(new ValidationFailure("New Rule Set Request", ex.Message));
            }
            return validationResult;
        }


        /// <summary>
        /// Save the ruleset associated to the input parameter. This should include any rules linked
        /// </summary>
        /// <param name="ruleSet"></param>
        /// <returns></returns>
        public ValidationResult SaveRuleSet(RuleSet ruleSet)
        {
            var validationResult = new ValidationResult();
            try
            {
                _ruleSetRepo.BeginTransaction();
                ;
                validationResult = LinkRulesToRuleSet(ruleSet);

                _ruleSetRepo.Update(ruleSet);

                if (validationResult.IsValid)
                    _ruleSetRepo.Commit();

            }
            catch (Exception ex)
            {
                validationResult.Errors.Add(new ValidationFailure("Save Rule Set Request", ex.Message));
            }
            return validationResult;
        }

        /// <summary>
        /// Save the rulesets associated to the input parameter. This should include any rules linked
        /// </summary>
        /// <param name="ruleSets"></param>
        /// <returns></returns>
        public ValidationResult SaveRuleSets(List<RuleSet> ruleSets)
        {
            var validationResult = new ValidationResult();
            try
            {
                _ruleSetRepo.BeginTransaction();
                foreach (var ruleSet in ruleSets)
                {
                    _ruleSetRepo.Update(ruleSet);
                    validationResult = LinkRulesToRuleSet(ruleSet);
                }

                _ruleSetRepo.Commit();

            }
            catch (Exception ex)
            {
                validationResult.Errors.Add(new ValidationFailure("Save Rule Sets Request", ex.Message));
            }
            return validationResult;
        }


        /// <summary>
        /// Refreshes the rules associated to the ruleset 
        /// </summary>
        /// <param name="ruleSet"></param>
        /// <returns></returns>
        private ValidationResult LinkRulesToRuleSet(RuleSet ruleSet)
        {
            var res = new ValidationResult();

            try
            {

                if (ruleSet.RuleSetRefNo.HasValue)
                {
                    var rersRules = _ruleSetRuleRepo.FindAll(x => x.RuleSetRefNo == ruleSet.RuleSetRefNo,null)
                        .ToList();

                    foreach (var rulesEngineRuleSetRule in rersRules)
                    {
                        _ruleSetRuleRepo.Delete(rulesEngineRuleSetRule.RuleSetRuleRefNo);
                    }

                    if (ruleSet.Rules != null)
                    {
                        foreach (var rule in ruleSet.Rules)
                        {
                            if (rule.RuleRefNo.HasValue)
                            {
                                if (_ruleService.GetRule(rule.RuleRefNo.Value) == null)
                                {
                                    res = _ruleService.SaveRule(rule);
                                }

                                if (!res.IsValid)
                                {
                                    break;
                                }

                                _ruleSetRuleRepo.Save(new RuleSetRule
                                    {
                                        RuleRefNo = rule.RuleRefNo.Value,
                                        RuleSetRefNo = ruleSet.RuleSetRefNo.Value,
                                        RuleSetRuleRefNo = Guid.NewGuid()
                                    });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Errors.Add(new ValidationFailure("Link Rules and Rule Set Request", ex.Message));

            }

            return res;
        }
    }
}
