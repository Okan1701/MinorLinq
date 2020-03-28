using System;
using System.Collections.Generic;

namespace MinorLinq.Lib
{
    public class DbTable<TEntity> : IDbTable where TEntity : class, new()
    {
        private IDataContext assignedDataContext;

        public void SetAssignedContext(DataContext context)
        {
            assignedDataContext = context;
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

        public Query<TEntity> Where(Func<TEntity, object> whereFunc)
        {
            throw new NotImplementedException();
        }
    }
}