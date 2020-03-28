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
            Query<Posts> query = context.Posts.Select(p => new  { p.Id });
            
            Console.WriteLine("Done");
        }
    }
}