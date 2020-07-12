using System.Linq.Expressions;

namespace MinorLinq.Lib
{
    public static class Utils
    {
        public static string GetConditionOperator(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Equal: return "=";
                case ExpressionType.NotEqual: return "!=";
                case ExpressionType.GreaterThan: return ">";
                case ExpressionType.GreaterThanOrEqual: return ">=";
                case ExpressionType.LessThan: return "<";
                case ExpressionType.LessThanOrEqual: return "<=";
                default: throw new MinorLinqInvalidExpressionTypeException($"The expression type for the where condition is invalid! ({type.ToString()})");
            }
        }
    }
}