using System.Collections.Generic;

namespace MinorLinq.Lib
{
    public interface IDataContext
    {
        List<TEntity> ExecuteQuery<TEntity>(string tableName, string[] selects, QueryCondition[] conditions) where TEntity : class, new();
    }
}