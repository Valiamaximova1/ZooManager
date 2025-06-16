using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
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

        public ICommand LogoutCommand { get; }

        public ProfileViewModel(UserDto user, Action onLogout)
        {
            _user = user;
            _onLogout = onLogout ;

            LogoutCommand = new DelegateCommand(Logout);
        }

        public string FirstName => _user.FirstName;
        public string LastName => _user.LastName;
        public string Email => _user.Email;

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
