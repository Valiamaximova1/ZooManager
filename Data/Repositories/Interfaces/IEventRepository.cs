using Models;
using Shared.Enums;
namespace Data.Repositories.Interfaces
{
    public interface IEventRepository
    {
        IQueryable<Event> GetAll();
        Task<IEnumerable<Event>> GetAllAsync();
        Task<IEnumerable<Event>> GetAllAsyncWithAnimals();
        Task<IEnumerable<Event>> GetByTypeAsync(EventType type);
        Task<IEnumerable<Event>> GetByDateAsync(DateTime date);
        Task<Event> GetByIdAsync(Guid id);
        Task AddAsync(Event ev);
        Task UpdateAsync(Event ev);
        Task DeleteAsync(Event ev);
        IQueryable<Event> GetAllWithIncludes();
        Task<Event> GetByIdWithAnimalsAsync(Guid id);
    }
}
