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
        
        public string DriverName { get; set; } = "Sqlite v1.0.0";
        public string ConnectionString { get; set; }

        public void OpenConnection()
        {
            dbConnection = new SqliteConnection(ConnectionString);
            dbConnection.Open();
        }

        public void CloseConnection()
        {
            dbConnection.Close();
            dbConnection.Dispose();
        }

        public (DbDataReader, IEnumerable<string>) ExecuteQuery(string tableName, string[] selects, QueryCondition[] conditions,
            (string, bool)[] orderByColumns)
        {
            throw new System.NotImplementedException();
        }

        public async Task<(DbDataReader, IEnumerable<string>)> ExecuteQueryAsync(string tableName, string[] selects, QueryCondition[] conditions,
            (string, bool)[] orderByColumns)
        {
            throw new System.NotImplementedException();
        }
    }
}