using Data;
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
            new Event
            {
                Title = "Обиколка с екскурзовод",
                Type = EventType.Турове,
                Date = DateTime.Today.AddDays(1),
                Description = "Екскурзия с професионален водач из цялата зоологическа градина."
            },
            new Event
            {
                Title = "Демонстрация по хранене",
                Type = EventType.Готварски,
                Date = DateTime.Today.AddDays(2),
                Description = "Гледайте как гледачите хранят лъвовете и тигрите!"
            },
            new Event
            {
                Title = "Шоу с птици",
                Type = EventType.Специални,
                Date = DateTime.Today.AddDays(3),
                Description = "Шоу с папагали и орли – демонстрация на умения и дресура."
            }
        });
                context.SaveChanges();
            }


            if (!context.Animals.Any())
            {
                context.Animals.AddRange(new List<Animal>
            {
                new Animal
                {
                    Name = "Лъв",
                    Description = "Царят на джунглата",
                    Category = AnimalCategory.Бозайник,
                    ImagePath = "Images\\lion.jpg",
                    SoundPath = "Sounds/lion.mp3"
                },
                new Animal
                {
                    Name = "Тигър",
                    Description = "такова е ",
                    Category = AnimalCategory.Бозайник,
                    ImagePath = "Images\\tiger.jpg",
                    SoundPath = "Sounds/tiger.mp3"
                },
                new Animal
                {
                    Name = "Папагал",
                    Description = "Пъстър и говорещ",
                    Category = AnimalCategory.Птица,
                    ImagePath = "Images\\parrot.jpg",
                    SoundPath = "Sounds\\parrot.mp3"
                },
                new Animal
                {
                    Name = "Цаца",
                    Description = "Описание за цацата",
                    Category = AnimalCategory.Риба,
                    ImagePath = "Images\\caca.jpg",
                    SoundPath = "Sounds\\caca.mp3"
                },   new Animal
                {
                    Name = "Комар",
                    Description = "Мразя ги",
                    Category = AnimalCategory.Насекомо,
                    ImagePath = "Images\\komar.jpg",
                    SoundPath = "Sounds\\komar.mp3"
                },   new Animal
                {
                    Name = "Гущер",
                    Description = "Описание за гущера",
                    Category = AnimalCategory.Влечуго,
                    ImagePath = "Images\\gushter.jpg",
                    SoundPath = "Sounds\\gushter.mp3"
                },
            });
                context.SaveChanges();
            }
            if (!context.Animals.Any())
            {

                var tour = context.Events.FirstOrDefault(e => e.Title == "Обиколка с екскурзовод");
                var feeding = context.Events.FirstOrDefault(e => e.Title == "Демонстрация по хранене");
                var birds = context.Events.FirstOrDefault(e => e.Title == "Шоу с птици");

                var lion = context.Animals.FirstOrDefault(a => a.Name == "Лъв");
                var tiger = context.Animals.FirstOrDefault(a => a.Name == "Тигър");
                var parrot = context.Animals.FirstOrDefault(a => a.Name == "Папагал");
                var mosquito = context.Animals.FirstOrDefault(a => a.Name == "Комар");

                if (tour != null && lion != null && parrot != null)
                {
                    tour.Animals.Add(lion);
                    tour.Animals.Add(parrot);
                }

                if (feeding != null && lion != null && tiger != null)
                {
                    feeding.Animals.Add(lion);
                    feeding.Animals.Add(tiger);
                }

                if (birds != null && parrot != null && mosquito != null)
                {
                    birds.Animals.Add(parrot);
                    birds.Animals.Add(mosquito);
                }

                context.SaveChanges();
            }
        }

    }


}
