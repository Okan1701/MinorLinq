using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinorLinq.Lib
{
    public interface IDataContext
    {
        List<TEntity> ExecuteQuery<TEntity>(string tableName, string[] selects, QueryCondition[] conditions, (string,bool)[] orderByColumns) where TEntity : class, new();
        Task<List<TEntity>> ExecuteQueryAsync<TEntity>(string tableName, string[] selects, QueryCondition[] conditions, (string,bool)[] orderByColumns) where TEntity : class, new();
    }
}