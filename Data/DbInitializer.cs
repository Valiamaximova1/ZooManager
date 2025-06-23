using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class DbInitializer
    {
        public static void Initialize()
        {
            var factory = new ZooDbContextFactory();
            using var context = factory.CreateDbContext(Array.Empty<string>());

            context.Database.EnsureCreated();
            //context.Database.Migrate();
            Seed(context);
        }

        private static void Seed(ZooDbContext context)
        {

            if (!context.Users.Any())
            {
                context.Users.AddRange(new List<User>
        {
            new User { Email = "admin@zoo.bg", PasswordHash = "123", FirstName = "Admin", LastName = "Zookeeper" },
            new User { Email = "val", PasswordHash = "123", FirstName = "Val", LastName = "Max" }
        });
            }


            if (!context.Events.Any())
            {
                context.Events.AddRange(new List<Event>
        {
            new Event { Title = "Обиколка с екскурзовод", Type = EventType.Турове, Date = DateTime.Today.AddDays(1), Description = "Екскурзия с професионален водач из цялата зоологическа градина." },
            new Event { Title = "Демонстрация по хранене", Type = EventType.Готварски, Date = DateTime.Today.AddDays(2), Description = "Гледайте как гледачите хранят лъвовете и тигрите!" },
            new Event { Title = "Шоу с птици", Type = EventType.Специални, Date = DateTime.Today.AddDays(3), Description = "Шоу с папагали и орли – демонстрация на умения и дресура." },
            new Event { Title = "Нощно сафари", Type = EventType.Турове, Date = DateTime.Today.AddDays(2), Description = "Вълнуваща разходка из зоопарка след залез с екскурзовод." },
            new Event { Title = "Ферма за деца", Type = EventType.Образователни, Date = DateTime.Today.AddDays(3), Description = "Децата могат да галят и хранят домашни животни под наблюдение." },
            new Event { Title = "Хранене на хищници", Type = EventType.Готварски, Date = DateTime.Today.AddDays(4), Description = "Гледайте как се хранят лъвове, тигри и мечки – вълнуващо преживяване." },
            new Event { Title = "Зоолог-говорител", Type = EventType.Образователни, Date = DateTime.Today.AddDays(5), Description = "Нашият зоолог ще говори за поведението на най-интересните видове." },
            new Event { Title = "Мини зоопарк за малчугани", Type = EventType.Специални, Date = DateTime.Today.AddDays(6), Description = "Безопасна зона с зайци, патета и морски свинчета за най-малките." },
            new Event { Title = "Полет на хищни птици", Type = EventType.Специални, Date = DateTime.Today.AddDays(7), Description = "Впечатляващо въздушно шоу с орли, соколи и бухали." },
            new Event { Title = "Тропическа нощ", Type = EventType.Турове, Date = DateTime.Today.AddDays(8), Description = "Разходка из тропическия павилион с екзотични обяснения и музика." },
            new Event { Title = "Готвим за животните", Type = EventType.Готварски, Date = DateTime.Today.AddDays(9), Description = "Демонстрация как се подготвя балансирана храна за различните животни." },
            new Event { Title = "Ден на доброволеца", Type = EventType.Образователни, Date = DateTime.Today.AddDays(10), Description = "Всеки желаещ може да помогне в ежедневните грижи за животните." }
        });
                context.SaveChanges();
            }


            if (!context.Animals.Any())
            {
                context.Animals.AddRange(new List<Animal>
        {
            new Animal { Name = "Лъв", Description = "Царят на джунглата", Category = AnimalCategory.Бозайник, ImagePath = "Images\\lion.jpg", SoundPath = "Sounds\\lion.mp3" },
            new Animal { Name = "Тигър", Description = "такова е", Category = AnimalCategory.Бозайник, ImagePath = "Images\\tiger.jpg", SoundPath = "Sounds\\tiger.mp3" },
            new Animal { Name = "Папагал", Description = "Пъстър и говорещ", Category = AnimalCategory.Птица, ImagePath = "Images\\parrot.jpg", SoundPath = "Sounds\\parrot.mp3" },
            new Animal { Name = "Цаца", Description = "Описание за цацата", Category = AnimalCategory.Риба, ImagePath = "Images\\caca.jpg", SoundPath = "Sounds\\caca.mp3" },
            new Animal { Name = "Комар", Description = "Мразя ги", Category = AnimalCategory.Насекомо, ImagePath = "Images\\komar.jpg", SoundPath = "Sounds\\komar.mp3" },
            new Animal { Name = "Гущер", Description = "Описание за гущера", Category = AnimalCategory.Влечуго, ImagePath = "Images\\gushter.jpg", SoundPath = "Sounds\\gushter.mp3" }
        });
                context.SaveChanges();
            }


            var events = context.Events.Include(ev => ev.Animals).ToList();
            var animals = context.Animals.ToList();

            var eventByTitle = events.ToDictionary(ev => ev.Title);
            //var animalByName = animals.ToDictionary(animal => animal.Name);
            var animalByName = animals
                .GroupBy(a => a.Name)
                .Select(g => g.First())
                .ToDictionary(a => a.Name);

            void AddAnimalToEvent(string eventTitle, string animalName)
            {
                if (eventByTitle.TryGetValue(eventTitle, out var ev) &&
                    animalByName.TryGetValue(animalName, out var animal) &&
                    !ev.Animals.Any(an => an.Id == animal.Id))
                {
                    ev.Animals.Add(animal);
                }
            }


            AddAnimalToEvent("Обиколка с екскурзовод", "Лъв");
            AddAnimalToEvent("Обиколка с екскурзовод", "Папагал");
            AddAnimalToEvent("Демонстрация по хранене", "Лъв");
            AddAnimalToEvent("Демонстрация по хранене", "Тигър");
            AddAnimalToEvent("Шоу с птици", "Папагал");
            AddAnimalToEvent("Шоу с птици", "Комар");
            AddAnimalToEvent("Нощно сафари", "Лъв");
            AddAnimalToEvent("Нощно сафари", "Гущер");
            AddAnimalToEvent("Ферма за деца", "Папагал");
            AddAnimalToEvent("Ферма за деца", "Цаца");
            AddAnimalToEvent("Хранене на хищници", "Лъв");
            AddAnimalToEvent("Хранене на хищници", "Тигър");
            AddAnimalToEvent("Зоолог-говорител", "Лъв");
            AddAnimalToEvent("Зоолог-говорител", "Комар");
            AddAnimalToEvent("Мини зоопарк за малчугани", "Папагал");
            AddAnimalToEvent("Мини зоопарк за малчугани", "Цаца");
            AddAnimalToEvent("Полет на хищни птици", "Папагал");
            AddAnimalToEvent("Тропическа нощ", "Гущер");
            AddAnimalToEvent("Тропическа нощ", "Комар");
            AddAnimalToEvent("Готвим за животните", "Тигър");
            AddAnimalToEvent("Готвим за животните", "Цаца");
            AddAnimalToEvent("Ден на доброволеца", "Лъв");
            AddAnimalToEvent("Ден на доброволеца", "Гущер");

            context.SaveChanges();

            if (!context.TicketTemplates.Any())
            {
                var event1 = context.Events.FirstOrDefault(e => e.Title == "Обиколка с екскурзовод");
                var event2 = context.Events.FirstOrDefault(e => e.Title == "Шоу с птици");
                var event3 = context.Events.FirstOrDefault(e => e.Title == "Нощно сафари");
                var event4 = context.Events.FirstOrDefault(e => e.Title == "Ферма за деца");
                var event5 = context.Events.FirstOrDefault(e => e.Title == "Тропическа нощ");
                var event6 = context.Events.FirstOrDefault(e => e.Title == "Ден на доброволеца");

                if (event1 != null && event2 != null && event3 != null && event4 != null && event5 != null && event6 != null)
                {
                    context.TicketTemplates.AddRange(new List<TicketTemplate>
                {
                    new TicketTemplate
                    {
                        Title = "Обикновен билет",
                        Description = "Стандартен билет за обиколка.",
                        Type = TicketType.Редовен,
                        AvailableQuantity = 50,
                        Price = 50,
                        EventId = event1.Id
                    },
                    new TicketTemplate
                    {
                        Title = "VIP билет",
                        Description = "Включва място на първи ред и подарък.",
                        Type = TicketType.Ученически,
                        AvailableQuantity = 20,
                        Price = 100,
                        EventId = event2.Id
                    },
                    new TicketTemplate
                    {
                        Title = "Нощен билет",
                        Description = "Достъп до специалното нощно сафари.",
                        Type = TicketType.Редовен,
                        AvailableQuantity = 25,
                        Price = 70,
                        EventId = event3.Id
                    },
                    new TicketTemplate
                    {
                        Title = "Детски билет",
                        Description = "Само за деца под 12 години.",
                        Type = TicketType.Ученически,
                        AvailableQuantity = 50,
                        Price = 20,
                        EventId = event4.Id
                    },
                    new TicketTemplate
                    {
                        Title = "Тропическа разходка",
                        Description = "Екзотична обиколка с музика и светлини.",
                        Type = TicketType.Редовен,
                        AvailableQuantity = 15,
                        Price = 80,
                        EventId = event5.Id
                    },
                    new TicketTemplate
                    {
                        Title = "Билет за доброволци",
                        Description = "Безплатен достъп за доброволци.",
                        Type = TicketType.Редовен,
                        AvailableQuantity = 100,
                        Price = 0,
                        EventId = event6.Id
                    }
                });

                    context.SaveChanges();
                }

            }

            if (!context.TicketPurchases.Any())
            {
                var user = context.Users.FirstOrDefault(user => user.Email == "val");
                var ticketTemplate = context.TicketTemplates.FirstOrDefault(t => t.Title == "Обикновен билет");

                if (user != null && ticketTemplate != null)
                {
                    context.TicketPurchases.Add(new TicketPurchase
                    {
                        UserId = user.Id,
                        TicketTemplateId = ticketTemplate.Id,
                        Quantity = 2,
                        Price = ticketTemplate.Price,
                        PurchasedAt = DateTime.Now.AddDays(-1)
                    });

                    ticketTemplate.AvailableQuantity -= 2;
                    context.SaveChanges();
                }
            }
        }



    }


}
