using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

        public (IDataReader,IEnumerable<string>) ExecuteQuery(string tableName, string[] selects, QueryCondition[] conditions)
        {
            var cmd = CreateNpgsqlCommand(tableName, selects, conditions);
            var reader = cmd.ExecuteReader();
            
            // get the sql statements
            var statements = reader.Statements.Select(stmt => stmt.SQL).AsEnumerable();

            return (reader, statements);
        }
        
        private NpgsqlCommand CreateNpgsqlCommand(string tableName, string[] selects, QueryCondition[] conditions) 
        {
            // Build the SELECT .. FROM part of the query first
            string sql = "SELECT ";
            var first = true;
            foreach (var select in selects) 
            {
                var prefix = first ? "" : ",";
                sql += $"{prefix}t.\"{select}\"";
                first = false;
            }
            sql += $" FROM \"{tableName}\" as t";

            // Now we handle the where conditions if any are present
            var conditionParams = new List<(string, object)>();
            first = true;
            foreach (var condition in conditions) 
            {
                // For now we don't support having both members be a column name
                if (condition.LeftMember.IsColumn && condition.RightMember.IsColumn) throw new MinorLinqTranslateException("Both members of the where condition cannot be column names at the same time!");

                if (first) sql += " WHERE ";

                string column;
                object value;
                if (condition.LeftMember.IsColumn) 
                {
                    column = condition.LeftMember.Value;
                    value = condition.RightMember.ValueRaw;
                }
                else 
                {
                    column = condition.RightMember.Value;
                    value = condition.LeftMember.ValueRaw;
                }

                var prefix = first ? "" : " AND ";
                sql += $"{prefix}t.\"{column}\" = @{column}";
                first = false;

                // We will bind the actual value later
                conditionParams.Add(("@" + column, value));
            }

            // Create the Npgsql command
            var cmd = new NpgsqlCommand(sql, dbConnection);

            // Bind the where condition values
            foreach (var condition in conditionParams) 
            {
                cmd.Parameters.AddWithValue(condition.Item1, condition.Item2);
            }

            return cmd;
        }
    }
}