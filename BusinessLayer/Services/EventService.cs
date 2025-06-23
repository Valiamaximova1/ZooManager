using BusinessLayer.DTOs;
using BusinessLayer.Mappers;
using BusinessLayer.Services.Interfaces;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAnimalRepository _animalRepository;



        public EventService(IEventRepository eventRepository, IAnimalRepository animalRepository)
        {
            _eventRepository = eventRepository;
            _animalRepository = animalRepository;
        }


        public async Task<IEnumerable<EventDto>> GetAllAsync()
        {
            var events = await _eventRepository.GetAllAsyncWithAnimals();
            return events.Select(eventA => eventA.ToDto());
        }

        public async Task<IEnumerable<EventDto>> GetFilteredAsync(EventType? type, DateTime? date)
        {
            var allfilterEvents = await _eventRepository.GetAllAsync();

            return allfilterEvents
                .Where(eventFilter => (!type.HasValue || eventFilter.Type == type.Value)
                          && (!date.HasValue || eventFilter.Date.Date == date.Value.Date))
                .Select(e => e.ToDto());
        }

        public async Task<IEnumerable<EventDto>> GetFilteredDateAsync(DateTime? date)
        {
            var allfilterEvents = await _eventRepository.GetAllAsync();

            return allfilterEvents
                .Where(eventFilter => (!date.HasValue || eventFilter.Date.Date == date.Value.Date))
                .Select(e => e.ToDto());
        }

        public async Task<IEnumerable<EventDto>> GetFilteredCombinedAsync(EventType? type, DateTime? date, List<Guid> animalIds)
        {
            var events = await _eventRepository.GetAllAsync();

            if (type.HasValue)
                events = events.Where(e => e.Type == type.Value);

            if (date.HasValue)
                events = events.Where(e => e.Date.Date == date.Value.Date);

            if (animalIds != null && animalIds.Any())
                events = events.Where(e => e.Animals.Any(a => animalIds.Contains(a.Id)));

            return events.Select(e => e.ToDto());
        }

        public async Task UpdateAsync(EventDto dto)
        { 
            var existing = await _eventRepository.GetByIdWithAnimalsAsync(dto.Id);
            if (existing == null)
                throw new ArgumentException("Събитието не съществува.");

            var animalIds = dto.AnimalIds.Distinct().ToList();
            var animalsToAttach = await _animalRepository
                .GetAll()
                .Where(a => animalIds.Contains(a.Id))
                .ToListAsync();

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Date = dto.Date;
            existing.Type = dto.Type;

            existing.Animals.Clear();
            foreach (var animal in animalsToAttach)
            {
                existing.Animals.Add(animal);
            }

            await _eventRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _eventRepository.GetByIdAsync(id);
            if (entity == null)
                throw new ArgumentException("Събитието не е намерено.");

            await _eventRepository.DeleteAsync(entity);
        }

    }
}
