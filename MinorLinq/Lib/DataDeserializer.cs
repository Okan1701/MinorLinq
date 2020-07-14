using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using MinorLinq.Lib.Interfaces;

namespace MinorLinq.Lib
{
    public class DataDeserializer : IDataDeserializer
    {
        /// <summary>
        /// Read the contents of the data reader and deserialize it into a List of entities with type TEntity
        /// </summary>
        /// <param name="reader">The DbDataReader that contains the SQL query results</param>
        /// <typeparam name="T">Represents the type of the entity</typeparam>
        /// <returns>List of entities that represent the results from the reader</returns>
        public List<T> Deserialize<T>(DbDataReader reader) where T : class, new()
        {
            var entityList = new List<T>();
            var entityPropNames = typeof(T).GetProperties().Select(x => x.Name).ToArray();

            while (reader.Read())
            {
                entityList.Add(ProccesReaderRow<T>(reader, entityPropNames));
            }
            
            return entityList;
        }
        /// <summary>
        /// Read the contents of the data reader and deserialize it asynchronously into a List of entities with type TEntity
        /// </summary>
        /// <param name="reader">The DbDataReader that contains the SQL query results</param>
        /// <typeparam name="T">Represents the type of the entity</typeparam>
        /// <returns>List of entities that represent the results from the reader</returns>
        public async Task<List<T>> DeserializeAsync<T>(DbDataReader reader) where T : class, new()
        {
            var entityList = new List<T>();
            var entityPropNames = typeof(T).GetProperties().Select(x => x.Name).ToArray();

            while (await reader.ReadAsync())
            {
                entityList.Add(ProccesReaderRow<T>(reader, entityPropNames));
            }
            
            return entityList;
        }
        
        /// <summary>
        /// Processes the current row of the reader by getting the results
        /// and assigning it to the correct property of the entity
        /// </summary>
        /// <param name="reader">The DbReader that needs to be processed</param>
        /// <param name="entityPropNames">All valid property names of the entity</param>
        /// <typeparam name="T">Type of the target Entity</typeparam>
        /// <returns>An instance of the entity that contains the reader row data</returns>
        /// <exception cref="MinorLinqInvalidReaderRowException">Thrown when not a single column name in the reader matched a entity property</exception>
        private T ProccesReaderRow<T>(DbDataReader reader, string[] entityPropNames) where T : class, new()
        {
            var isValidRow = false;
            var entity = new T();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i);
                object value = reader.GetValue(i);
                if (entityPropNames.Contains(columnName))
                {
                    SetEntityValue(columnName, value, entity);
                    // If we found a value in the row that we could assign to entity, then row is valid
                    isValidRow = true;
                }
            }

            if (isValidRow) return entity;
            throw new MinorLinqInvalidReaderRowException("The current row in the DbDataReader contains no valid column!");
        }

        /// <summary>
        /// Converts the value to the same type of the entity property and then assigns it to that property
        /// </summary>
        /// <param name="propName">The entity property name that we want to assign the value to</param>
        /// <param name="value">The value that we will convert and assign</param>
        /// <param name="entity">The entity that contains the property</param>
        /// <typeparam name="T">Type of the entity</typeparam>
        private void SetEntityValue<T>(string propName, object value, T entity) where T : class, new()
        {
            foreach (var prop in entity.GetType().GetProperties())
            {
                if (prop.Name == propName)
                {
                    var propType = prop.PropertyType;
                    object convertedValue = Convert.ChangeType(value, propType);
                    prop.SetValue(entity, convertedValue);
                }
            }
        }
    }
}