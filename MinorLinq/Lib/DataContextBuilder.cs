using MinorLinq.Lib.Interfaces;

namespace MinorLinq.Lib
{
    public class DataContextBuilder
    {
        private bool logQuery;
        
        public IDbDriver DbDriver { get; set; }
        public IDataDeserializer Deserializer { get; set; } = new DataDeserializer();
        public ILogger Logger { get; set; } = new Logger();
        public bool Logging { get; set; }

        public DataContextBuilder EnableLogging(bool log)
        {
            Logging = log;
            return this;
        }
        
        
    }
}