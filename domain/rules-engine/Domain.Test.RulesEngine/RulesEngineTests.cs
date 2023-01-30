//using System;
//using System.Collections.Generic;
//using Autofac.Extras.Moq;
//using AutoMapper;
//using Domain.RulesEngine.Business;
//using Domain.RulesEngine.Models;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using Shouldly;

//namespace Domain.RulesEngine.Test
//{
//    [TestClass]
//    public class RulesEngineTests
//    {
//        [TestMethod]
//        public void TestEvaluation()
//        {
//            //EvaluationData class is a pseudo class that contains the information about the EvaluationData that needs to be evaluated 

//            var edData = new EvaluationData
//            {
//                PaymentRef = "Trn5",
//                BeneficiaryAccountCurrency = "ZAR",
//                BeneficiaryType = "CORPORATE",
//                ClientCIFKey = "123",
//                Charges_Details = "OUR",
//                Currency = "USD",
//                Sender = "BARCGB22"
//            };


//            // Iterate through your list of EvaluationDatas to see which ones meet the rules vs. the ones that don't
//            using (var mock = AutoMock.GetLoose())
//            {


//                mock.Mock<IRulesEngineRuleSetRepo>().Setup(c => c.GetRuleSets()).Returns(TestRuleSets.paymentsRuleSets_TRI_STP);

//                var res = mock.Create<RulesEngineService>();
//                var result = res.EvaluateRuleSets(new RuleSetEvaluationData
//                { RuleSetType = "TRI_STP", ConditionData = edData });

//                ;
//                result.Count.ShouldBe(1);
//            }
//        }

//        [TestMethod]
//        public void TestConflict_should_return_conflict_list_for_full_difference()
//        {


//            using (var mock = AutoMock.GetLoose())
//            {
//                var config = new MapperConfiguration(cfg =>
//                {
//                    // Add all profiles in current assembly
//                    cfg.AddProfile(new MappingProfile());
//                });

//                mock.Provide<IMapper>(new Mapper(config));

//                mock.Mock<IRulesEngineRuleSetRepo>().Setup(c => c.GetRuleSetsForConflicts(It.IsAny<RulesEngineRuleSet>()))
//                    .Returns(new List<RulesEngineRuleSet>
//                    {
//                        new RulesEngineRuleSet
//                        {
//                            RuleSetType = "TRI_STP", EntityInstRefNo = Guid.NewGuid(),
//                            RulesEngineRules = new List<RulesEngineRuleSetRule>
//                            {
//                                new RulesEngineRuleSetRule
//                                {
//                                    RulesEngineRule =
//                                        new RulesEngineRule
//                                        {
//                                            RuleField = "Charges_Details",
//                                            RuleOperator = "Equal", RuleValue = "OUR"
//                                        }
//                                },
//                                new RulesEngineRuleSetRule
//                                {
//                                    RulesEngineRule =
//                                        new RulesEngineRule
//                                        {
//                                            RuleField = "Sender",
//                                            RuleOperator = "Equal", RuleValue = "BARCGB22"
//                                        }
//                                },
//                            }
//                        }
//                    });

//                var res = mock.Create<RulesEngineService>();

//                var result = res.CheckForPossibleConflicts(new RulesEngineRuleSet
//                {
//                    RuleSetName = "Name",EntityInstRefNo = Guid.NewGuid(),RuleSetRanking = 1,RuleSetRefNo = Guid.NewGuid(),RuleSetType = "TRI_STP",
//                    Rules = new List<RulesEngineRule>
//                    {
//                        new RulesEngineRule  {RuleField = "Field1", RuleValue = "Value1",RuleRefNo = Guid.NewGuid(),RuleOperator = "Equal"},
//                        new RulesEngineRule {RuleField = "Field2", RuleValue = "Value1",RuleRefNo = Guid.NewGuid(),RuleOperator = "Equal"}
//                    }
//                });

//                result.Count.ShouldBe(1);

//            }

//        }
//        [TestMethod]
//        public void TestConflict_should_return_conflict_list_for_partial_difference()
//        {


//            using (var mock = AutoMock.GetLoose())
//            {
//                var config = new MapperConfiguration(cfg =>
//                {
//                    // Add all profiles in current assembly
//                    cfg.AddProfile(new MappingProfile());
//                });

//                mock.Provide<IMapper>(new Mapper(config));

//                mock.Mock<IRulesEngineRuleSetRepo>().Setup(c => c.GetRuleSetsForConflicts(It.IsAny<RulesEngineRuleSet>()))
//                    .Returns(new List<RulesEngineRuleSet>
//                    {
//                        new RulesEngineRuleSet
//                        {
//                            RuleSetType = "TRI_STP", EntityInstRefNo = Guid.NewGuid(),
//                            RulesEngineRules = new List<RulesEngineRuleSetRule>
//                            {
//                                new RulesEngineRuleSetRule
//                                {
//                                    RulesEngineRule =
//                                        new RulesEngineRule
//                                        {
//                                            RuleField = "Charges_Details",
//                                            RuleOperator = "Equal", RuleValue = "OUR"
//                                        }
//                                },
//                                new RulesEngineRuleSetRule
//                                {
//                                    RulesEngineRule =
//                                        new RulesEngineRule
//                                        {
//                                            RuleField = "Sender",
//                                            RuleOperator = "Equal", RuleValue = "BARCGB22"
//                                        }
//                                },
//                            }
//                        }
//                    });

