using System;
using MinorLinq.Lib;
using MinorLinq.Models;
using System.Linq.Expressions;

namespace MinorLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleDataContext context = new ConsoleDataContext();
            Query<Posts> query = context.Posts.Select(p => new  { p.Id, p.CreatedOn });
            var test = "trololololo";
            var where = context.Posts.Where(p => p.DefaultLanguageCode == "ss");

            Console.WriteLine("Done");
        }


    }
}