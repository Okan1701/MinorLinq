using System.Data;
using System.Collections.Generic;

namespace MinorLinq.Lib.Interfaces
{
    public interface IDataDeserializer
    {
        List<T> Deserialize<T>(IDataReader reader) where T : class, new();
    }
}