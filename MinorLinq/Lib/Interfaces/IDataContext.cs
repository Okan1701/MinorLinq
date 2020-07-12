using System.Collections.Generic;

namespace MinorLinq.Lib
{
    public interface IDataContext
    {
        List<TEntity> ExecuteQuery<TEntity>(string tableName, string[] selects, QueryCondition[] conditions, (string,bool)[] orderByColumns) where TEntity : class, new();
    }
}