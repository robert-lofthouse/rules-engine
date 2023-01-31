using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using AutoMapper;
using Domain.RulesEngine.Business;
using Domain.RulesEngine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RulesEngine.Data.Interface;
using Shouldly;

namespace Domain.RulesEngine.Test
{
    [TestClass]
    public class EvaluationTests
    {
        [TestMethod]
        public void EvaluateRuleSets_should_return_a_single_match()
        {
            //EvaluationData class is a pseudo class that contains the information about the EvaluationData that needs to be evaluated 
            var conditionData = new Dictionary<string, string>
            {
                { "PaymentRef", "Trn5" },
                { "BeneficiaryAccountCurrency", "ZAR" },
                { "BeneficiaryType", "CORPORATE" },
                { "ClientCIFKey", "123" },
                { "Charges_Details", "OUR" },
                { "Currency", "USD" },
                { "Sender", "BARCGB22" }
            };


            // Iterate through your list of EvaluationDatas to see which ones meet the rules vs. the ones that don't
            using var mock = AutoMock.GetLoose();

            mock.Mock<IRuleSetRepository>().Setup(c => c.GetRuleSets("TRI_STP")).Returns(TestRuleSets.paymentsRuleSets_TRI_STP);

            var res = mock.Create<RuleSetService>();
            var result = res.EvaluateRuleSets(new RuleSetTypeEvaluationData
            {
                RuleSetType = "TRI_STP", ConditionData = conditionData
            });

            result.Count.ShouldBe(1);
        }

        [TestMethod]
        public void EvaluateRuleSet_should_return_true()
        {
            var conditionData = new Dictionary<string, string>
            {
                { "PaymentRef", "Trn5" },
                { "BeneficiaryAccountCurrency", "ZAR" },
                { "BeneficiaryType", "CORPORATE" },
                { "ClientCIFKey", "123" },
                { "Charges_Details", "OUR" },
                { "Currency", "USD" },
                { "Sender", "BARCGB22" }
            };

            var ruleSetRefNo = Guid.NewGuid();

            using var mock = AutoMock.GetLoose();

            mock.Mock<IRuleSetRepository>().Setup(c => c.GetRuleSet(ruleSetRefNo)).Returns(TestRuleSets.EvaluateRuleSet);

            var res = mock.Create<RuleSetService>();
            var result = res.EvaluateRuleSet(new RuleSetEvaluationData
            {
                RuleSetRefNo = ruleSetRefNo, ConditionData = conditionData
            });

            result.ShouldBeTrue();
        }

        [TestMethod]
        public void EvaluateRuleSet_should_return_false()
        {
            var conditionData = new Dictionary<string, string>
            {
                { "Charges_Details", "OUR" },
                { "Sender", "BARCGB2Z" }
            };

            var ruleSetRefNo = Guid.NewGuid();

            using var mock = AutoMock.GetLoose();

            mock.Mock<IRuleSetRepository>().Setup(c => c.GetRuleSet(ruleSetRefNo)).Returns(TestRuleSets.EvaluateRuleSet);

            var res = mock.Create<RuleSetService>();
            var result = res.EvaluateRuleSet(new RuleSetEvaluationData
            {
                RuleSetRefNo = ruleSetRefNo, ConditionData = conditionData
            });

            result.ShouldBeFalse();
        }

        [TestMethod]
        public void EvaluateRuleSet_should_return_false_if_the_compared_value_is_not_greater_than_the_rule_value()
        {
            var conditionData = new Dictionary<string, string>
            {
                { "Value", "9000" }
            };

            var ruleSetRefNo = Guid.NewGuid();

            using var mock = AutoMock.GetLoose();

            mock.Mock<IRuleSetRepository>().Setup(c => c.GetRuleSet(ruleSetRefNo)).Returns(TestRuleSets.EvaluateRuleSetValue);

            var res = mock.Create<RuleSetService>();
            var result = res.EvaluateRuleSet(new RuleSetEvaluationData
            {
                RuleSetRefNo = ruleSetRefNo, ConditionData = conditionData
            });

            result.ShouldBeFalse();
        }

        [TestMethod]
        public void EvaluateRuleSet_should_return_true_if_the_compared_value_is_greater_than_the_rule_value()
        {
            var conditionData = new Dictionary<string, string>
            {
                { "Sender", "BARCGB22" },
                { "Value", "11000" }
            };

            var ruleSetRefNo = Guid.NewGuid();

            using var mock = AutoMock.GetLoose();

            mock.Mock<IRuleSetRepository>().Setup(c => c.GetRuleSet(ruleSetRefNo)).Returns(TestRuleSets.EvaluateRuleSetValue);

            var res = mock.Create<RuleSetService>();
            var result = res.EvaluateRuleSet(new RuleSetEvaluationData
            {
                RuleSetRefNo = ruleSetRefNo, ConditionData = conditionData
            });

            result.ShouldBeTrue();
        }

        [TestMethod]
        public void EvaluateRuleSet_should_return_false_if_the_compared_value_is_greater_than_the_rule_value_but_the_bic_doesnot_match()
        {
            var conditionData = new Dictionary<string, string>
            {
                { "Sender", "BARCGB2z" },
                { "Value", "11000" }
            };

            var ruleSetRefNo = Guid.NewGuid();

            using var mock = AutoMock.GetLoose();

            mock.Mock<IRuleSetRepository>().Setup(c => c.GetRuleSet(ruleSetRefNo)).Returns(TestRuleSets.EvaluateRuleSetValue);

            var res = mock.Create<RuleSetService>();
            var result = res.EvaluateRuleSet(new RuleSetEvaluationData
            {
                RuleSetRefNo = ruleSetRefNo, ConditionData = conditionData
            });

            result.ShouldBeFalse();
        }

        [TestMethod]
        public void EvaluateRuleSet_should_return_false_if_a_date_value_is_before_the_rule_value()
        {
            var conditionData = new Dictionary<string, string>
            {
                { "MyDate", "10 Mar 2010" }
            };

            var ruleSetRefNo = Guid.NewGuid();

            using var mock = AutoMock.GetLoose();

            mock.Mock<IRuleSetRepository>().Setup(c => c.GetRuleSet(ruleSetRefNo)).Returns(TestRuleSets.EvaluateRuleSetDate);

            var res = mock.Create<RuleSetService>();
            var result = res.EvaluateRuleSet(new RuleSetEvaluationData
            {
                RuleSetRefNo = ruleSetRefNo, ConditionData = conditionData
            });

            result.ShouldBeFalse();
        }
        [TestMethod]
        public void EvaluateRuleSet_should_return_true_if_a_date_value_is_after_the_rule_value()
        {
            var conditionData = new Dictionary<string, string>
            {
                { "MyDate", "10 Mar 2023" }
            };

            var ruleSetRefNo = Guid.NewGuid();

            using var mock = AutoMock.GetLoose();

            mock.Mock<IRuleSetRepository>().Setup(c => c.GetRuleSet(ruleSetRefNo)).Returns(TestRuleSets.EvaluateRuleSetDate);

            var res = mock.Create<RuleSetService>();
            var result = res.EvaluateRuleSet(new RuleSetEvaluationData
            {
                RuleSetRefNo = ruleSetRefNo, ConditionData = conditionData
            });

            result.ShouldBeTrue();
        }
    }
}
