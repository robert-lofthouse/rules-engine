using System;
using System.Collections.Generic;
using Domain.RulesEngine.Interface;
using Domain.RulesEngine.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.RulesEngine.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RuleController : Controller
    {
        private readonly IRuleService _ruleService;

        public RuleController(IRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        /// <summary>
        /// Add a new rule with a field, operator and a value - cannot add a rule with an existing field/operator/value combination
        /// </summary>
        /// <param name="ruleSetRule"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddNewRule([FromBody] Rule ruleSetRule)
        {
            return Ok(_ruleService.AddNewRule(ruleSetRule));
        }
        
        /// <summary>
        /// Update an existing rule with different values for either field, operator or value
        /// </summary>
        /// <param name="ruleSetRule"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult SaveRule([FromBody] Rule ruleSetRule)
        {
            return Ok(_ruleService.SaveRule(ruleSetRule));
        }

        /// <summary>
        /// Delete an existing rule based on ref no
        /// </summary>
        /// <param name="ruleSetRuleRefNo"></param>
        /// <returns></returns>
        [HttpDelete("{ruleSetRuleRefNo}")]
        public IActionResult DeleteRule(Guid ruleSetRuleRefNo)
        {
            return Ok(_ruleService.DeleteRule(ruleSetRuleRefNo));
        }

        /// <summary>
        /// Get a list of rules for the supplied ruleset ref no
        /// </summary>
        /// <param name="ruleSetRefNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRules/{ruleSetRefNo}")]
        public List<Rule> GetRulesForRuleSet(Guid ruleSetRefNo)
        {
            return _ruleService.GetRules(ruleSetRefNo);
        }

        /// <summary>
        /// Get a list of all rules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Rule> GetRules()
        {
            return _ruleService.GetRules();
        }

        /// <summary>
        /// Get a specific rule based on Ref No
        /// </summary>
        /// <param name="ruleSetRuleRefNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRule/{ruleSetRuleRefNo}")]
        public Rule GetRuleByRefNo(Guid ruleSetRuleRefNo)
        {
            return _ruleService.GetRule(ruleSetRuleRefNo);
        }

        /// <summary>
        /// Get a specific rule based on rule values (field, operator, value)
        /// </summary>
        /// <param name="ruleSetRule"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public Rule GetRule([FromBody]Rule ruleSetRule)
        {
            return _ruleService.GetRule(ruleSetRule);
        }

    }

}
