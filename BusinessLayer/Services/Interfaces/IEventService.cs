using BusinessLayer.DTOs;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetAllAsync();
        Task<IEnumerable<EventDto>> GetFilteredAsync(EventType? type, DateTime? date);
        Task<IEnumerable<EventDto>> GetFilteredDateAsync( DateTime? date);
        Task<IEnumerable<EventDto>> GetFilteredCombinedAsync(EventType? type, DateTime? date, List<Guid> animalIds);


        Task UpdateAsync(EventDto dto);
        Task DeleteAsync(Guid id);
    }
}
