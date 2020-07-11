using Npgsql;

namespace MinorLinq.Lib.Drivers.Npgsql 
{
    public class NpgsqlDriver : IDbDriver 
    {
        private NpgsqlConnection connection;

        public void OpenConnection(string connectionString) 
        {
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }

        public void CloseConnection() 
        {
            connection.Dispose();
        }
    }
}