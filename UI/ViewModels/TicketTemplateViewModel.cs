using BusinessLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UI.Commands;

namespace UI.ViewModels
{
    public class TicketTemplateViewModel : BaseViewModel
    {
        //    private readonly TicketTemplateDto _ticket;
        //    private readonly ITicketService _ticketService;
        //    private readonly Guid _userId;

        //    public TicketTemplateViewModel(TicketTemplateDto ticket, ITicketService ticketService, Guid userId)
        //    {
        //        _ticket = ticket;
        //        _ticketService = ticketService;
        //        _userId = userId;

        //        BuyCommand = new DelegateCommand(BuyTicket);
        //    }

        //    public string Title => _ticket.Title;
        //    public string Description => _ticket.Description;
        //    public int AvailableQuantity => _ticket.AvailableQuantity;
        //    public int QuantityToBuy { get; set; } = 1;

        //    public ICommand BuyCommand { get; }

        //    private async void BuyTicket()
        //    {
        //        if (QuantityToBuy <= 0 || QuantityToBuy > AvailableQuantity)
        //        {
        //            MessageBox.Show("Невалидна бройка.");
        //            return;
        //        }

        //        await _ticketService.PurchaseTicketAsync(_ticket.Id, _userId, QuantityToBuy);
        //        _ticket.AvailableQuantity -= QuantityToBuy;
        //        OnPropertyChanged(nameof(AvailableQuantity));
        //    }
    }

}
