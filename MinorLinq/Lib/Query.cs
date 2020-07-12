using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using MinorLinq.Lib;

namespace MinorLinq.Lib
{
    public class Query<TEntity> where TEntity : class, new()
    {
        protected string tableName;
        protected string[] selects;
        protected QueryCondition[] where;
        protected IDataContext context;

        public Query() {  }

        public Query(string tableName, string[] selects, QueryCondition[] where)
        {
            this.tableName = tableName;
            this.selects = selects;
            this.where = where;
        }

        public Query(string tableName, string[] selects, QueryCondition[] where, IDataContext context)
        {
            this.tableName = tableName;
            this.selects = selects;
            this.where = where;
            this.context = context;
        }

        public Query<TEntity> Select(Func<TEntity, object> selectFunc)
        {
            TEntity emptyEntity = new TEntity();
            object selectRes = selectFunc(emptyEntity);
            var properties = new List<string>();
            var entityName = emptyEntity.GetType().Name;
            foreach (var prop in selectRes.GetType().GetProperties())
            {
                properties.Add(prop.Name);
            }
            return new Query<TEntity>(entityName, properties.ToArray(), where, context);
        }

        public Query<TEntity> Where(Expression<Func<TEntity, bool>> whereFunc)
        {
            // If expression is not a BinaryExpression then we cannot procces it.
            var binaryExpr = whereFunc.Body as BinaryExpression;
            if (binaryExpr == null) throw new ArgumentException("The provided expression was invalid!");

            // Proccess both sides of the expression and extract the values
            var columnNames = typeof(TEntity).GetProperties().Select(x => x.Name).ToArray();
            var whereCondition = new QueryCondition 
            { 
                LeftMember = ProccesConditionalExpressionMember(binaryExpr.Left, columnNames),
                RightMember = ProccesConditionalExpressionMember(binaryExpr.Right, columnNames),
                OperatorType = binaryExpr.NodeType 
            };

            return new Query<TEntity>(tableName, selects, new QueryCondition[] { whereCondition }, context);
        }

        public List<TEntity> ToList() 
        {
            return context.ExecuteQuery<TEntity>(tableName, selects, where);
        }

        protected QueryConditionMember ProccesConditionalExpressionMember(Expression expression, string[] columns) 
        {
            // First check if it contains a column name
            if (expression.NodeType == ExpressionType.MemberAccess) 
            {
                var member = expression as MemberExpression;
                var name = member.Member.Name;
                if (columns.Any(x => x == name)) 
                {
                    return new QueryConditionMember 
                    {
                        Value = name,
                        ValueType = name.GetType(),
                        IsColumn = true
                    };
                }
            }

            // Process the expression and extract the value and valuetype
            var conditionMember = new QueryConditionMember { IsColumn = false };
            switch (expression.NodeType) 
            {
                case ExpressionType.MemberAccess:
                    var rightMember = expression as MemberExpression;
                    conditionMember = GetMemberExpressionValue(expression);
                    break;
                case ExpressionType.Constant:
                    var constant = expression as ConstantExpression;
                    conditionMember.Value = constant.Value.ToString();
                    conditionMember.ValueRaw = constant.Value;
                    conditionMember.ValueType = constant.Type;
                    break;
                default:
                    throw new ArgumentException($"Expression has invalid NodeType! ({expression.NodeType.ToString()})");
            }

            return conditionMember;
        }

        protected QueryConditionMember GetMemberExpressionValue(Expression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            object value = getter();
            return new QueryConditionMember 
            {
                ValueRaw = value,
                Value = value.ToString(),
                ValueType = value.GetType(),
                IsColumn = false
            };
        }
    }

    public class QueryCondition 
    {
        public QueryConditionMember LeftMember { get; set; }
        public QueryConditionMember RightMember { get; set; }
        public ExpressionType OperatorType { get; set; }
    }

    public class QueryConditionMember 
    {
        public string Value { get; set; }
        public object ValueRaw { get; set; }
        public Type ValueType { get; set; }
        public bool IsColumn { get; set; }
    }
}