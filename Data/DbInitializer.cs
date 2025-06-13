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


            if (!context.Animals.Any())
            {

                // 👉 ВИНАГИ правим връзките независимо дали има животни
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

                var lion = context.Animals.FirstOrDefault(a => a.Name == "Лъв");
                var tiger = context.Animals.FirstOrDefault(a => a.Name == "Тигър");
                var parrot = context.Animals.FirstOrDefault(a => a.Name == "Папагал");
                var mosquito = context.Animals.FirstOrDefault(a => a.Name == "Комар");
                var gushter = context.Animals.FirstOrDefault(a => a.Name == "Гущер");
                var caca = context.Animals.FirstOrDefault(a => a.Name == "Цаца");

                // Всички проверки
                if (tour != null && lion != null) tour.Animals.Add(lion);
                if (tour != null && parrot != null) tour.Animals.Add(parrot);

                if (feeding != null && lion != null) feeding.Animals.Add(lion);
                if (feeding != null && tiger != null) feeding.Animals.Add(tiger);

                if (birds != null && parrot != null) birds.Animals.Add(parrot);
                if (birds != null && mosquito != null) birds.Animals.Add(mosquito);

                if (nightSafari != null && lion != null) nightSafari.Animals.Add(lion);
                if (nightSafari != null && gushter != null) nightSafari.Animals.Add(gushter);

                if (kidsFarm != null && parrot != null) kidsFarm.Animals.Add(parrot);
                if (kidsFarm != null && caca != null) kidsFarm.Animals.Add(caca);

                if (predatorFeeding != null && lion != null) predatorFeeding.Animals.Add(lion);
                if (predatorFeeding != null && tiger != null) predatorFeeding.Animals.Add(tiger);

                if (zoologTalk != null && lion != null) zoologTalk.Animals.Add(lion);
                if (zoologTalk != null && mosquito != null) zoologTalk.Animals.Add(mosquito);

                if (miniZoo != null && parrot != null) miniZoo.Animals.Add(parrot);
                if (miniZoo != null && caca != null) miniZoo.Animals.Add(caca);

                if (raptorFlight != null && parrot != null) raptorFlight.Animals.Add(parrot);

                if (tropicalNight != null && gushter != null) tropicalNight.Animals.Add(gushter);
                if (tropicalNight != null && mosquito != null) tropicalNight.Animals.Add(mosquito);

                if (cookingForAnimals != null && tiger != null) cookingForAnimals.Animals.Add(tiger);
                if (cookingForAnimals != null && caca != null) cookingForAnimals.Animals.Add(caca);

                if (volunteerDay != null && lion != null) volunteerDay.Animals.Add(lion);
                if (volunteerDay != null && gushter != null) volunteerDay.Animals.Add(gushter);
            }
            context.SaveChanges();
        }


    }


}
