using System;
using MinorLinq.Lib;
using MinorLinq.Lib.Drivers.Npgsql;
using MinorLinq.Lib.Drivers.Sqlite;

namespace MinorLinq.Models
{
    public class ConsoleDataContext : DataContext
    {
        public DbTable<Category> Categories { get; set; }
        public DbTable<Post> Posts { get; set; }
        
        public ConsoleDataContext() : base() { }
        public ConsoleDataContext(Func<DataContextBuilder,DataContextBuilder> options) : base(options) { }
        
        protected override void OnConfigure(DataContextBuilder builder)
        {
            builder.UseNpgsql("Host=192.168.2.204;Username=postgres;Password=postgres;Database=MinorLinq_t01");
            //builder.UseSqlite("Data Source=sample.db");
        }
    }
}