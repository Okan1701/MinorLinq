using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using MinorLinq.Lib.Interfaces;

namespace MinorLinq.Lib
{
    public class DataDeserializer : IDataDeserializer
    {
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
                    isValidRow = true;
                }
            }

            if (isValidRow) return entity;
            throw new MinorLinqInvalidReaderRowException("The current row in the DbDataReader contains no valid column!");
        }

        private void SetEntityValue<T>(string propName, object value, T entity) where T : class, new()
        {
            foreach (var prop in entity.GetType().GetProperties())
            {
                if (prop.Name == propName) prop.SetValue(entity, value);
            }
        }
    }
}