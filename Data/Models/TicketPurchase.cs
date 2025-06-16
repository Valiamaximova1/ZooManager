using Data.Models;
using Shared.Enums;
namespace Models
{

    public class TicketPurchase : BaseEntity
    {
        public int Quantity { get; set; }

        public Guid TicketTemplateId { get; set; }
        public TicketTemplate TicketTemplate { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public DateTime PurchasedAt { get; set; }

        public decimal Price { get; set; }
    }
}
