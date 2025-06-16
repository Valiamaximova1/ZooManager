using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class TicketTemplateDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketType Type { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal Price { get; set; }
    }

}
