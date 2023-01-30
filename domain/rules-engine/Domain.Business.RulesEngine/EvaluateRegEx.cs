using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain.RulesEngine.Models;

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
