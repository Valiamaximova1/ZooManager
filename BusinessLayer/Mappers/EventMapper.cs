using BusinessLayer.DTOs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Type = ev.Type
            };
        }

        public static Event ToEntity(this EventDto dto)
        {
            return new Event
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Date = dto.Date,
                Type = dto.Type
            };
        }
    }
}
