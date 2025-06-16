using BusinessLayer.DTOs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mappers
{
    public static class UserTicketMapper
    {
        public static UserTicketDto ToUserTicketDto(TicketPurchase purchase)
        {
            return new UserTicketDto
            {
                TicketTitle = purchase.TicketTemplate.Title,
                Type = purchase.TicketTemplate.Type,
                Quantity = purchase.Quantity,
                PurchasedAt = purchase.PurchasedAt
            };
        }
    }

}
