using MinorLinq.Lib;
using MinorLinq.Lib.Drivers.Npgsql;

namespace MinorLinq.Models
{
    public class ConsoleDataContext : DataContext
    {
        public DbTable<Post> Posts { get; set; } = new DbTable<Post>();
        public DbTable<Category> Categories { get; set; } = new DbTable<Category>();

        public ConsoleDataContext() : base()
        {
            //dbDriver = new NpgsqlDriver();
        }

    }
}