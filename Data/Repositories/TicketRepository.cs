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
    public class TicketRepository : ITicketRepository
    {
        private readonly ZooDbContext _context;

        public TicketRepository(ZooDbContext context)
        {
            _context = context;
        }

        //public Task<List<TicketPurchase>> GetAllAsync() => _context.Tickets.Include(t => t.Event).Include(t => t.User).ToListAsync();

        //public Task<List<TicketPurchase>> GetByUserIdAsync(Guid userId) =>
        //    _context.Tickets.Include(t => t.Event).Where(t => t.UserId == userId).ToListAsync();

        //public async Task AddAsync(TicketPurchase ticket)
        //{
        //    await _context.Tickets.AddAsync(ticket);
        //}

        //public async Task SaveAsync()
        //{
        //    await _context.SaveChangesAsync();
        //}
    }

}
