using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Domain.RulesEngine.Business
{
    public static class EvaluateRegEx
    {
        public static bool DoRegExIsMatch(Dictionary<string,string> dataToEval, DictionaryEntry ruleToEval)
        {
            return dataToEval.ContainsKey(ruleToEval.Key.ToString()) && Regex.IsMatch(dataToEval[ruleToEval.Key.ToString()], ruleToEval.Value.ToString());
        }
    }
}
