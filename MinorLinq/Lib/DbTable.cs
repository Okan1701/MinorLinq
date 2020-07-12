using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace MinorLinq.Lib
{
    public class DbTable<TEntity> : Query<TEntity>,IDbTable where TEntity : class, new()
    {
        public DbTable() : base(typeof(TEntity).Name, typeof(TEntity).GetProperties().Select(x => x.Name).ToArray() , new QueryCondition[0], new (string, bool)[0]) { }

        public void SetAssignedContext(IDataContext context)
        {
            this.context = context;
        }
    }
}