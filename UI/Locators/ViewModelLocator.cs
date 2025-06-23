using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using Data;
using Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.ViewModels;

namespace UI.Locators
{
    public class ViewModelLocator
    {
        private static ServiceProvider _provider;

        static ViewModelLocator()
        {
            var services = new ServiceCollection();

            // Add DbContext and Repositories
            var factory = new ZooDbContextFactory();
            var context = factory.CreateDbContext(Array.Empty<string>());

            var animalRepository = new AnimalRepository(context);
            var eventRepository = new EventRepository(context);


            // Add Services (manually constructed because we don't use scoped lifetime here)
            var animalService = new AnimalService(animalRepository);
            var eventService = new EventService(eventRepository, animalRepository);

            // Register ViewModels
            services.AddSingleton<IAnimalService>(animalService);
            services.AddSingleton<IEventService>(eventService);

            services.AddSingleton<EventsViewModel>();
            services.AddSingleton<AnimalsViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<ProfileViewModel>();

            _provider = services.BuildServiceProvider();
        }

        public EventsViewModel EventsViewModel => _provider.GetRequiredService<EventsViewModel>();
        public AnimalsViewModel AnimalsViewModel => _provider.GetRequiredService<AnimalsViewModel>();
        public HomeViewModel HomeViewModel => _provider.GetRequiredService<HomeViewModel>();
        public ProfileViewModel ProfileViewModel => _provider.GetRequiredService<ProfileViewModel>();
    }

}