using System.Collections.Generic;

namespace MinorLinq.Lib.Interfaces
{
    public interface ILogger
    {
        void Log(string msg);
        void LogQueryResult(long execTimeMs, IEnumerable<string> sql);
    }
}