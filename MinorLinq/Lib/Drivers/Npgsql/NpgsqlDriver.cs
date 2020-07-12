using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MinorLinq.Lib.Interfaces;

namespace MinorLinq.Lib.Drivers.Npgsql 
{
    public class NpgsqlDriver : IDbDriver 
    {
        private NpgsqlConnection dbConnection;

        public string DriverName { get; set; } = "PostgreSQL v1.0.0";
        public string ConnectionString { get; set; }

        public void OpenConnection() 
        {
            dbConnection = new NpgsqlConnection(ConnectionString);
            dbConnection.Open();
        }

        public void CloseConnection() 
        {
            dbConnection.Close();
            dbConnection.Dispose();
        }

        public (IDataReader,IEnumerable<string>) ExecuteQuery(string tableName, string[] selects, QueryCondition[] conditions, (string, bool)[] orderByColumns)
        {
            var cmd = CreateNpgsqlCommand(tableName, selects, conditions, orderByColumns);
            var reader = cmd.ExecuteReader();
            
            // get the sql statements
            var statements = reader.Statements.Select(stmt => stmt.SQL).AsEnumerable();

            return (reader, statements);
        }
        
        private NpgsqlCommand CreateNpgsqlCommand(string tableName, string[] selects, QueryCondition[] conditions, (string, bool)[] orderByColumns) 
        {
            // Build the SELECT .. FROM part of the query first
            string sql = GenerateSqlSelect(tableName, selects);

            // Now we handle the where conditions if any are present
            var whereStatement = GenerateSqlWhere(conditions);
            sql += " " + whereStatement.Item1;
            
            // Generate the ORDER BY part
            sql += " " + GenerateSqlOrderBy(orderByColumns);

            // Create the Npgsql command
            var cmd = new NpgsqlCommand(sql, dbConnection);

            // Bind the where condition values
            foreach ((string, object) condition in whereStatement.Item2) 
            {
                cmd.Parameters.AddWithValue(condition.Item1, condition.Item2);
            }

            return cmd;
        }

        private string GenerateSqlSelect(string tableName, string[] selects)
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
            return sql;
        }

        private (string,List<(string, object)>) GenerateSqlWhere(QueryCondition[] conditions)
        {
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
                    sql += $"{prefix}@{column} {conditionOperator} t.\"{column}\"";
                }
                first = false;

                // We will bind the actual value later
                conditionParams.Add(("@" + column, value));
            }

            return (sql, conditionParams);
        }

        private string GenerateSqlOrderBy((string,bool)[] orderByColumns)
        {
            if (orderByColumns.Length == 0) return "";

            var sql = "ORDER BY ";
            var first = true;
            foreach (var column in orderByColumns)
            {
                var prefix = first ? "" : ",";
                var suffix = column.Item2 ? "DESC" : "";
                sql += $"{prefix}t.\"{column.Item1}\" {suffix}";
                first = false;
            }

            return sql;
        }
    }
}