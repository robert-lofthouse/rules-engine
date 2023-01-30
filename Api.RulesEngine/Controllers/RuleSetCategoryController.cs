using System;
using System.Collections.Generic;
using Domain.RulesEngine.Interface;
using Domain.RulesEngine.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.RulesEngine.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RuleSetCategoryController : Controller
    {
        private readonly IRuleSetCategoryService _ruleSetCategoryService;

        public RuleSetCategoryController(IRuleSetCategoryService ruleSetCategoryService)
        {
            _ruleSetCategoryService = ruleSetCategoryService;
        }

        /// <summary>
        /// Add a new rule with a field, operator and a value - cannot add a rule with an existing field/operator/value combination
        /// </summary>
        /// <param name="ruleSetCategory"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddNewRuleSetCategory([FromBody] RuleSetCategory ruleSetCategory)
        {
            return Ok(_ruleSetCategoryService.AddNewRuleSetCategory(ruleSetCategory));
        }
        
        /// <summary>
        /// Update an existing rule with different values for either field, operator or value
        /// </summary>
        /// <param name="ruleSetCategory"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult SaveRuleSetCategory([FromBody] RuleSetCategory ruleSetCategory)
        {
            return Ok(_ruleSetCategoryService.SaveRuleSetCategory(ruleSetCategory));
        }

        /// <summary>
        /// Delete an existing rule based on ref no
        /// </summary>
        /// <param name="ruleSetCategoryRefNo"></param>
        /// <returns></returns>
        [HttpDelete("{ruleSetCategoryRefNo}")]
        public IActionResult DeleteRuleSetCategory(Guid ruleSetCategoryRefNo)
        {
            return Ok(_ruleSetCategoryService.DeleteRuleSetCategory(ruleSetCategoryRefNo));
        }

        /// <summary>
        /// Get a list of all rules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<RuleSetCategory> GetRuleSetCategories()
        {
            return _ruleSetCategoryService.GetRuleSetCategories();
        }

        /// <summary>
        /// Get a specific rule based on Ref No
        /// </summary>
        /// <param name="ruleSetCategoryRefNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRuleSetCategory/{ruleSetCategoryRefNo}")]
        public RuleSetCategory GetRuleSetCategoryByRefNo(Guid ruleSetCategoryRefNo)
        {
            return _ruleSetCategoryService.GetRuleSetCategory(ruleSetCategoryRefNo);
        }

        /// <summary>
        /// Get a specific rule based on rule values (field, operator, value)
        /// </summary>
        /// <param name="ruleSetCategory"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public RuleSetCategory GetRuleSetCategory([FromBody]RuleSetCategory ruleSetCategory)
        {
            return _ruleSetCategoryService.GetRuleSetCategory(ruleSetCategory);
        }

    }

}
