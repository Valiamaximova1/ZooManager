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
            var events = await _eventRepository.GetAllAsync();
            return events.Select(a => a.ToDto());
        }
        public async Task<IEnumerable<EventDto>> GetFilteredAsync(EventType? type, DateTime? date)
        {
            var all = await _eventRepository.GetAllAsync();

            return all
                .Where(e => (!type.HasValue || e.Type == type.Value)
                          && (!date.HasValue || e.Date.Date == date.Value.Date))
                .Select(e => e.ToDto());
        }

        public async Task UpdateAsync(EventDto dto)
        {
            var existing = await _eventRepository.GetByIdAsync(dto.Id);
            if (existing is null)
                throw new ArgumentException("Невалидно събитие!");

            existing.Title = dto.Title;
            existing.Type = dto.Type;
            existing.Description = dto.Description;
            existing.Date = dto.Date;

            // Обновяване на животните (many-to-many)
            existing.Animals.Clear();
            foreach (var animalId in dto.AnimalIds)
            {
                var animal = await _animalRepository.GetByIdAsync(animalId);
                if (animal != null)
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
