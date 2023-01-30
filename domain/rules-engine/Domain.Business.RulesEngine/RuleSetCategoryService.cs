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

    public class RuleSetCategoryService : IRuleSetCategoryService
    {
        private readonly ICacherService _cacherService;
        private readonly IRulesEngineRepository<RuleSetCategory, Guid> _ruleSetCategoryRepo;


        public RuleSetCategoryService(ICacherService cacherService,
            IRulesEngineRepository<RuleSetCategory, Guid> ruleSetCategoryRepo
            )
        {

            _cacherService = cacherService;
            _ruleSetCategoryRepo = ruleSetCategoryRepo;
        }

        /// <summary>
        /// Retrieve a specific rule based on a specific set of rule conditions
        /// </summary>
        /// <param name="ruleSetCategory"></param>
        /// <returns></returns>
        public RuleSetCategory GetRuleSetCategory(RuleSetCategory ruleSetCategory)
        {
            return _ruleSetCategoryRepo.FindOne(x =>
                x.RuleSetCategoryName == ruleSetCategory.RuleSetCategoryName, null);
        }

        /// <summary>
        /// Retrieve a specific rule based on rule reference number
        /// </summary>
        /// <param name="ruleSetCategoryRefNo"></param>
        /// <returns></returns>
        public RuleSetCategory GetRuleSetCategory(Guid ruleSetCategoryRefNo)
        {
            return _ruleSetCategoryRepo.FindOne(x =>
                x.RuleSetCategoryRefNo == ruleSetCategoryRefNo, null);
        }


        /// <summary>
        /// List all the rules in the rules repository
        /// </summary>
        /// <returns></returns>
        public List<RuleSetCategory> GetRuleSetCategories()
        {
            return _ruleSetCategoryRepo.GetAll().ToList();
        }

        /// <summary>
        /// Add a new rule to the rules repository
        /// </summary>
        /// <param name="ruleSetCategory"></param>
        /// <returns></returns>
        public ValidationResult AddNewRuleSetCategory(RuleSetCategory ruleSetCategory)
        {
            var validationResult = new ValidationResult();

            try
            {
                _ruleSetCategoryRepo.BeginTransaction();

                if (_ruleSetCategoryRepo.FindOne(c =>
                    c.RuleSetCategoryName == ruleSetCategory.RuleSetCategoryName, null) != null)

                    throw new ApplicationException("A ruleset Category with these conditions already exists");

                ruleSetCategory.RuleSetCategoryRefNo = Guid.NewGuid();

                _ruleSetCategoryRepo.Save(ruleSetCategory);

                _ruleSetCategoryRepo.Commit();

            }
            catch (Exception ex)
            {
                validationResult.Errors.Add(new ValidationFailure("New Rule Set Category Request", ex.Message));
            }

            return validationResult;
        }

        /// <summary>
        /// Save an existing rule's field or value
        /// </summary>
        /// <param name="ruleSetCategory"></param>
        /// <returns></returns>
        public ValidationResult SaveRuleSetCategory(RuleSetCategory ruleSetCategory)
        {
            var validationResult = new ValidationResult();
            try
            {
                _ruleSetCategoryRepo.Update(ruleSetCategory);

            }
            catch (Exception ex)
            {
                validationResult.Errors.Add(new ValidationFailure("Update RuleSet Category Request", ex.Message));
            }
            return validationResult;
        }

        /// <summary>
        /// Delete a rule for the provided Ref Number
        /// </summary>
        /// <param name="ruleSetCategoryRefNo"></param>
        /// <returns></returns>
        public ValidationResult DeleteRuleSetCategory(Guid ruleSetCategoryRefNo)
        {
            var validationResult = new ValidationResult();
            try
            {
                var ruleSetCategory = _ruleSetCategoryRepo.FindOne(c => c.RuleSetCategoryRefNo == ruleSetCategoryRefNo, null);
                if (ruleSetCategory != null && ruleSetCategory.RuleSetCategoryRefNo.HasValue)
                {
                    _ruleSetCategoryRepo.Delete(ruleSetCategory.RuleSetCategoryRefNo.Value);
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
