
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Domain.RulesEngine.Models;

namespace Domain.RulesEngine.Business
{
    public class PrecompiledRules
    {
        ///
        /// A method used to precompile rules for a provided type
        /// 
//        public static List<Func<T, bool>> CompileRule<T>(List<T> targetEntity, List<Rule> rules)
        public static List<Func<RuleSetEvaluationData, bool>> CompileRule(List<Rule> rules)
        {


            var compiledRules = new List<Func<RuleSetEvaluationData, bool>>();

            // Loop through the rules and compile them against the properties of the supplied shallow object 
            rules.ForEach(rule =>
            {

                var ruleValue = rule.RuleOperator == "NotEqual" ? string.Concat("(?!", rule.RuleValue, ").*") : rule.RuleValue;

                var genericType = Expression.Parameter(typeof(RuleSetEvaluationData));
                var key = MemberExpression.Property(genericType, "ConditionData");

                var propertyType = typeof(DictionaryEntry);

                var value = Expression.Constant(Convert.ChangeType(new DictionaryEntry(rule.RuleField, $"(?i)({ruleValue})"), propertyType));
                
                MethodInfo method = typeof(EvaluateRegEx).GetMethod("DoRegExIsMatch", new []{typeof(Dictionary<string,string>), typeof(DictionaryEntry)});
                
                var binaryExpression = Expression.MakeBinary(ExpressionType.Equal, key, value ,false, method);

                compiledRules.Add(Expression.Lambda<Func<RuleSetEvaluationData, bool>>(binaryExpression, genericType).Compile());
            });

            // Return the compiled rules to the caller
            return compiledRules;

        }

    }
}