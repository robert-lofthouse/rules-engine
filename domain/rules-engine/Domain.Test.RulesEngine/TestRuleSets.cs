//using System;
//using System.Collections.Generic;

//namespace Domain.RulesEngine.Test
//{
//    public static class TestRuleSets
//    {

//        public static List<RulesEngineRuleSet> paymentsRuleSets_TRI_STP =>
//            new List<RulesEngineRuleSet>
//            {
//                new RulesEngineRuleSet
//                {
//                    RuleSetType = "TRI_STP", EntityInstRefNo = Guid.NewGuid(),
//                    RulesEngineRules = new List<RulesEngineRuleSetRule>
//                    {
//                        new RulesEngineRuleSetRule
//                        {
//                            RulesEngineRule =
//                                new RulesEngineRule
//                                {
//                                    RuleField = "Charges_Details", RuleOperator = "Equal", RuleValue =
//                                        "BEN|SHA"
//                                }
//                        },
//                        new RulesEngineRuleSetRule
//                        {
//                            RulesEngineRule =
//                                new RulesEngineRule
//                                {
//                                    RuleField = "ClientCIFKey", RuleOperator = "Equal", RuleValue =
//                                        "150000990"
//                                }
//                        },
//                        new RulesEngineRuleSetRule
//                        {
//                            RulesEngineRule =
//                                new RulesEngineRule
//                                {
//                                    RuleField = "BeneficiaryAccountCurrency",
//                                    RuleOperator = "Equal", RuleValue = "MUR"
//                                }
//                        }
//                    }
//                },
//                new RulesEngineRuleSet
//                {
//                    RuleSetType = "TRI_STP", EntityInstRefNo = Guid.NewGuid(),
//                    RulesEngineRules = new List<RulesEngineRuleSetRule>
//                    {
//                        new RulesEngineRuleSetRule
//                        {
//                            RulesEngineRule =
//                                new RulesEngineRule
//                                {
//                                    RuleField = "Charges_Details",
//                                    RuleOperator = "Equal", RuleValue = "OUR"
//                                }
//                        }
//                    }
//                },
//                new RulesEngineRuleSet
//                {
//                    RuleSetType = "TRI_STP", EntityInstRefNo = Guid.NewGuid(),
//                    RulesEngineRules = new List<RulesEngineRuleSetRule>
//                    {
//                        new RulesEngineRuleSetRule
//                        {
//                            RulesEngineRule =
//                                new RulesEngineRule
//                                {
//                                    RuleField = "Charges_Details",
//                                    RuleOperator = "Equal", RuleValue = "OUR"
//                                }
//                        },
//                        new RulesEngineRuleSetRule
//                        {
//                            RulesEngineRule =
//                                new RulesEngineRule
//                                {
//                                    RuleField = "Sender",
//                                    RuleOperator = "Equal", RuleValue = "BARCGB22"
//                                }
//                        },
//                    }
//                },
//                new RulesEngineRuleSet
//                {
//                    RuleSetType = "TRI_STP", EntityInstRefNo = Guid.NewGuid(),
//                    RulesEngineRules = new List<RulesEngineRuleSetRule>
//                    {
//                        new RulesEngineRuleSetRule
//                        {
//                            RulesEngineRule =
//                                new RulesEngineRule
//                                {
//                                    RuleField = "Charges_Details",
//                                    RuleOperator = "Equal", RuleValue = "BEN|SHA"
//                                }
//                        }
//                    }
//                }
//            };
//            public static List<RulesEngineRuleSet> PotentialConflicts_TRI_STP =>
//            new List<RulesEngineRuleSet>
//            {
//                new RulesEngineRuleSet
//                {
//                    RuleSetType = "TRI_STP", EntityInstRefNo = Guid.NewGuid(),
//                    RulesEngineRules = new List<RulesEngineRuleSetRule>
//                    {
//                        new RulesEngineRuleSetRule
//                        {
//                            RulesEngineRule =
//                                new RulesEngineRule
//                                {
//                                    RuleField = "Charges_Details",
//                                    RuleOperator = "Equal", RuleValue = "OUR"
//                                }
//                        },
//                        new RulesEngineRuleSetRule
//                        {
//                            RulesEngineRule =
//                                new RulesEngineRule
//                                {
//                                    RuleField = "Sender",
//                                    RuleOperator = "Equal", RuleValue = "BARCGB22"
//                                }
//                        },
//                    }
//                }
//            };
//    }
//}
