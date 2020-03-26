using System;
using MinorLinq.Lib;
using MinorLinq.Models;

namespace MinorLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleDataContext context = new ConsoleDataContext();
            Query query = context.Posts.Select(p => new {Id = p.Id, CreatedOn = p.CreatedOn});
            
            Console.WriteLine("Done");
        }
    }
}