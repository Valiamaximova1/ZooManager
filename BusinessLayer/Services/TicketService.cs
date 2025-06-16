using BusinessLayer.DTOs;
using BusinessLayer.Mappers;
using BusinessLayer.Services.Interfaces;
using Data.Models;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketTemplateRepository _templateRepo;
        private readonly ITicketPurchaseRepository _purchaseRepo;



        public TicketService(ITicketTemplateRepository templateRepo, ITicketPurchaseRepository purchaseRepo)
        {
            _templateRepo = templateRepo;
            _purchaseRepo = purchaseRepo;
        }

        public async Task<IEnumerable<TicketTemplateDto>> GetAllTemplatesAsync()
        {
            var templates = await _templateRepo.GetAllAsync();
            return templates.Select(TicketTemplateMapper.ToDto);
        }

        public async Task PurchaseTicketAsync(Guid userId, Guid templateId, int quantity)
        {
            var template = await _templateRepo.GetByIdAsync(templateId);
            if (template == null || template.AvailableQuantity < quantity)
                throw new InvalidOperationException("Not enough tickets available.");

            template.AvailableQuantity -= quantity;
            await _templateRepo.UpdateAsync(template);

            var purchase = new TicketPurchase
            {
                Id = Guid.NewGuid(),
                TicketTemplateId = templateId,
                UserId = userId,
                Quantity = quantity,
                PurchasedAt = DateTime.Now
            };

            await _purchaseRepo.AddAsync(purchase);
        }

        public async Task<List<UserTicketDto>> GetUserTicketsAsync(Guid userId)
        {
            var purchases = await _purchaseRepo.GetByUserIdAsync(userId);
            return purchases
                .Where(p => p.TicketTemplate != null)
                .Select(p => UserTicketMapper.ToUserTicketDto(p))
                .ToList();
        }

    }

}
