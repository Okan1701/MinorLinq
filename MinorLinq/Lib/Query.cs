using System;
using System.Collections.Generic;

namespace MinorLinq.Lib
{
    public class Query<TEntity> where TEntity : class, new()
    {
        private string tableName;
        private string[] selects;

        public Query(string tableName, string[] selects)
        {
            this.tableName = tableName;
            this.selects = selects;
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
            return new Query<TEntity>(entityName, properties.ToArray());
        }
    }
}