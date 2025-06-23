using BusinessLayer.DTOs;
using BusinessLayer.Factories.Interfaces;
using Shared.Enums;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Factories
{
    public class EventFactory : IFactory<EventDto>
    {
        public EventDto Create()
        {
            return new EventDto
            {
                Id = Guid.NewGuid(),
                Title = string.Empty,
                Description = string.Empty,
                Type = EventType.Неопределен,
                Date = DateTime.Today,
                AnimalIds = new List<Guid>(),
                AnimalNamesCollection = new List<string>()
             };
        }
    }
}
