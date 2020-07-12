using System;
using System.Collections.Generic;
using MinorLinq.Lib.Drivers.Npgsql;

namespace MinorLinq.Lib
{
    public abstract class DataContext : IDataContext, IDisposable
    {
        protected IDbDriver dbDriver;
        
        public bool Disposed { get; set; } = false;

        public DataContext()
        {
            dbDriver = new NpgsqlDriver();
            OnInit();
        }

        public void Dispose() => Dispose(true);

        private void OnInit()
        {
            OnEntityRegister();
            dbDriver.OpenConnection("Host=192.168.2.204;Username=postgres;Password=138b1488Smdfij8w!;Database=MinorLinq_t01");
        }

        protected virtual void OnEntityRegister()
        {
            foreach (var property in this.GetType().GetProperties())
            {
                if (typeof(IDbTable).IsAssignableFrom(property.PropertyType))
                {
                    IDbTable entity = (IDbTable)property.GetValue(this);
                    entity.SetAssignedContext(this);
                }
            }
        }

        protected virtual void Dispose(bool disposing) 
        {
            if (Disposed) return;
            if (disposing) 
            {
                //dbDriver.CloseConnection();
                Disposed = true;
            }
        }

        public List<TEntity> ExecuteQuery<TEntity>(string tableName, string[] selects, QueryCondition[] conditions)
        {
            if (Disposed) throw new ObjectDisposedException("Context is already disposed and cannot accept queries!");
            dbDriver.ExecuteQuery(tableName, selects, conditions);

            return null;
        }
    }
}