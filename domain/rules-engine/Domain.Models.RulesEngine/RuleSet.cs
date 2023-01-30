using System;
using System.Collections.Generic;

namespace Domain.RulesEngine.Models

{
    public class RuleSet 
    {
        public Guid? RuleSetRefNo { get; set; }
        public string RuleSetName { get; set; }
        public Guid RuleSetTypeRefNo { get; set; }
        public int RuleSetRanking { get; set; }
        public int RuleSetTypeRanking { get; set; }
        public List<Rule> Rules { get; set; }
    }
}
