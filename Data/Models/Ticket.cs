using Shared.Enums;
namespace Models
{

    public class Ticket : BaseEntity
    {
        public TicketType Type { get; set; }
        public int Quantity { get; set; }

        public Guid EventId { get; set; }
        public Event Event { get; set; }

        public Guid UserId { get; set; }   
        public User User { get; set; }
    }
}
