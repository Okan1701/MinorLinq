using System;
using System.Collections.Generic;
using MinorLinq.Lib;
using MinorLinq.Models;
using System.Linq.Expressions;

namespace MinorLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new ConsoleDataContext())
            {
                List<Category> categories = context.Categories
                    .Select(x => new {x.Id, x.DefaultLanguageCode, x.CreatedOn})
                    .Where(x => 2 == x.Id)
                    .ToList();
            }

            Console.WriteLine("Done");
        }


    }
}