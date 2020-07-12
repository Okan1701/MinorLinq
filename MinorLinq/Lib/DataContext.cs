using System;
using System.Collections.Generic;
using System.Diagnostics;
using MinorLinq.Lib.Drivers.Npgsql;
using MinorLinq.Lib.Interfaces;

namespace MinorLinq.Lib
{
    public abstract class DataContext : IDataContext, IDisposable
    {
        protected IDbDriver dbDriver;
        protected IDataDeserializer deserializer;
        protected bool logQuery;
        
        public bool Disposed { get; set; }
        public bool IsConfigured { get; set; }

        public DataContext()
        {
            IsConfigured = false;
            OnInit(new DataContextBuilder());
        }

        public DataContext(DataContextBuilder options)
        {
            IsConfigured = true;
            OnInit(options);
        }

        public DataContext(Func<DataContextBuilder, DataContextBuilder> options)
        {
            var builder = options(new DataContextBuilder());
            IsConfigured = true;
            OnInit(builder);
        }

        public void Dispose() => Dispose(true);

        private void OnInit(DataContextBuilder options)
        {
            OnConfigure(options);
            
            dbDriver = options.DbDriver;
            deserializer = options.Deserializer;
            logQuery = options.LogQuery;
            IsConfigured = true;
            
            OnEntityRegister();
            dbDriver.OpenConnection();
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

        protected virtual void OnConfigure(DataContextBuilder builder)
        {
            return;
        }

        protected virtual void Dispose(bool disposing) 
        {
            if (Disposed) return;
            if (disposing) 
            {
                dbDriver.CloseConnection();
                GC.Collect();
                Disposed = true;
            }
        }

        public List<TEntity> ExecuteQuery<TEntity>(string tableName, string[] selects, QueryCondition[] conditions) where TEntity : class, new()
        {
            if (Disposed) throw new ObjectDisposedException("Context is already disposed and cannot accept queries!");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var queryRes = dbDriver.ExecuteQuery(tableName, selects, conditions);
            stopWatch.Stop();
            if (logQuery) { Console.WriteLine($"Execution of query took {stopWatch.ElapsedMilliseconds} ms | SQL: {queryRes.Item2}"); }
            return deserializer.Deserialize<TEntity>(queryRes.Item1);
        }
    }
}