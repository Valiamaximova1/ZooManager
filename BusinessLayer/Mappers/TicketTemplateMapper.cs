using BusinessLayer.DTOs;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mappers
{
    public static class TicketTemplateMapper
    {
        public static TicketTemplateDto ToDto(this TicketTemplate entity)
        {
            return new TicketTemplateDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Type = entity.Type,
                AvailableQuantity = entity.AvailableQuantity,
                Price = entity.Price
            };
        }

        public static TicketTemplate ToEntity(this TicketTemplateDto dto)
        {
            return new TicketTemplate
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Type = dto.Type,
                AvailableQuantity = dto.AvailableQuantity,
                Price = dto.Price
            };
        }
    }

}
