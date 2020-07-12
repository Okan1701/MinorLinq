using System.Data;
using System.Data.Common;

namespace MinorLinq.Lib
{
    public class QueryResult
    {
        public DbDataReader RowData { get; set; }
        public string ExecutedSql { get; set; }
        
    }
}