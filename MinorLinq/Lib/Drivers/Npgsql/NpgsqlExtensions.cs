namespace MinorLinq.Lib.Drivers.Npgsql
{
    public static class NpgsqlExtensions
    {
        public static DataContextBuilder UseNpgsql(this DataContextBuilder options, string connectionString)
        {
            var driver = new NpgsqlDriver() { ConnectionString = connectionString};
            options.DbDriver = driver;
            return options;
        }
    }
}