using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class UserTicketDto
    {
        public string TicketTitle { get; set; }
        public TicketType Type { get; set; }
        public DateTime EventDate { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchasedAt { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Price * Quantity;

    }

}
