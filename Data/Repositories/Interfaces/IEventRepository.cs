using Models;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<IEnumerable<Event>> GetAllAsyncWithAnimals();
        Task<IEnumerable<Event>> GetByTypeAsync(EventType type);
        Task<IEnumerable<Event>> GetByDateAsync(DateTime date);
        Task<Event> GetByIdAsync(Guid id);
        Task AddAsync(Event ev);
        Task UpdateAsync(Event ev);
        Task DeleteAsync(Event ev);
        IQueryable<Event> GetAllWithIncludes();
    }
}
