using System;
using System.Collections.Generic;
using MinorLinq.Lib;
using MinorLinq.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MinorLinq.Lib.Drivers.Npgsql;

namespace MinorLinq
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var context = new ConsoleDataContext(options => options.EnableLogging(true)))
            {


                var test = context.Categories
                    .Select(x => new {x.Id, x.DefaultLanguageCode, x.CreatedOn})
                    .Where(x => x.Id > 5);

                List<Category> categories = context.Categories
                    .Select(x => new {x.Id, x.DefaultLanguageCode, x.CreatedOn})
                    .Where(x => x.DefaultLanguageCode != "NL-nl")
                    .Where(x => x.Id >= 1)
                    .Where(x => x.IsHidden)
                    .OrderByDescending(x => x.Id)
                    .OrderBy(x => x.IsHidden)
                    .ToList();
                Console.WriteLine($"{categories.Count} results");
            }

            Console.WriteLine("Done");
        }


    }
}