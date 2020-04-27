using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace TDSDTO.Filter
{
    public abstract class Filter
    {
        public Func<T, bool> GetPredicate<T>()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            return Expression
                .Lambda<Func<T, bool>>(GetExpression(parameter), parameter)
                .Compile();
        }

        public bool ExecuteFilter<T>(T obj)
        {
            return GetPredicate<T>()(obj);
        }

        internal abstract Expression GetExpression(ParameterExpression parameter);
    }

    public class FilterConditionCollection : List<Filter>
    {
    }

    public class FieldOperand
    {
        public FieldOperand(string name)
        {
            this.FieldName = name;
        }
        
        public string FieldName { get; set; }
    }

    public class FilterCondition<L, R> : Filter
    {
        public FilterCondition(L leftOperand, R rightOperand, ConditionOperation operation)
        {
            this.LeftOperand = leftOperand;
            this.Operation = operation;
            this.RightOperand = rightOperand;
        }

        public L LeftOperand { get; set; }
        public R RightOperand { get; set; }
        public ConditionOperation Operation { get; set; } = ConditionOperation.Equal;

        internal override Expression GetExpression(ParameterExpression parameter)
        {
            Expression left;
            if (LeftOperand is FieldOperand lo)
            {
                left = Expression.PropertyOrField(parameter, lo.FieldName);
            }
            else
            {
                left = Expression.Constant(LeftOperand);
            }

            Expression right;
            if (RightOperand is FieldOperand ro)
            {
                right = Expression.PropertyOrField(parameter, ro.FieldName);
            }
            else
            {
                right = Expression.Constant(RightOperand);
            }

            return GetOperation(left, right);
        }

        private Expression GetOperation(Expression left, Expression right)
        {
            switch (Operation)
            {
                case ConditionOperation.Less:
                    return Expression.LessThan(left, right);
                case ConditionOperation.LessEqual:
                    return Expression.LessThanOrEqual(left, right);
                case ConditionOperation.Greater:
                    return Expression.GreaterThan(left, right);
                case ConditionOperation.GreaterEqual:
                    return Expression.GreaterThanOrEqual(left, right);
                case ConditionOperation.NotEqual:
                    return Expression.NotEqual(left, right);
                case ConditionOperation.Contains:
                    return Expression.Call(
                        Expression.Call(left, "ToString", null, null),
                        "Contains", null, 
                        Expression.Call(right, "ToString", null, null));

            };
            return Expression.Equal(left, right);
        }
    }

    public class FilterConditionGroup : Filter
    { 
        public FilterConditionCollection Conditions { get; set; }
        public ConditionGroupOperation Operation { get; set; } = ConditionGroupOperation.And;

        internal override Expression GetExpression(ParameterExpression parameter)
        {
            if(Conditions == null || Conditions.Count == 0)
                return Expression.Lambda(Expression.Constant(true), parameter);

            var operation = GetOperator();
            var expression = Conditions[0].GetExpression(parameter);
            for(int i = 1; i < Conditions.Count; i++)
            {
                expression = operation(expression, Conditions[i].GetExpression(parameter));
            }
            return expression;
        }

        private Func<Expression, Expression, Expression> GetOperator()
        {
            return Operation == ConditionGroupOperation.Or 
                ? Expression.Or 
                : (Func<Expression, Expression, Expression>)Expression.And;
        }
    }

    public enum ConditionOperation
    {
        Equal,
        Greater,
        Less,
        GreaterEqual,
        LessEqual,
        NotEqual,
        Contains
    }

    public enum ConditionGroupOperation
    {
        And,
        Or
    }
}
