namespace MinorLinq.Lib.Drivers.Sqlite
{
    public static class SqliteExtensions
    {
        public static DataContextBuilder UseSqlite(this DataContextBuilder options, string connectionString)
        {
            var driver = new SqliteDriver() { ConnectionString = connectionString};
            options.DbDriver = driver;
            return options;
        }
    }
}