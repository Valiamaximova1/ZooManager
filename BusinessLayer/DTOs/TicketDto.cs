using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketType Type { get; set; }
        public int Quantity { get; set; }

        public Guid EventId { get; set; }
        public string EventTitle { get; set; }

        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
    }
}
