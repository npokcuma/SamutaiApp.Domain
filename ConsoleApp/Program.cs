using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
            //MultiDataBaseOperations();
            //InsertBattle();
            //QueryAndUpdateBattleDisconected();
            //InsertNewSamuraiWithQuote();
            //InsertNewSamuraiWithManyQuotes();
            AddQuoteToExistingSamuraiWhileTracked();
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote(){
                Text = "I bet you're happy that I've saved you"});
            context.SaveChanges();
        }

        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai()
            {
                Name = "Kyuzo",
                Quotes = new List<Quote>()
                {
                    new Quote()
                    {
                        Text = "Watch out for my sharp sword"
                    },
                    new Quote()
                    {
                        Text = "I told you to watch out for the sharp sword! Oh well!"
                    }
                }
            };
            context.Samurais.Add(samurai);
            context.SaveChanges();  
        }

        private static void InsertNewSamuraiWithQuote()
        {
            var samurai = new Samurai(){
                Name = "Kambei Shinada",
                Quotes = new List<Quote>()
                {
                    new Quote()
                    {
                        Text = "I've come to save you"
                    }
                }
                };
            context.Samurais.Add(samurai);
            context.SaveChanges();
        }

        private static void QueryAndUpdateBattleDisconected()
        {
            var battle = context.Battles.AsTracking().FirstOrDefault();
            battle.EndDate = new DateTime(1560, 06, 30);
            using (var newContextInstance = new SamuraiAppDataContext())
            {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }
        }

        private static void InsertBattle()
        {
            context.Battles.Add(new Battle()
            {
                Name = "Battle of Okehazama",
                StartDate = new DateTime(1560, 05, 01),
                EndDate = new DateTime(1560, 06, 15)
            });
            context.SaveChanges();
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
