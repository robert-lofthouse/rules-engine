
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Domain.RulesEngine.Models;

namespace Domain.RulesEngine.Business
{
    public static class PrecompiledRules
    {
        /// <summary>
        /// A method used to precompile rules for a provided type
        /// </summary>
        /// <param name="rules">List of predefined rules to be used as a base of the rules compilation</param>
        /// <returns></returns>
        public static List<Func<T, bool>> CompileRule<T>(List<Rule> rules)
        {

            var compiledRules = new List<Func<T, bool>>();

            // Loop through the rules and compile them against the properties of the supplied shallow object 
            rules.ForEach(rule =>
            {

                var ruleValue = rule.RuleOperator == "NotEqual" ? string.Concat("(?!", rule.RuleValue, ").*") : rule.RuleValue;

                var genericType = Expression.Parameter(typeof(T));
                var key = Expression.Property(genericType, "ConditionData");

                var propertyType = typeof(DictionaryEntry);

                var value = Expression.Constant(Convert.ChangeType(new DictionaryEntry(rule.RuleField, $"(?i)({ruleValue})"), propertyType));
                
                MethodInfo method = typeof(EvaluateRegEx).GetMethod("DoRegExIsMatch", new []{typeof(Dictionary<string,string>), typeof(DictionaryEntry)});
                
                var binaryExpression = Expression.MakeBinary(ExpressionType.Equal, key, value ,false, method);

                compiledRules.Add(Expression.Lambda<Func<T, bool>>(binaryExpression, genericType).Compile());
            });

            // Return the compiled rules to the caller
            return compiledRules;

        }
    }
}