using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TicketPurchaseRepository : ITicketPurchaseRepository
    {
        private readonly ZooDbContext _context;

        public TicketPurchaseRepository(ZooDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TicketPurchase ticketPurchase)
        {
            await _context.TicketPurchases.AddAsync(ticketPurchase);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TicketPurchase>> GetByUserIdAsync(Guid userId)
        {
            return await _context.TicketPurchases
                .Where(purchase => purchase.UserId == userId)
                .Include(purchase => purchase.TicketTemplate)
                .ToListAsync();
        }
    }

}
