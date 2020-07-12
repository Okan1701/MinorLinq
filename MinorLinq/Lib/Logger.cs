using System;
using System.Collections.Generic;
using System.Globalization;
using MinorLinq.Lib.Interfaces;

namespace MinorLinq.Lib
{
    public class Logger : ILogger
    {
        public void Log(string msg)
        {
            Console.WriteLine(
                $"{DateTime.Now.ToString(CultureInfo.DefaultThreadCurrentCulture)} | " +
                $"INFO | " + msg);
        }

        public void LogQueryResult(long execTimeMs, IEnumerable<string> statements)
        {
            Log($"Execution of query took {execTimeMs} ms");
            foreach (var stmt in statements)
            {
                Console.WriteLine(
                    $"{DateTime.Now.ToString(CultureInfo.DefaultThreadCurrentCulture)} | " +
                    $"SQL | " + stmt);
            }
        }
    }
}