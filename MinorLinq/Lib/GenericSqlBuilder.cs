using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;

namespace MinorLinq.Lib
{
    public class GenericSqlBuilder
    {
        private string paramPrefix = "@";
        private List<(string, object)> parameters = new List<(string, object)>();
        
        public string Sql { get; set; }

        public GenericSqlBuilder()
        {
            Sql = "";
        }

        public GenericSqlBuilder(string prefix)
        {
            Sql = "";
            paramPrefix = prefix;
        }

        public GenericSqlBuilder(string prefix, string existingSql)
        {
            Sql = existingSql;
            paramPrefix = prefix;
        }        
        public GenericSqlBuilder(string prefix, string existingSql, List<(string, object)> parameters)
        {
            Sql = existingSql;
            paramPrefix = prefix;
            this.parameters = parameters;
        }

        public GenericSqlBuilder GenerateSelect(string tableName, string[] selects)
        {
            string sql = "SELECT ";
            var first = true;
            foreach (var select in selects) 
            {
                var prefix = first ? "" : ",";
                sql += $"{prefix}t.\"{select}\"";
                first = false;
            }
            sql += $" FROM \"{tableName}\" as t";
            
            return new GenericSqlBuilder(paramPrefix, sql);
        }

        public GenericSqlBuilder GenerateWhere(IEnumerable<QueryCondition> conditions)
        {
            if (string.IsNullOrEmpty(Sql)) throw new Exception("A Select statement needs to be generated before you can use the other operators");
            
            var sql = "";
            var conditionParams = new List<(string, object)>();
            var first = true;
            foreach (var condition in conditions) 
            {
                // For now we don't support having both members be a column name
                if (condition.LeftMember.IsColumn && condition.RightMember.IsColumn) throw new MinorLinqTranslateException("Both members of the where condition cannot be column names at the same time!");

                if (first) sql += "WHERE ";

                string column;
                object value;
                var prefix = first ? "" : " AND ";
                var conditionOperator = Utils.GetConditionOperator(condition.OperatorType);
                if (condition.LeftMember.IsColumn) 
                {
                    column = condition.LeftMember.Value;
                    value = condition.RightMember.ValueRaw;
                    sql += $"{prefix}t.\"{column}\" {conditionOperator} @{column}";
                }
                else 
                {
                    column = condition.RightMember.Value;
                    value = condition.LeftMember.ValueRaw;
                    sql += $"{prefix}{paramPrefix}{column} {conditionOperator} t.\"{column}\"";
                }
                first = false;

                // We will bind the actual value later
                conditionParams.Add((paramPrefix + column, value));
            }

            var test = Sql + " " + sql;
            return new GenericSqlBuilder(paramPrefix, Sql + " " + sql, conditionParams);
        }

        public GenericSqlBuilder GenerateOrderBy((string,bool)[] orderByColumns)
        {
            if (orderByColumns.Length == 0) return new GenericSqlBuilder(paramPrefix, Sql, parameters);

            var sql = "ORDER BY ";
            var first = true;
            foreach (var column in orderByColumns)
            {
                var prefix = first ? "" : ",";
                var suffix = column.Item2 ? "DESC" : "";
                sql += $"{prefix}t.\"{column.Item1}\" {suffix}";
                first = false;
            }
            
            return new GenericSqlBuilder(paramPrefix, Sql + " " + sql, parameters);
        }

        public SqlBuilderResult GetSql()
        {
            return new SqlBuilderResult()
            {
                Sql = Sql,
                Parameters = parameters
            };
        }
    }
    
    public class SqlBuilderResult
    {
        public string Sql { get; set; }
        public IEnumerable<(string, object)> Parameters { get; set; }
    }
}