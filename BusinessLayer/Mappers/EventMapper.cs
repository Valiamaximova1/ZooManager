using BusinessLayer.DTOs;
using DocumentFormat.OpenXml.Vml.Office;
using DocumentFormat.OpenXml.Wordprocessing;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BusinessLayer.Mappers
{
    public static class EventMapper
    {
        public static EventDto ToDto(this Event ev)
        {
            return new EventDto
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                Date = ev.Date,
                Type = ev.Type,
                AnimalIds = ev.Animals?.Select(a => a.Id).ToList() ?? new(),
                AnimalNamesCollection = ev.Animals?.Select(a => a.Name).ToList() ?? new(),
                 TicketCount = ev.TicketTemplates?.Sum(t => t.AvailableQuantity) ?? 0
            };
        }


        public static Event ToEntity(this EventDto dto, List<Animal> animals)
        {
            var entity = new Event
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Date = dto.Date,
                Type = dto.Type,
                Animals = new List<Animal>(),
            };

            foreach (var id in dto.AnimalIds.Distinct())
            {
                var animal = animals.FirstOrDefault(a => a.Id == id);
                if (animal != null)
                    entity.Animals.Add(animal);
            }

            return entity;
        }

        public static void UpdateEntity(this Event entity, EventDto dto, List<Animal> animals)
        {
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.Date = dto.Date;
            entity.Type = dto.Type;

            //entity.Animals.Clear();

            //foreach (var id in dto.AnimalIds.Distinct())
            //{
            //    var animal = animals.FirstOrDefault(a => a.Id == id);
            //    if (animal != null)
            //        entity.Animals.Add(animal);
            //}
        }
    }
}
