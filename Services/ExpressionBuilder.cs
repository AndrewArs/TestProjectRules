using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DomainModels.Models;
using Services.Helpers;

namespace Services
{
    public class ExpressionBuilder<T>
    {
        public Expression<Func<T, bool>> BuildExpression(Rule rule)
        {
            var param = Expression.Parameter(typeof(T), "t");
            if (rule.Conditions.Length == 0)
            {
                return null;
            }

            var exp = GetExpression(param, rule.Conditions[0]);

            if (rule.Conditions.Length == 1)
            {
                return Expression.Lambda<Func<T, bool>>(exp, param);
            }

            foreach (var condition in rule.Conditions.Skip(1))
            {
                var condExp = GetExpression(param, condition);

                switch (rule.Operator)
                {
                    case Constants.OrOperator:
                        exp = Expression.Or(exp, condExp);
                        break;
                    case Constants.AndOperator:
                        exp = Expression.And(exp, condExp);
                        break;
                    default: throw new ArgumentException($"Wrong operator: {rule.Operator}");
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private static Expression GetExpression(Expression param, ConditionFilter condition)
        {
            var propName = CaseHelper.SnakeCaseToPascalCase(condition.Key);
            var left = Expression.Property(param, propName);
            var right = Expression.Constant(condition.Val);

            switch (condition.Condition)
            {
                case Constants.EqualsCondition:
                    return Expression.Equal(left, right);
                case Constants.InArrayCondition:
                    var method = typeof(List<long>).GetMethod("Contains");
                    return Expression.Call(left, method, right);
                case Constants.MoreThanCondition:
                    return Expression.GreaterThan(left, right);
                case Constants.LessThanCondition:
                    return Expression.LessThan(left, right);
                default: throw new ArgumentException($"Wrong condition: {condition.Condition}");
            }
        }
    }
}
