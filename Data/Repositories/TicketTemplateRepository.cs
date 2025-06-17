using Data.Models;
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
    public class TicketTemplateRepository : ITicketTemplateRepository
    {
        private readonly ZooDbContext _context;

        public TicketTemplateRepository(ZooDbContext context)
        {
            _context = context;
        }

        public async Task<List<TicketTemplate>> GetAllAsync()
        {
            return await _context.TicketTemplates.Include(ticket => ticket.Event).ToListAsync();
        }

        public async Task<TicketTemplate> GetByIdAsync(Guid id)
        {
            return await _context.TicketTemplates.Include(ticket => ticket.Event).FirstOrDefaultAsync(ticket => ticket.Id == id);
        }

        public async Task UpdateAsync(TicketTemplate template)
        {
            _context.TicketTemplates.Update(template);
            await _context.SaveChangesAsync();
        }
    }

}
