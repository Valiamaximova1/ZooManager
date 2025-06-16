using BusinessLayer.DTOs;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using UI.Commands;

namespace UI.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly UserDto _user;
        private readonly Action _onLogout;

        private readonly ITicketService _ticketService;
        public ObservableCollection<UserTicketDto> PurchasedTickets { get; set; } = new();

        public ICommand LogoutCommand { get; }

        public ProfileViewModel(UserDto user, Action onLogout, ITicketService ticketService)
        {
            _user = user;
            _onLogout = onLogout ;
            _ticketService = ticketService;
            LoadTickets();


            LogoutCommand = new DelegateCommand(Logout);
        }

        public string FirstName => _user.FirstName;
        public string LastName => _user.LastName;
        public string Email => _user.Email;

        private async void LoadTickets()
        {
            var tickets = await _ticketService.GetUserTicketsAsync(_user.Id);
            PurchasedTickets = new ObservableCollection<UserTicketDto>(tickets);
            OnPropertyChanged(nameof(PurchasedTickets));
        }

        private void Logout()
        {
            //  Изчистване на запомнения email
            Properties.Settings.Default.LastEmail = string.Empty;
            Properties.Settings.Default.Save();

            // Връщане към login екрана
            _onLogout.Invoke();
        }
    }

}
