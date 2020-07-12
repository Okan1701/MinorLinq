using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace MinorLinq.Lib.Interfaces
{
    public interface IDataDeserializer
    {
        List<T> Deserialize<T>(DbDataReader reader) where T : class, new();
        Task<List<T>> DeserializeAsync<T>(DbDataReader reader) where T : class, new();
    }
}