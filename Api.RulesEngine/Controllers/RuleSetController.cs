using System;
using System.Collections.Generic;
using Domain.RulesEngine.Interface;
using Domain.RulesEngine.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.RulesEngine.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RuleSetController : Controller
    {
        private readonly IRuleSetService _ruleSetService;

        public RuleSetController(IRuleSetService rulesEngineService)
        {
            _ruleSetService = rulesEngineService;
        }

        /// <summary>
        /// Checks for any other rulesets that may also evaluate to true along with the supplied ruleset
        /// </summary>
        /// <param name="ruleSet"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheckForConflicts")]
        public List<RuleSet> CheckForConflicts([FromBody] RuleSet ruleSet)
        {
            return _ruleSetService.CheckForPossibleConflicts(ruleSet);
        }

        /// <summary>
        /// Evaluates the conditions supplied to determine which rulesets match
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Evaluate")]
        public List<RuleEvaluationReturn> EvaluateRuleSets([FromBody] RuleSetTypeEvaluationData dataObject)
        {
            return _ruleSetService.EvaluateRuleSets(dataObject);
        }
        
        /// <summary>
        /// Evaluates the conditions supplied to determine which rulesets match
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EvaluateRuleSet")]
        public List<RuleEvaluationReturn> EvaluateRuleSet([FromBody] RuleSetEvaluationData dataObject)
        {
            return _ruleSetService.EvaluateRuleSet(dataObject);
        }


        /// <summary>
        /// Get a list of all rulesets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<RuleSet> RuleSets()
        {
            return _ruleSetService.GetRuleSets();
        }

        /// <summary>
        /// Get a specific ruleset for the supplied ruleset ref no
        /// </summary>
        /// <param name="RuleSetRefNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{RuleSetRefNo}")]
        public RuleSet RuleSet(Guid RuleSetRefNo)
        {
            return _ruleSetService.GetRuleSet(RuleSetRefNo);
        }

        /// <summary>
        /// Save an existing ruleset with different values. Can be used to change the name, ranking or list of rules for a ruleset
        /// </summary>
        /// <param name="ruleSet"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult SaveRuleSet([FromBody] RuleSet ruleSet)
        {
            return Ok(_ruleSetService.SaveRuleSet(ruleSet));
        }

        /// <summary>
        /// Save existing rulesets with different values. Can be used to change the name, ranking or list of rules for a ruleset
        /// </summary>
        /// <param name="ruleSets"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("SaveRuleSets")]
        public IActionResult SaveRuleSets([FromBody] List<RuleSet> ruleSets)
        {
            return Ok(_ruleSetService.SaveRuleSets(ruleSets));
        }

        /// <summary>
        /// Save a new ruleset 
        /// </summary>
        /// <param name="ruleSet"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddNewRuleSet([FromBody] RuleSet ruleSet)
        {
            return Ok(_ruleSetService.AddNewRuleSet(ruleSet));
        }

    }

}
