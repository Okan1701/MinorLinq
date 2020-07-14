using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using MinorLinq.Lib.Interfaces;

namespace MinorLinq.Lib.Drivers.Sqlite
{
    public class SqliteDriver : IDbDriver
    {
        private SqliteConnection dbConnection;
        private ILogger logger;
        
        public string DriverName { get; set; } = "Sqlite v1.0.0";
        public string ConnectionString { get; set; }

        public void OpenConnection(ILogger contextLogger)
        {
            dbConnection = new SqliteConnection(ConnectionString);
            dbConnection.Open();
            logger = contextLogger;
        }

        public void CloseConnection()
        {
            dbConnection.Close();
            dbConnection.Dispose();
        }

        public (DbDataReader, IEnumerable<string>) ExecuteQuery(string tableName, string[] selects, QueryCondition[] conditions,
            (string, bool)[] orderByColumns)
        {
            var cmd = CreateSqlCommand(tableName, selects, conditions, orderByColumns);
            var reader = cmd.ExecuteReader();

            return (reader, new []{ cmd.CommandText });
        }

        public async Task<(DbDataReader, IEnumerable<string>)> ExecuteQueryAsync(string tableName, string[] selects, QueryCondition[] conditions,
            (string, bool)[] orderByColumns)
        {
            var cmd = CreateSqlCommand(tableName, selects, conditions, orderByColumns);
            var reader = await cmd.ExecuteReaderAsync();

            return (reader, new []{ cmd.CommandText });
        }

        private SqliteCommand CreateSqlCommand(string tableName, string[] selects, QueryCondition[] conditions,
            (string, bool)[] orderByColumns)
        {
            // Use the sql builder to generate a SQL query string
            SqlBuilderResult query = new GenericSqlBuilder("$")
                .GenerateSelect(tableName, selects)
                .GenerateWhere(conditions)
                .GenerateOrderBy(orderByColumns)
                .GetSql();
            
            // Create the Sqlite command
            var cmd = new SqliteCommand(query.Sql, dbConnection);
            
            // Bind the condition parameter values
            foreach ((string, object) condition in query.Parameters) 
            {
                cmd.Parameters.AddWithValue(condition.Item1, condition.Item2);
            }

            return cmd;
        }
    }
}