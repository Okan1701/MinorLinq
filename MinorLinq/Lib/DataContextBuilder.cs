using MinorLinq.Lib.Interfaces;

namespace MinorLinq.Lib
{
    public class DataContextBuilder
    {
        private bool logQuery;
        
        public IDbDriver DbDriver { get; set; }
        public IDataDeserializer Deserializer { get; set; } = new DataDeserializer();
        public bool LogQuery { get; set; }

        public DataContextBuilder SetLogQuery(bool log)
        {
            LogQuery = log;
            return this;
        }
        
        
    }
}