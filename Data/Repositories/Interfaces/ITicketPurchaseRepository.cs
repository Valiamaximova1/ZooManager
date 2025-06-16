using Data.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface ITicketPurchaseRepository
    {
        Task AddAsync(TicketPurchase ticketPurchase);
        Task<IEnumerable<TicketPurchase>> GetByUserIdAsync(Guid userId);
    }
}
