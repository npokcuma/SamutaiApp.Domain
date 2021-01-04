using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace ConsoleApp
{
    class Program
    {
        private static SamuraiAppDataContext context = new SamuraiAppDataContext();
        static void Main(string[] args)
        {
            //context.Database.EnsureCreated();
            //GetSamurais("Before Add:");
            //AddSamurai();
            //GetSamurais("After Add:");
            MultiDataBaseOperations();
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void QueryFilters()
        {
            //var samurais = context.Samurais.Where(s => s.Name == "Samson").ToList();
            //foreach (var samurai in samurais) {
            //    Console.WriteLine(samurai.Name);
            //}
            string name = "Samson";
            var last = context.Samurais.OrderBy(s => s.Id).LastOrDefault(s => s.Name == name);
        }

        private static void MultiDataBaseOperations()
        {
            var samurais = context.Samurais.FirstOrDefault();
            samurais.Name += "San";
            context.Samurais.Add(new Samurai()
            {
                Name = "Kikuchio"
            });
            context.SaveChangesAsync();
        }
        private static void AddSamurai()
        {
            var samurai = new Samurai
            {
                Name = "Samson"
            };
            context.Samurais.Add(samurai);
            context.SaveChanges();
        }

        private static void GetSamurais(string text)
        {
            var samurais = context.Samurais.ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }
    }
}
