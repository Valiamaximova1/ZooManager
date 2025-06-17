using BusinessLayer.DTOs;
using BusinessLayer.Mappers;
using BusinessLayer.Services.Interfaces;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Shared.Enums;
using System;
using System.Collections.Generic;
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



        public async Task UpdateAsync(EventDto dto)
        {
            var existing = await _eventRepository.GetByIdAsync(dto.Id);
            if (existing is null)
                throw new ArgumentException("Събитието не съществува.");

            var allAnimals = await _animalRepository.GetAllAsync();
            existing.UpdateEntity(dto, allAnimals.ToList());

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
