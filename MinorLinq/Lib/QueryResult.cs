using System.Data;

namespace MinorLinq.Lib
{
    public class QueryResult
    {
        public IDataReader RowData { get; set; }
        public string ExecutedSql { get; set; }
        
    }
}