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
            Query<Post> query = context.Posts.Select(p => new  { p.Id, p.CreatedOn });
            var where = context.Posts.Where(p => p.DefaultLanguageCode == "ss");

            var multi = context.Posts.Select(x => new { x.AuthorId })
                .Select(x => new { x.CategoryId, x.EditedOn })
                .Where(x => x.CategoryId == 5);

            Console.WriteLine("Done");
        }


    }
}