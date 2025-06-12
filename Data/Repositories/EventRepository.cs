using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ZooDbContext _context;

        public EventRepository(ZooDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events
                .Include(e => e.Animals)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetByTypeAsync(EventType type)
        {
            return await _context.Events
                .Include(e => e.Animals)
                .Where(e => e.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetByDateAsync(DateTime date)
        {
            return await _context.Events
                .Include(e => e.Animals)
                .Where(e => e.Date.Date == date.Date)
                .ToListAsync();
        }

        public async Task<Event> GetByIdAsync(Guid id)
        {
            return await _context.Events
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Event ev)
        {
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Event ev)
        {
            _context.Events.Update(ev);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Event ev)
        {
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
        }
    }
}
