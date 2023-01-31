
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
                var genericType = Expression.Parameter(typeof(T));
                var dictionary = Expression.Property(genericType, "ConditionData");

                BinaryExpression binaryExpression = null;
                var method = typeof(EvaluateRegEx).GetMethod("DoRegExIsMatch", new[] { typeof(Dictionary<string, string>), typeof(DictionaryEntry) });
    
                if (rule.RuleOperator is "NotEqual" or "Equal")
                {
                    var value = Convert.ChangeType(new DictionaryEntry(rule.RuleField,
                            rule.RuleOperator == "NotEqual"
                                ? $"(?!{rule.RuleValue}).*"
                                : rule.RuleValue),
                        typeof(DictionaryEntry));
                    binaryExpression = Expression.MakeBinary(ExpressionType.Equal, dictionary, Expression.Constant(value), false, method);
                }
                else
                {
                    var key = Expression.Constant(rule.RuleField);
                    var value = Expression.Property(dictionary, "Item", key);

                    var constant = int.TryParse(rule.RuleValue, out int result)
                        ? Expression.Constant(result)
                        : (DateTime.TryParse(rule.RuleValue, out DateTime dateResult)
                            ? Expression.Constant(dateResult)
                            : null);

                    if (constant != null)
                    {
                        binaryExpression = Expression.MakeBinary(
                            Enum.Parse<ExpressionType>(rule.RuleOperator),
                            Expression.Call(
                                    constant.Type.GetMethod("Parse", new[] { typeof(string) }),
                                    value),
                            constant);
                    }
                }

                if (binaryExpression != null)
                    compiledRules.Add(Expression.Lambda<Func<T, bool>>(binaryExpression, genericType).Compile());
            });


            // Return the compiled rules to the caller
            return compiledRules;

        }
    }
}
