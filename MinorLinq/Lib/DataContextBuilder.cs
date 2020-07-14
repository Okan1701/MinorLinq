using MinorLinq.Lib.Interfaces;

namespace MinorLinq.Lib
{
    public class DataContextBuilder
    {
        private bool logQuery;
        
        /// <summary>
        /// The driver instance that will be used by the context
        /// </summary>
        public IDbDriver DbDriver { get; set; }
        public IDataDeserializer Deserializer { get; set; } = new DataDeserializer();
        /// <summary>
        /// The instance of the logger that will be used by the context
        /// </summary>
        public ILogger Logger { get; set; } = new Logger();
        public bool Logging { get; set; }
        
        /// <summary>
        /// Enable or disable the logging feature of the DataContext
        /// </summary>
        /// <param name="log">True to enable logging, false to disable</param>
        /// <returns>Returns the updated builder</returns>
        public DataContextBuilder EnableLogging(bool log)
        {
            Logging = log;
            return this;
        }
        
        
    }
}