using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace MinorLinq.Lib.Interfaces
{
    public interface IDbDriver
    {
        string DriverName { get; set; }
        string ConnectionString { get; set; }
        void OpenConnection();
        void CloseConnection();
        (DbDataReader, IEnumerable<string>) ExecuteQuery(string tableName, string[] selects, QueryCondition[] conditions, (string,bool)[] orderByColumns);
        Task<(DbDataReader, IEnumerable<string>)> ExecuteQueryAsync(string tableName, string[] selects, QueryCondition[] conditions, (string,bool)[] orderByColumns);
    }
}