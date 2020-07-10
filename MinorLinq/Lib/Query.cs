using System;
using System.Collections.Generic;
using MinorLinq.Lib;

namespace MinorLinq.Lib
{
    public class Query<TEntity> where TEntity : class, new()
    {
        private string tableName;
        private string[] selects;
        private QueryWhereCondition[] where;

        public Query(string tableName, string[] selects, QueryWhereCondition[] where)
        {
            this.tableName = tableName;
            this.selects = selects;
            this.where = where;
        }
        
        public Query<TEntity> Select(Func<TEntity, object> selectFunc)
        {
            TEntity emptyEntity = new TEntity();
            object selectRes = selectFunc(emptyEntity);
            var properties = new List<string>();
            var entityName = emptyEntity.GetType().Name;
            foreach (var prop in selectRes.GetType().GetProperties())
            {
                properties.Add(prop.Name);
            }
            return new Query<TEntity>(entityName, properties.ToArray(), null);
        }
    }
}