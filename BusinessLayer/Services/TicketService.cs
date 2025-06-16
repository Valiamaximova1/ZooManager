using BusinessLayer.DTOs;
using BusinessLayer.Services.Interfaces;
using Data.Repositories.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly IEventRepository _eventRepo;

        public TicketService(ITicketRepository ticketRepo, IEventRepository eventRepo)
        {
            _ticketRepo = ticketRepo;
            _eventRepo = eventRepo;
        }

        //public async Task<List<TicketDto>> GetUserTicketsAsync(Guid userId)
        //{
        //    var tickets = await _ticketRepo.GetByUserIdAsync(userId);
        //    return tickets.Select(t => new TicketDto
        //    {
        //        Id = t.Id,
        //        Title = t.Title,
        //        Description = t.Description,
        //        Quantity = t.Quantity,
        //        Type = t.Type,
        //        EventId = t.EventId,
        //        EventTitle = t.Event.Title,
        //        UserId = t.UserId,
        //        UserEmail = t.User.Email
        //    }).ToList();
        //}

        //public async Task BuyTicketAsync(TicketDto dto)
        //{
        //    var ev = await _eventRepo.GetByIdAsync(dto.EventId);
        //    if (ev == null) throw new Exception("Събитието не е намерено.");

        //    if (dto.Quantity <= 0) throw new Exception("Невалидно количество.");

        //    var ticket = new TicketPurchase
        //    {
        //        Id = Guid.NewGuid(),
        //        Title = dto.Title,
        //        Description = dto.Description,
        //        Quantity = dto.Quantity,
        //        Type = dto.Type,
        //        EventId = dto.EventId,
        //        UserId = dto.UserId
        //    };

        //    ev.AvailableTickets -= dto.Quantity;

        //    await _ticketRepo.AddAsync(ticket);
        //    await _ticketRepo.SaveAsync();
        //}
    }
}
