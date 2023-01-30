using System;
using System.Collections.Generic;
using Domain.RulesEngine.Interface;
using Domain.RulesEngine.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.RulesEngine.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RuleSetTypeController : Controller
    {
        private readonly IRuleSetTypeService _ruleSetTypeService;

        public RuleSetTypeController(IRuleSetTypeService ruleSetTypeService)
        {
            _ruleSetTypeService = ruleSetTypeService;
        }

        /// <summary>
        /// Add a new ruleset Type with a field, operator and a value - cannot add a rule with an existing field/operator/value combination
        /// </summary>
        /// <param name="ruleSetType"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddNewRuleSetType([FromBody] RuleSetType ruleSetType)
        {
            return Ok(_ruleSetTypeService.AddNewRuleSetType(ruleSetType));
        }
        
        /// <summary>
        /// Update an existing rule with different values for either field, operator or value
        /// </summary>
        /// <param name="ruleSetRule"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult SaveRuleSetType([FromBody] RuleSetType ruleSetType)
        {
            return Ok(_ruleSetTypeService.SaveRuleSetType(ruleSetType));
        }

        /// <summary>
        /// Delete an existing rule based on ref no
        /// </summary>
        /// <param name="ruleSetTypeRefNo"></param>
        /// <returns></returns>
        [HttpDelete("{ruleSetTypeRefNo}")]
        public IActionResult DeleteRuleSetType(Guid ruleSetTypeRefNo)
        {
            return Ok(_ruleSetTypeService.DeleteRuleSetType(ruleSetTypeRefNo));
        }

        /// <summary>
        /// Get a list of all rules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<RuleSetType> GetRuleSetTypes()
        {
            return _ruleSetTypeService.GetRuleSetTypes();
        }

        /// <summary>
        /// Get a specific rule based on Ref No
        /// </summary>
        /// <param name="ruleSetRuleRefNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRuleSetType/{ruleSetTypeRefNo}")]
        public RuleSetType GetRuleSetTypeByRefNo(Guid ruleSetTypeRefNo)
        {
            return _ruleSetTypeService.GetRuleSetType(ruleSetTypeRefNo);
        }

        /// <summary>
        /// Get a specific rule based on rule values (field, operator, value)
        /// </summary>
        /// <param name="ruleSetRule"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public RuleSetType GetRuleSetType([FromBody]RuleSetType ruleSetType)
        {
            return _ruleSetTypeService.GetRuleSetType(ruleSetType);
        }

    }

}
