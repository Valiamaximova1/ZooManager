using Models;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class TicketTemplate : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketType Type { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal Price { get; set; }

        public Guid EventId { get; set; }
        public Event Event { get; set; }

        public ICollection<TicketPurchase> TicketPurchases { get; set; } = new List<TicketPurchase>();

    }

}
