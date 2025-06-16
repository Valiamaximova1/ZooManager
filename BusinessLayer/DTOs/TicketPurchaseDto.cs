using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class TicketPurchaseDto
    {
        public Guid Id { get; set; }
        public Guid TicketTemplateId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchasedAt { get; set; }
        public decimal Price { get; set; }
    }
}
