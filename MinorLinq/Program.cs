using System;
using System.Collections.Generic;
using System.Linq;
using MinorLinq.Models;
using System.Threading.Tasks;

namespace MinorLinq
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Category[] allCategories;
            IEnumerable<Post> allPosts;
            Category[] hiddenCategories;
            List<Post> postNamesFromRandomCategory;
            
            var random = new Random();
            int randomId = 11;
            
            Console.WriteLine("HOGESCHOOL ROTTERDAM");
            Console.WriteLine("Minor Software Engineering - 2019/2020");
            Console.WriteLine("Student: Okan Emeni (0950438)");
            Console.WriteLine("Project: C# LINQ to SQL (project 1)" + Environment.NewLine);

            using (var context = new ConsoleDataContext(options => options.EnableLogging(true)))
            {    
                // Get all categories and posts
                // Async methods are also available
                allCategories = await context.Categories.ToArrayAsync();
                allPosts = context.Posts.AsEnumerable();

                hiddenCategories = await context.Categories
                    .Where(category => category.IsHidden == true)
                    .OrderBy(category => category.Id)
                    .ToArrayAsync();
                
                // Get the ID of a random category
                //randomId = allCategories[random.Next(0, allCategories.Length - 1)].Id;
                postNamesFromRandomCategory = await context.Posts
                    .Select(p => new { p.PostName })
                    .Where(p => p.CategoryId == randomId)
                    .OrderByDescending(p => p.Id)
                    .ToListAsync();

            }
            Console.WriteLine(Environment.NewLine);
            
            Console.WriteLine($"There are {allCategories.Length} categories");
            Console.WriteLine($"There are {allPosts.Count()} posts");
            Console.WriteLine($"{hiddenCategories.Length} categories out of the {allCategories.Length} are hidden");
            Console.WriteLine(Environment.NewLine);
            
            Console.WriteLine($"Post names that belong to a random category with ID={randomId}");
            foreach (var post in postNamesFromRandomCategory)
            {
                Console.WriteLine($"Name: {post.PostName}");
            }
            
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Program finished!");
        }


    }
}