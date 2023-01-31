using System;
using System.Collections.Generic;
using Domain.RulesEngine.Models;

namespace Domain.RulesEngine.Test
{
    public static class TestRuleSets
    {

        public static List<RuleSet> paymentsRuleSets_TRI_STP =>
            new List<RuleSet>
            {
                new RuleSet
                {
                    RuleSetTypeRefNo = Guid.NewGuid(),
                    RuleSetRefNo = Guid.NewGuid(),
                    Rules = new List<Rule>
                    {
                        new Rule { RuleField = "Charges_Details", RuleOperator = "Equal", RuleValue = "BEN|SHA", RuleRefNo = Guid.NewGuid()},
                        new Rule { RuleField = "ClientCIFKey", RuleOperator = "Equal", RuleValue = "150000990", RuleRefNo = Guid.NewGuid() },
                        new Rule { RuleField = "BeneficiaryAccountCurrency", RuleOperator = "Equal", RuleValue = "MUR", RuleRefNo = Guid.NewGuid() }
                    }
                },
                new RuleSet
                {
                    RuleSetTypeRefNo = Guid.NewGuid(),
                    RuleSetRefNo = Guid.NewGuid(),
                    Rules = new List<Rule>
                    {
                        new Rule { RuleField = "Charges_Details", RuleOperator = "Equal", RuleValue = "OUR", RuleRefNo = Guid.NewGuid() }
                    }
                },
                new RuleSet
                {
                    RuleSetTypeRefNo = Guid.NewGuid(),
                    RuleSetRefNo = Guid.NewGuid(),
                    Rules = new List<Rule>
                    {
                        new Rule { RuleField = "Charges_Details", RuleOperator = "Equal", RuleValue = "OUR", RuleRefNo = Guid.NewGuid() },
                        new Rule { RuleField = "Sender", RuleOperator = "Equal", RuleValue = "BARCGB22", RuleRefNo = Guid.NewGuid() },
                    }
                },
                new RuleSet
                {
                    RuleSetTypeRefNo =  Guid.NewGuid(),
                    RuleSetRefNo = Guid.NewGuid(),
                    Rules = new List<Rule>
                    {
                        new Rule { RuleField = "Charges_Details", RuleOperator = "Equal", RuleValue = "BEN|SHA", RuleRefNo = Guid.NewGuid() }
                    }
                }
            };
        public static List<RuleSet> PotentialConflicts_TRI_STP =>
        new List<RuleSet>
        {
                new RuleSet
                {
                    RuleSetTypeRefNo = Guid.NewGuid(),
                    RuleSetRefNo = Guid.NewGuid(),
                    Rules = new List<Rule>
                    {
                        new Rule { RuleField = "Charges_Details", RuleOperator = "Equal", RuleValue = "OUR", RuleRefNo = Guid.NewGuid() },
                        new Rule { RuleField = "Sender", RuleOperator = "Equal", RuleValue = "BARCGB22", RuleRefNo = Guid.NewGuid() },
                    }
                }
        };

        public static RuleSet EvaluateRuleSet =>
            new RuleSet
            {
                RuleSetTypeRefNo = Guid.NewGuid(),
                RuleSetRefNo = Guid.NewGuid(),
                Rules = new List<Rule>
                {
                    new Rule
                    {
                        RuleField = "Charges_Details", RuleOperator = "Equal", RuleValue = "OUR",
                        RuleRefNo = Guid.NewGuid()
                    },
                    new Rule
                    {
                        RuleField = "Sender", RuleOperator = "Equal", RuleValue = "BARCGB22", RuleRefNo = Guid.NewGuid()
                    },
                }
            };
        public static RuleSet EvaluateRuleSetValue =>
            new RuleSet
            {
                RuleSetTypeRefNo = Guid.NewGuid(),
                RuleSetRefNo = Guid.NewGuid(),
                Rules = new List<Rule>
                {
                    new Rule
                    {
                        RuleField = "Value", RuleOperator = "GreaterThan", RuleValue = "10000", RuleRefNo = Guid.NewGuid()
                    }
                    ,
                    new Rule
                    {
                        RuleField = "Sender", RuleOperator = "Equal", RuleValue = "BARCGB22", RuleRefNo = Guid.NewGuid()
                    }
                }
            };        
        
        public static RuleSet EvaluateRuleSetDate =>
            new RuleSet
            {
                RuleSetTypeRefNo = Guid.NewGuid(),
                RuleSetRefNo = Guid.NewGuid(),
                Rules = new List<Rule>
                {
                    new Rule
                    {
                        RuleField = "MyDate", RuleOperator = "GreaterThan", RuleValue = "01 Jan 2023", RuleRefNo = Guid.NewGuid()
                    }
                }
            };
    }
}
