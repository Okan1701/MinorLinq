using System.Collections.Generic;
using System.Data;
using System.Linq;
using MinorLinq.Lib.Interfaces;

namespace MinorLinq.Lib
{
    public class DataDeserializer : IDataDeserializer
    {
        public List<T> Deserialize<T>(IDataReader reader) where T : class, new()
        {
            var entityList = new List<T>();
            var entityPropNames = typeof(T).GetProperties().Select(x => x.Name).ToArray();

            while (reader.Read())
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
                if (isValidRow) entityList.Add(entity);
            }
            
            return entityList;
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