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
        protected QueryWhereCondition[] where;

        public Query(string tableName, string[] selects, QueryWhereCondition[] where)
        {
            this.tableName = tableName;
            this.selects = selects;
            this.where = where;
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
            return new Query<TEntity>(entityName, properties.ToArray(), where);
        }

        public Query<TEntity> Where(Expression<Func<TEntity, bool>> whereFunc)
        {
            // If expression is not a BinaryExpression then we cannot procces it.
            var binaryExpr = whereFunc.Body as BinaryExpression;
            if (binaryExpr == null) throw new ArgumentException("The provided expression was invalid!");

            // Convert the left part of the expression to MemberExpression and get it's value
            var leftMember = binaryExpr.Left as MemberExpression;
            if (leftMember == null) throw new ArgumentException("The left member of the expression was not a valid MemberExpression!");
            var whereCondition = new QueryWhereCondition { LeftMember = leftMember.Member.Name};

            // Figure out if the right part is a constant or a member and get the value
            switch (binaryExpr.Right.NodeType) 
            {
                case ExpressionType.MemberAccess:
                    var rightMember = binaryExpr.Right as MemberExpression;
                    var valueTuple = GetMemberExpressionValue(binaryExpr.Right);
                    whereCondition.RightMember = valueTuple.Item1;
                    whereCondition.RightType = valueTuple.Item2;
                    break;
                case ExpressionType.Constant:
                    var rightConstant = binaryExpr.Right as ConstantExpression;
                    whereCondition.RightMember = rightConstant.Value.ToString();
                    whereCondition.RightType = rightConstant.Type;
                    break;
                default:
                    throw new ArgumentException("Right member of expression had invalid NodeType!");
            }
        
            // Get details about the entity
            var entityProps = typeof(TEntity).GetProperties();
            var leftMemberIsValid = entityProps.Select(x => x.Name == whereCondition.LeftMember).Any();

            // If the left member doesn't match an actual entity property then the condition is invalid
            if (!leftMemberIsValid) return new Query<TEntity>(tableName, selects, new QueryWhereCondition[0]);

            return new Query<TEntity>(tableName, selects, new QueryWhereCondition[] { whereCondition });
        }

        protected (string, Type) GetMemberExpressionValue(Expression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            var value = getter();
            return (value.ToString(), value.GetType());
        }
    }

    public class QueryWhereCondition 
    {
        public string LeftMember { get; set; }
        public Type LeftType { get; set; }
        public ExpressionType OperatorType { get; set; }
        public string RightMember { get; set; }
        public Type RightType { get; set; }
    }
}