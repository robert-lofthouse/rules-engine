using System;

namespace Domain.RulesEngine.Models
{

    public class Rule
    {
        public Rule()
        {

        }
        public Rule(string ruleField, string ruleOperator
            , string ruleValue)
        {
            RuleField = ruleField;
            RuleOperator = ruleOperator;
            RuleValue = ruleValue;
        }

        public Guid? RuleRefNo { get; set; }

        public string RuleField { get; set; }
        public string RuleOperator { get; set; }
        public string RuleValue { get; set; }


    }
}