using System;
using System.Collections.Generic;
using System.Linq;
using Domain.RulesEngine.Interface;
using Domain.RulesEngine.Models;
using RulesEngine.Data.Interface;
using FluentValidation.Results;

namespace Domain.RulesEngine.Business
{

    public class RuleSetTypeService : IRuleSetTypeService
    {
        private readonly ICacherService _cacherService;
        private readonly IRulesEngineRepository<RuleSetType, Guid> _ruleSetTypeRepo;


        public RuleSetTypeService(
            ICacherService cacherService,
            IRulesRepository rulesRepo,
            IRuleSetRepository ruleSetRepo,
            IRulesEngineRepository<RuleSetType, Guid> ruleSetTypeRepo
            )
        {

            _cacherService = cacherService;
            _ruleSetTypeRepo = ruleSetTypeRepo;
        }

        /// <summary>
        /// Retrieve a specific rule based on a specific set of rule conditions
        /// </summary>
        /// <param name="ruleSetTypeRule"></param>
        /// <returns></returns>
        public RuleSetType GetRuleSetType(RuleSetType ruleSetType)
        {
            return _ruleSetTypeRepo.FindOne(x =>
                x.RuleSetTypeName == ruleSetType.RuleSetTypeName, null);
        }

        /// <summary>
        /// Retrieve a specific rule based on rule reference number
        /// </summary>
        /// <param name="ruleSetTypeRefNo"></param>
        /// <returns></returns>
        public RuleSetType GetRuleSetType(Guid ruleSetTypeRefNo)
        {
            return _ruleSetTypeRepo.FindOne(x =>
                x.RuleSetTypeRefNo == ruleSetTypeRefNo, null);
        }


        /// <summary>
        /// List all the rules in the rules repository
        /// </summary>
        /// <returns></returns>
        public List<RuleSetType> GetRuleSetTypes()
        {
            return _ruleSetTypeRepo.GetAll().ToList();
        }

        /// <summary>
        /// Add a new rule to the rules repository
        /// </summary>
        /// <param name="ruleSetRule"></param>
        /// <returns></returns>
        public ValidationResult AddNewRuleSetType(RuleSetType ruleSetType)
        {
            var validationResult = new ValidationResult();

            try
            {
                _ruleSetTypeRepo.BeginTransaction();

                if (_ruleSetTypeRepo.FindOne(c =>
                    c.RuleSetTypeName == ruleSetType.RuleSetTypeName, null) != null)
                    throw new ApplicationException("A Ruleset Type with this name already exists");

                ruleSetType.RuleSetTypeRefNo = Guid.NewGuid();

                _ruleSetTypeRepo.Save(ruleSetType);

                _ruleSetTypeRepo.Commit();

            }
            catch (Exception ex)
            {
                validationResult.Errors.Add(new ValidationFailure("New Ruleset Type Request", ex.Message));
            }

            return validationResult;
        }

        /// <summary>
        /// Save an existing rule's field or value
        /// </summary>
        /// <param name="ruleSetType"></param>
        /// <returns></returns>
        public ValidationResult SaveRuleSetType(RuleSetType ruleSetType)
        {
            var validationResult = new ValidationResult();
            try
            {
                _ruleSetTypeRepo.Update(ruleSetType);

            }
            catch (Exception ex)
            {
                validationResult.Errors.Add(new ValidationFailure("Update Ruleset Type Request", ex.Message));
            }
            return validationResult;
        }

        /// <summary>
        /// Delete a rule for the provided Ref Number
        /// </summary>
        /// <param name="ruleSetTypeRefNo"></param>
        /// <returns></returns>
        public ValidationResult DeleteRuleSetType(Guid ruleSetTypeRefNo)
        {
            var validationResult = new ValidationResult();
            try
            {
                var ruleSetType = _ruleSetTypeRepo.FindOne(c => c.RuleSetTypeRefNo == ruleSetTypeRefNo, null);
                if (ruleSetType  != null)
                {
                    _ruleSetTypeRepo.Delete(ruleSetTypeRefNo);
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
