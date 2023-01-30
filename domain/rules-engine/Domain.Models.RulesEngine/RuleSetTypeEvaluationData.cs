
using System.Collections.Generic;
using Domain.RulesEngine.Enums;

namespace Domain.RulesEngine.Models
{
    public class RuleSetTypeEvaluationData : EvaluationData
    {
        public string RuleSetType { get; set; }
        public RulesEngineEvaluationType EvaluationType { get; set; }

    }
}
