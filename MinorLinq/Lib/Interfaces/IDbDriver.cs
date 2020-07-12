using System.Collections.Generic;

namespace MinorLinq.Lib
{
    public interface IDbDriver
    {
        void OpenConnection(string connection);
        void CloseConnection();
        void ExecuteQuery(string tableName, string[] selects, QueryCondition[] conditions);
    }
}