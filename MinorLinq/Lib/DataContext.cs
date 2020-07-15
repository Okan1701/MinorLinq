using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MinorLinq.Lib.Drivers.Npgsql;
using MinorLinq.Lib.Interfaces;

namespace MinorLinq.Lib
{
    public abstract class DataContext : IDataContext, IDisposable
    {
        protected IDbDriver dbDriver;
        protected IDataDeserializer deserializer;
        protected ILogger logger;
        protected bool isLogging;
        
        public bool Disposed { get; set; }
        /// <summary>
        /// Shows if context already recieved an existing DataContextBuilder or if it created a new one
        /// </summary>
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

        /// <summary>
        /// Initializes the context by applying the builder config, registering the entities and opening the database connection
        /// </summary>
        /// <param name="options">The DataContextBuilder which will be used to configure the context</param>
        private void OnInit(DataContextBuilder options)
        {
            // Run the builder through OnConfigure incase it is implemented
            OnConfigure(options);
            
            // Apply all the options from the builder
            dbDriver = options.DbDriver;
            deserializer = options.Deserializer;
            logger = options.Logger;
            isLogging = options.Logging;
            IsConfigured = true;
            
            OnEntityRegister();
            dbDriver.OpenConnection(logger);
            if (isLogging)
            {
                logger.Log($"{this.GetType().Name} has been initialized with driver: {dbDriver.DriverName} ({dbDriver.GetType().FullName})");
            }
        }
        
        /// <summary>
        /// Checks all declared DbTable<> properties and assigns them the current context instance
        /// </summary>
        private void OnEntityRegister()
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
        
        /// <summary>
        /// This method is optionally used to configure the context. It is meant to be overriden by subclasses that inherit this base class.
        /// </summary>
        /// <param name="builder">The builder instance which is used to configure the context</param>
        protected virtual void OnConfigure(DataContextBuilder builder)
        {
        }
        
        /// <summary>
        /// Dispose the context instance
        /// </summary>
        /// <param name="disposing">Bool to dispose or not</param>
        private void Dispose(bool disposing) 
        {
            if (Disposed) return;
            if (disposing) 
            {
                dbDriver.CloseConnection();
                GC.Collect();
                Disposed = true;
                if (isLogging)
                {
                    logger.Log($"{this.GetType().Name} has been disposed");
                }
            }
        }
        
        /// <summary>
        /// Executes a LINQ query and returns the deserialized results from the database in the form of a List
        /// </summary>
        /// <param name="tableName">The name of the database tables</param>
        /// <param name="selects">Array of selected column names</param>
        /// <param name="conditions">Array of select WHERE conditions</param>
        /// <param name="orderByColumns">Array of selected ORDER BY column names</param>
        /// <typeparam name="TEntity">Type of the target entity</typeparam>
        /// <returns>List of type T containing the results</returns>
        /// <exception cref="ObjectDisposedException">Cannot execute query if context is disposed</exception>
        public List<TEntity> ExecuteQuery<TEntity>(string tableName, string[] selects, QueryCondition[] conditions, (string, bool)[] orderByColumns) where TEntity : class, new()
        {
            if (Disposed) throw new ObjectDisposedException("Context is already disposed and cannot accept queries!");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var queryRes = dbDriver.ExecuteQuery(tableName, selects, conditions, orderByColumns);
            stopWatch.Stop();
            if (isLogging)
            {
                logger.LogQueryResult(stopWatch.ElapsedMilliseconds, queryRes.Item2);
            }

            var res = deserializer.Deserialize<TEntity>(queryRes.Item1);
            queryRes.Item1.Close();
            queryRes.Item1.Dispose();
            return res;
        }        
        
        /// <summary>
        /// Executes a LINQ query asynchronously and returns the deserialized results from the database in the form of a List
        /// </summary>
        /// <param name="tableName">The name of the database tables</param>
        /// <param name="selects">Array of selected column names</param>
        /// <param name="conditions">Array of select WHERE conditions</param>
        /// <param name="orderByColumns">Array of selected ORDER BY column names</param>
        /// <typeparam name="TEntity">Type of the target entity</typeparam>
        /// <returns>List of type T containing the results</returns>
        /// <exception cref="ObjectDisposedException">Cannot execute query if context is disposed</exception>
        public async Task<List<TEntity>> ExecuteQueryAsync<TEntity>(string tableName, string[] selects, QueryCondition[] conditions, (string, bool)[] orderByColumns) where TEntity : class, new()
        {
            if (Disposed) throw new ObjectDisposedException("Context is already disposed and cannot accept queries!");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var queryRes = await dbDriver.ExecuteQueryAsync(tableName, selects, conditions, orderByColumns);
            stopWatch.Stop();
            if (isLogging)
            {
                logger.LogQueryResult(stopWatch.ElapsedMilliseconds, queryRes.Item2);
            }

            var res = await deserializer.DeserializeAsync<TEntity>(queryRes.Item1);
            await queryRes.Item1.CloseAsync();
            await queryRes.Item1.DisposeAsync();
            return res;
        }
        
    }
}