using System;

namespace Domain.RulesEngine.Models

{
    public class RuleSetType
    {
        public Guid RuleSetTypeRefNo { get; set; }
        public string RuleSetTypeName { get; set; }
        public int RuleSetTypeRanking { get; set; }
        public Guid RuleSetCategoryRefNo { get; set; }

    }
}