//                var res = mock.Create<RulesEngineService>();

//                var result = res.CheckForPossibleConflicts(new RulesEngineRuleSet
//                {
//                    RuleSetName = "Name",EntityInstRefNo = Guid.NewGuid(),RuleSetRanking = 1,RuleSetRefNo = Guid.NewGuid(),RuleSetType = "TRI_STP",
//                    Rules = new List<RulesEngineRule>
//                    {
//                        new RulesEngineRule  {RuleField = "Charges_Details", RuleValue = "OUR",RuleRefNo = Guid.NewGuid(),RuleOperator = "Equal"},
//                        new RulesEngineRule {RuleField = "Field2", RuleValue = "Value1",RuleRefNo = Guid.NewGuid(),RuleOperator = "Equal"}
//                    }
//                });

//                result.Count.ShouldBe(1);

//            }

//        }

//        [TestMethod]
//        public void TestConflict_should_return_No_conflict_list_for_same_fields()
//        {


//            using (var mock = AutoMock.GetLoose())
//            {
//                var config = new MapperConfiguration(cfg =>
//                {
//                    // Add all profiles in current assembly
//                    cfg.AddProfile(new MappingProfile());
//                });

//                mock.Provide<IMapper>(new Mapper(config));

//                mock.Mock<IRulesEngineRuleSetRepo>().Setup(c => c.GetRuleSetsForConflicts(It.IsAny<RulesEngineRuleSet>())).Returns(
//                    new List<RulesEngineRuleSet>
//                    {
//                        new RulesEngineRuleSet
//                        {
//                            RuleSetType = "TRI_STP", EntityInstRefNo = Guid.NewGuid(),
//                            RulesEngineRules = new List<RulesEngineRuleSetRule>
//                            {
//                                new RulesEngineRuleSetRule
//                                {
//                                    RulesEngineRule =
//                                        new RulesEngineRule
//                                        {
//                                            RuleField = "Charges_Details",
//                                            RuleOperator = "Equal", RuleValue = "OUR"
//                                        }
//                                },
//                                new RulesEngineRuleSetRule
//                                {
//                                    RulesEngineRule =
//                                        new RulesEngineRule
//                                        {
//                                            RuleField = "Sender",
//                                            RuleOperator = "Equal", RuleValue = "BARCGB22"
//                                        }
//                                },
//                            }
//                        }
//                    });

//                var res = mock.Create<RulesEngineService>();

//                var result = res.CheckForPossibleConflicts(new RulesEngineRuleSet
//                {
//                    RuleSetName = "Name",EntityInstRefNo = Guid.NewGuid(),RuleSetRanking = 1,RuleSetRefNo = Guid.NewGuid(),RuleSetType = "TRI_STP",
//                    Rules = new List<RulesEngineRule>
//                    {
//                        new RulesEngineRule {RuleField = "Charges_Details", RuleValue = "CD",RuleRefNo = Guid.NewGuid(),RuleOperator = "Equal"},
//                        new RulesEngineRule {RuleField = "Sender",RuleValue = "Send",RuleRefNo = Guid.NewGuid(),RuleOperator = "Equal"}
//                    }
//                });

//                result.Count.ShouldBe(0);

//            }

//        }
//        [TestMethod]
//        public void TestConflict_should_return_no_conflict_list_for_mix()
//        {


//            using (var mock = AutoMock.GetLoose())
//            {
//                var config = new MapperConfiguration(cfg =>
//                {
//                    // Add all profiles in current assembly
//                    cfg.AddProfile(new MappingProfile());
//                });

//                mock.Provide<IMapper>(new Mapper(config));

//                mock.Mock<IRulesEngineRuleSetRepo>().Setup(c => c.GetRuleSetsForConflicts(It.IsAny<RulesEngineRuleSet>())).Returns(
//                    new List<RulesEngineRuleSet>
//                    {
//                        new RulesEngineRuleSet
//                        {
//                            RuleSetType = "TRI_STP", EntityInstRefNo = Guid.NewGuid(),
//                            RulesEngineRules = new List<RulesEngineRuleSetRule>
//                            {
//                                new RulesEngineRuleSetRule {RulesEngineRule = new RulesEngineRule {RuleField = "Currency", RuleOperator = "Equal", RuleValue = "USD"}},
//                                new RulesEngineRuleSetRule {RulesEngineRule = new RulesEngineRule {RuleField = "Sender", RuleOperator = "Equal", RuleValue = "BARCGB22"}
//                                },
//                            }
//                        }
//                    });

//                var res = mock.Create<RulesEngineService>();

//                var result = res.CheckForPossibleConflicts(new RulesEngineRuleSet
//                {
//                    RuleSetName = "Name",EntityInstRefNo = Guid.NewGuid(),RuleSetRanking = 1,RuleSetRefNo = Guid.NewGuid(),RuleSetType = "TRI_STP",
//                    Rules = new List<RulesEngineRule>
//                    {
//                        new RulesEngineRule {RuleField = "Currency", RuleValue = "GBP",RuleRefNo = Guid.NewGuid(),RuleOperator = "Equal"},
//                        new RulesEngineRule {RuleField = "Charges_Details",RuleValue = "OUR",RuleRefNo = Guid.NewGuid(),RuleOperator = "Equal"}
//                    }
//                });

//                result.Count.ShouldBe(0);

//            }

//        }
//    }
//}
