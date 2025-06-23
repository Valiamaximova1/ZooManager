using BusinessLayer.DTOs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mappers
{
    public static class TicketPurchaseMapper
    {
        public static TicketPurchaseDto ToDto(this TicketPurchase entity)
        {
            return new TicketPurchaseDto
            {
                Id = entity.Id,
                TicketTemplateId = entity.TicketTemplateId,
                UserId = entity.UserId,
                Quantity = entity.Quantity,
                PurchasedAt = entity.PurchasedAt,
                Price = entity.Price
            };
        }

        public static TicketPurchase ToEntity(this TicketPurchaseDto dto)
        {
            return new TicketPurchase
            {
                Id = dto.Id,
                TicketTemplateId = dto.TicketTemplateId,
                UserId = dto.UserId,
                Quantity = dto.Quantity,
                PurchasedAt = dto.PurchasedAt,
                Price = dto.Price

            };
        }
    }

}
