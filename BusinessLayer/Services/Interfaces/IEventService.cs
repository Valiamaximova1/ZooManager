using BusinessLayer.DTOs;
using BusinessLayer.Events;
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
        event EventHandler<EventChangedEventArgs>? EventChanged;

        Task<IEnumerable<EventDto>> GetAllAsync();
        Task<IEnumerable<EventDto>> GetFilteredAsync(EventType? type, DateTime? date);
        Task<IEnumerable<EventDto>> GetFilteredDateAsync( DateTime? date);
        Task<IEnumerable<EventDto>> GetFilteredCombinedAsync(EventType? type, DateTime? date, List<Guid> animalIds);
        Task UpdateAsync(EventDto dto);
        Task CreateAsync(EventDto eventDto);
        Task DeleteAsync(Guid id);
    }
}
