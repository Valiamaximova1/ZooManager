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
            new Animal { Name = "Лъв", Description = "Царят на джунглата", Category = AnimalCategory.Бозайник, ImagePath = "Images\\lion.jpg", SoundPath = "Sounds/lion.mp3" },
            new Animal { Name = "Тигър", Description = "такова е ", Category = AnimalCategory.Бозайник, ImagePath = "Images\\tiger.jpg", SoundPath = "Sounds/tiger.mp3" },
            new Animal { Name = "Папагал", Description = "Пъстър и говорещ", Category = AnimalCategory.Птица, ImagePath = "Images\\parrot.jpg", SoundPath = "Sounds\\parrot.mp3" },
            new Animal { Name = "Цаца", Description = "Описание за цацата", Category = AnimalCategory.Риба, ImagePath = "Images\\caca.jpg", SoundPath = "Sounds\\caca.mp3" },
            new Animal { Name = "Комар", Description = "Мразя ги", Category = AnimalCategory.Насекомо, ImagePath = "Images\\komar.jpg", SoundPath = "Sounds\\komar.mp3" },
            new Animal { Name = "Гущер", Description = "Описание за гущера", Category = AnimalCategory.Влечуго, ImagePath = "Images\\gushter.jpg", SoundPath = "Sounds\\gushter.mp3" }
        });
                context.SaveChanges();
            }
            context.SaveChanges();

            // 👉 Помощен метод
            void AddAnimalToEvent(Event ev, Animal animal)
            {
                if (ev != null && animal != null && !ev.Animals.Any(a => a.Id == animal.Id))
                {
                    ev.Animals.Add(animal);
                }
            }
            if (!context.Animals.Any())
            {
                // 👉 Извличане на събития
                var tour = context.Events.FirstOrDefault(e => e.Title == "Обиколка с екскурзовод");
                var feeding = context.Events.FirstOrDefault(e => e.Title == "Демонстрация по хранене");
                var birds = context.Events.FirstOrDefault(e => e.Title == "Шоу с птици");
                var nightSafari = context.Events.FirstOrDefault(e => e.Title == "Нощно сафари");
                var kidsFarm = context.Events.FirstOrDefault(e => e.Title == "Ферма за деца");
                var predatorFeeding = context.Events.FirstOrDefault(e => e.Title == "Хранене на хищници");
                var zoologTalk = context.Events.FirstOrDefault(e => e.Title == "Зоолог-говорител");
                var miniZoo = context.Events.FirstOrDefault(e => e.Title == "Мини зоопарк за малчугани");
                var raptorFlight = context.Events.FirstOrDefault(e => e.Title == "Полет на хищни птици");
                var tropicalNight = context.Events.FirstOrDefault(e => e.Title == "Тропическа нощ");
                var cookingForAnimals = context.Events.FirstOrDefault(e => e.Title == "Готвим за животните");
                var volunteerDay = context.Events.FirstOrDefault(e => e.Title == "Ден на доброволеца");

                // 👉 Извличане на животни
                var lion = context.Animals.FirstOrDefault(a => a.Name == "Лъв");
                var tiger = context.Animals.FirstOrDefault(a => a.Name == "Тигър");
                var parrot = context.Animals.FirstOrDefault(a => a.Name == "Папагал");
                var mosquito = context.Animals.FirstOrDefault(a => a.Name == "Комар");
                var gushter = context.Animals.FirstOrDefault(a => a.Name == "Гущер");
                var caca = context.Animals.FirstOrDefault(a => a.Name == "Цаца");

                // 👉 Връзки
                AddAnimalToEvent(tour, lion);
                AddAnimalToEvent(tour, parrot);

                AddAnimalToEvent(feeding, lion);
                AddAnimalToEvent(feeding, tiger);

                AddAnimalToEvent(birds, parrot);
                AddAnimalToEvent(birds, mosquito);

                AddAnimalToEvent(nightSafari, lion);
                AddAnimalToEvent(nightSafari, gushter);

                AddAnimalToEvent(kidsFarm, parrot);
                AddAnimalToEvent(kidsFarm, caca);

                AddAnimalToEvent(predatorFeeding, lion);
                AddAnimalToEvent(predatorFeeding, tiger);

                AddAnimalToEvent(zoologTalk, lion);
                AddAnimalToEvent(zoologTalk, mosquito);

                AddAnimalToEvent(miniZoo, parrot);
                AddAnimalToEvent(miniZoo, caca);

                AddAnimalToEvent(raptorFlight, parrot);

                AddAnimalToEvent(tropicalNight, gushter);
                AddAnimalToEvent(tropicalNight, mosquito);

                AddAnimalToEvent(cookingForAnimals, tiger);
                AddAnimalToEvent(cookingForAnimals, caca);

                AddAnimalToEvent(volunteerDay, lion);
                AddAnimalToEvent(volunteerDay, gushter);

                // 👉 Запазване
                context.SaveChanges();

            }

            if (!context.TicketTemplates.Any())
            {
                var event1 = context.Events.FirstOrDefault(e => e.Title == "Обиколка с екскурзовод");
                var event2 = context.Events.FirstOrDefault(e => e.Title == "Шоу с птици");

                if (event1 != null && event2 != null)
                {
                    context.TicketTemplates.AddRange(new List<TicketTemplate>
        {
            new TicketTemplate
            {
                Title = "Обикновен билет",
                Description = "Стандартен билет за обиколка.",
                Type = TicketType.Редовен,
                AvailableQuantity = 50,
                EventId = event1.Id
            },
            new TicketTemplate
            {
                Title = "VIP билет",
                Description = "Включва място на първи ред и подарък.",
                Type = TicketType.Ученически,
                AvailableQuantity = 20,
                EventId = event2.Id
            }
        });
                    context.SaveChanges();
                }
            }

            // Добавяне на покупки на билети
            if (!context.TicketPurchases.Any())
            {
                var user1 = context.Users.FirstOrDefault(u => u.Email == "val");
                var ticketTemplate = context.TicketTemplates.FirstOrDefault(t => t.Title == "Обикновен билет");

                if (user1 != null && ticketTemplate != null)
                {
                    context.TicketPurchases.Add(new TicketPurchase
                    {
                        UserId = user1.Id,
                        TicketTemplateId = ticketTemplate.Id,
                        Quantity = 2,
                        PurchasedAt = DateTime.Now.AddDays(-1)
                    });

                    ticketTemplate.AvailableQuantity -= 2;
                    context.SaveChanges();
                }
            }
            context.SaveChanges();
        }


    }


}
