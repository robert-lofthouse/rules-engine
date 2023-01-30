
using System.Collections.Generic;
using Domain.RulesEngine.Enums;

namespace Domain.RulesEngine.Models
{
    public class RuleSetEvaluationData
    {
        public string RuleSetType { get; set; }
        public RulesEngineEvaluationType EvaluationType { get; set; }
        public Dictionary<string, string> ConditionData { get; set; }

    }
}
