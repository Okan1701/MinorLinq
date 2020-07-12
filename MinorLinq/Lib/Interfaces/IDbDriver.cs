using System.Collections.Generic;
using System.Data;

namespace MinorLinq.Lib.Interfaces
{
    public interface IDbDriver
    {
        void OpenConnection(string connection);
        void CloseConnection();
        (IDataReader, IEnumerable<string>) ExecuteQuery(string tableName, string[] selects, QueryCondition[] conditions);
    }
}