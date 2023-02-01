
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
                //get the type of the parent object that contains the dictionary of items to evaluate
                var genericType = Expression.Parameter(typeof(T));

                //get the dictionary property type from the parent object
                var dictionary = Expression.Property(genericType, "ConditionData");

                BinaryExpression binaryExpression;

                if (string.Equals(rule.RuleOperator, "regex", StringComparison.Ordinal))
                {
                    // get an expression that points to the method call that will do a regex evaluation
                    var method = typeof(EvaluateRegEx).GetMethod("DoRegExIsMatch", new[] { typeof(Dictionary<string, string>), typeof(DictionaryEntry) });

                    // get a value expression that contains the value of the rule to be checked
                    var value = Convert.ChangeType(new DictionaryEntry(rule.RuleField, rule.RuleValue), typeof(DictionaryEntry));

                    // build a full expression that does the regex match between the rule value and the value to be checked
                    binaryExpression = Expression.MakeBinary(ExpressionType.Equal,
                        dictionary,
                        Expression.Constant(value),
                        false,
                        method);
                }
                else
                {
                    // get the reference to the rule field in the dictionary
                    var key = Expression.Constant(rule.RuleField);
                    // get the reference to the rule value in the dictionary
                    var value = Expression.Property(dictionary, "Item", key);

                    //create a constant expression that has the same type as the rule value. It needs to be parsed because the value in the dictionary is string. 
                    // only DateTime, Int and string are supported at the moment
                    var constant = int.TryParse(rule.RuleValue, out int result)
                        ? Expression.Constant(result)
                        : DateTime.TryParse(rule.RuleValue, out DateTime dateResult)
                            ? Expression.Constant(dateResult)
                            : Expression.Constant(rule.RuleValue);

                    // build a full expression that does the comparision using the rule operator, between the rule value and the value to be checked
                    // the value to be checked needs to be parsed to the same type as the rule value, otherwise the comparison can't happen
                    // the value to be checked does not need to be parsed if the rule value is a string
                    binaryExpression = Expression.MakeBinary(
                        Enum.Parse<ExpressionType>(rule.RuleOperator),
                        constant.Type == typeof(string) ? value : Expression.Call(
                                constant.Type.GetMethod("Parse", new[] { typeof(string) }),
                                value),
                        constant);

                }

                compiledRules.Add(Expression.Lambda<Func<T, bool>>(binaryExpression, genericType).Compile());
            });

            // Return the compiled rules to the caller
            return compiledRules;

        }
    }
}
