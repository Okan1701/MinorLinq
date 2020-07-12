using System.Collections.Generic;
using System.Data;

namespace MinorLinq.Lib.Interfaces
{
    public interface IDbDriver
    {
        string DriverName { get; set; }
        string ConnectionString { get; set; }
        void OpenConnection();
        void CloseConnection();
        (IDataReader, IEnumerable<string>) ExecuteQuery(string tableName, string[] selects, QueryCondition[] conditions);
    }
}