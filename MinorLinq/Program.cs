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
            using (var context = new ConsoleDataContext()) 
            {
                var multi = context.Categories.Select(x => new { x.Id, x.DefaultLanguageCode })
                    .Where(x => x.Id == 2);

                multi.ToList();
            }

            Console.WriteLine("Done");
        }


    }
}