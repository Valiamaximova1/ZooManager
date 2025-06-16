using Data.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface ITicketTemplateRepository
    {
        Task<List<TicketTemplate>> GetAllAsync();
        Task<TicketTemplate> GetByIdAsync(Guid id);
        Task UpdateAsync(TicketTemplate template);
    }
}
