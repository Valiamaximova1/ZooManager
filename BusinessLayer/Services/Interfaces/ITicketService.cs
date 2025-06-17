using BusinessLayer.DTOs;
using Data.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketTemplateDto>> GetAllTemplatesAsync();
        Task PurchaseTicketAsync(Guid userId, Guid templateId, int quantity);
        Task<List<UserTicketDto>> GetUserTicketsAsync(Guid userId);
    }
}
