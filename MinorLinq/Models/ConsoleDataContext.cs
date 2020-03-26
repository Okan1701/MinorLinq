using MinorLinq.Lib;

namespace MinorLinq.Models
{
    public class ConsoleDataContext : DataContext
    {
        public DbTable<Posts> Posts { get; set; } = new DbTable<Posts>();
        public DbTable<Categories> Categories { get; set; } = new DbTable<Categories>();

    }
}