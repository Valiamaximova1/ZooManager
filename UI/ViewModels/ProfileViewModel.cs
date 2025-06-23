using BusinessLayer.DTOs;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using UI.Commands;

namespace UI.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly UserDto _user;
        private readonly Action _onLogout;
        private string _currentPassword;
        private string _newPassword;
        private string _confirmPassword;

        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;
        private ObservableCollection<UserTicketDto> _purchasedTickets;
        public ICommand ChangePasswordCommand { get; }
        public ICommand LogoutCommand { get; }

        public ProfileViewModel(UserDto user, Action onLogout, ITicketService ticketService, IUserService userService)
        {
            _user = user;
            _onLogout = onLogout;
             _userService = userService;
            _ticketService = ticketService;
            _ticketService.TicketPurchased += async (ticket) =>
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    PurchasedTickets.Add(ticket); 
                });
            };


            Task.Run(LoadTickets);

            LogoutCommand = new DelegateCommand(Logout);
            ChangePasswordCommand = new DelegateCommand(ChangePassword);
        }

        public string FirstName => _user.FirstName;
        public string LastName => _user.LastName;
        public string Email => _user.Email;

        public string CurrentPassword
        {
            get => _currentPassword;
            set { _currentPassword = value; OnPropertyChanged(); }
        }

        public string NewPassword
        {
            get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(); }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }
        public ObservableCollection<UserTicketDto> PurchasedTickets
        {
            get => _purchasedTickets;
            set
            {
                _purchasedTickets = value;
                OnPropertyChanged();
            }
        }

        private async void ChangePassword()
        {
            if (string.IsNullOrWhiteSpace(CurrentPassword) ||
                string.IsNullOrWhiteSpace(NewPassword) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                MessageBox.Show("Моля, попълнете всички полета.");
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                 MessageBox.Show("Новата парола и потвърждението не съвпадат.");
                return;
            }

            bool success = await _userService.ChangePasswordAsync(_user.Email, CurrentPassword, NewPassword);

            if (success)
            {
                 MessageBox.Show("Паролата е променена успешно.");
                CurrentPassword = NewPassword = ConfirmPassword = string.Empty;
            }
            else
            {
                 MessageBox.Show("Грешна текуща парола.");
            }
        }


    
        private async  Task LoadTickets()
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
