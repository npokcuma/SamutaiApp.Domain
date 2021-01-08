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
            //QueryAndUpdateBattleDisconnected();
            //InsertNewSamuraiWithQuote();
            //InsertNewSamuraiWithManyQuotes();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiNotTracked(2);
            //AddQuoteToExistingSamuraiNotTrackedEasy(2);
            //EagerLoadingSamuraiWithQuotes();
            //ProjectSamuraisWithQuotes();
            //ExplicitLoadQuotes();
            //FilteringWithRelatedData();
            //ModifyingRelatedDataWhenNotTracked();
            //JoinBattleAndSamurai();
            //EnlistSamuraiIntoBattle();
            //RemoveJoinBetweenSamuraiAndBattleSimple();
            //GetSamuraiWithBattles();
            //AddNewHorseToSamuraiUsingId();
            //AddNewHorseToSamuraiObject();
            //AddNewHorseToDisconnectedSamuraiObject();
            //ReplaceHorse();
            //GetHorseWithSamurai();
            //GetSamuraiWithClan();
            GetClanWithSamurais();
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void GetClanWithSamurais()
        {
            //var clan = context.Clans.Include(c => c.....);
            var clan = context.Clans.Find(3);
            var samuraiForClan = context.Samurais.Where(s => s.Clan.Id == 3).ToList();
        }

        private static void GetSamuraiWithClan()
        {
            var samurai = context.Samurais.Include(s => s.Clan).FirstOrDefault();
        }

        private static void GetHorseWithSamurai()
        {
            var horseWithoutSamurai = context.Set<Horse>().Find(1);

            var horseWithSamurai = context.Samurais.Include(s => s.Horse)
                .FirstOrDefault(s => s.Id == 2);

            var horseWithSamurais = context.Samurais.Where(s => s.Horse != null)
                .Select(s => new
                {
                    Horse = s.Horse,
                    Samurai = s
                }).ToList();
        }

        private static void ReplaceHorse()
        {
            var samurai = context.Samurais.Include(s => s.Horse).FirstOrDefault(s => s.Id == 9);
            samurai.Horse = new Horse
            {
                Name = "Trigger"
            };
            context.SaveChanges();
        }


        private static void AddNewHorseToDisconnectedSamuraiObject()
        {
            var samurai = context.Samurais.AsNoTracking().FirstOrDefault(s=>s.Id == 9);
            samurai.Horse = new Horse
            {
                Name = "Mr. Ed"
            };
            using(var newContext = new SamuraiAppDataContext())
            {
                newContext.Attach(samurai);
                newContext.SaveChanges();
            }
        }


        private static void AddNewHorseToSamuraiObject()
        {
            var samurai = context.Samurais.Find(10);
            samurai.Horse = new Horse
            {
                Name = "Black Beauty"
            };
            context.SaveChanges();
        }

        private static void AddNewHorseToSamuraiUsingId()
        {
            var horse = new Horse(){
                Name = "Scout",
                SamuraiId = 2
            };
            context.Add(horse);
            context.SaveChanges();
        }


        private static void GetSamuraiWithBattles()
        {
            //var samuraiWithBattle = context.Samurais.Include(s => s.SamuraiBattles)
            //    .ThenInclude(sb => sb.Battle)
            //    .FirstOrDefault(samurai => samurai.Id == 2);

            var samuraiWithBattleCleaner = context.Samurais.Where(samurai => samurai.Id == 2)
                .Select(s => new
                {
                    Samurai = s,
                    Battles = s.SamuraiBattles.Select(sb => sb.Battle)
                })
                .FirstOrDefault();

        }

        private static void RemoveJoinBetweenSamuraiAndBattleSimple()
        {
            var join = new SamuraiBattle()
            {
                BattleId = 1,
                SamuraiId = 2
            };
            context.Remove(join);
            context.SaveChanges();
        }

        private static void EnlistSamuraiIntoBattle()
        {
            var battle = context.Battles.Find(1);
            battle.SamuraiBattles.Add(new SamuraiBattle(){
                SamuraiId = 10});
            context.SaveChanges();
        }

        private static void JoinBattleAndSamurai()
        {
            //Samurai and Battle are exist and we have their IDs
            var sbJoin = new SamuraiBattle()
            {
                SamuraiId = 1,
                BattleId = 1,
            };
            context.Add(sbJoin);
            context.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = context.Samurais
                .Include(s => s.Quotes)
                .FirstOrDefault(s => s.Id == 2);
            var quote = samurai.Quotes[2];
            quote.Text = "Did you hear that again?";
            using (var newContext = new SamuraiAppDataContext())
            {
                //newContext.Quotes.Update(quote);
                newContext.Entry(quote).State = EntityState.Modified;
                newContext.SaveChanges();
            }
        }

         

        //You can only load from a single object
        private static void ExplicitLoadQuotes()
        {
            var samurai = context.Samurais.FirstOrDefault(s => s.Name.Contains("Samson"));
            //context.Entry(samurai).Collection(s => s.Quotes).Load();
            //context.Entry(samurai).Reference(s => s.Horse).Load();
            var happyQuotes = context.Entry(samurai)
                .Collection(b => b.Quotes)
                .Query()
                .Where(q => q.Text.Contains("Happy"))
                .ToList();
        }

        private static void ProjectSamuraisWithQuotes()
        {
            //var somePropertiesWithQuotes = context.Samurais.Select(s => new {s.Id, s.Name, s.Quotes.Count}).ToList();
            //var somePropertiesWithQuotes = context.Samurais
            //    .Select(s => new {s.Id, s.Name, HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))})
            //    .ToList();
            var somePropertiesWithQuotes = context.Samurais
                .Select(s => new {Samurai = s, HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))})
                .ToList();
            var firstSamurai = somePropertiesWithQuotes[0].Samurai.Name += " The Happiest";
        }

        private static void EagerLoadingSamuraiWithQuotes()
        {
            //var samuraiWithQuotes = context.Samurais.Where(s=>s.Name == "Samson").Include(s => s.Quotes).ToList();
            //var samuraiWithQuotes2 = context.Samurais.Include(s => s.Quotes).ThenInclude(q => q.Translation);
            //var samuraiWithQuotes3 = context.Samurais.Include(s => s.Quotes.Translation);
            var samuraiWithQuotes3 = context.Samurais.Include(s => s.Quotes).Include(q => q.Clan);
        }

        private static void AddQuoteToExistingSamuraiNotTrackedEasy(int samuraiId)
        {
            var quote = new Quote()
            {
                Text = "Now that I saved you, will you feed me dinner again?",
                SamuraiId =  samuraiId
            };
            using (var newContext = new SamuraiAppDataContext())
            {
                newContext.Quotes.Add(quote);
                newContext.SaveChanges();
            }
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote(){
                Text = "I bet you're happy that I've saved you"});
            context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote()
            {
                Text = "Now that I saved you, will you feed me dinner?"
            });
            using (var newContext = new SamuraiAppDataContext())
            {
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
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

        private static void QueryAndUpdateBattleDisconnected()
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
