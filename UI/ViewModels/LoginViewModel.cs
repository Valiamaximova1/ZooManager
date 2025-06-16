using BusinessLayer.DTOs;
using BusinessLayer.Services.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using UI.Commands;

namespace UI.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        private string _email = string.Empty;
        private string _password = string.Empty;

        private readonly AsyncDelegateCommand _loginCommand;
        public ICommand LoginCommand => _loginCommand;
        public ICommand NavigateToRegisterCommand { get; }

        public event Action<UserDto> LoginSuccessful;
        public event Action NavigateToRegisterRequested;

        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
            _loginCommand = new AsyncDelegateCommand(LoginAsync, CanLogin);
            NavigateToRegisterCommand = new AsyncDelegateCommand(() => Task.Run(() => NavigateToRegisterRequested?.Invoke()));
            var savedEmail = Properties.Settings.Default.LastEmail;
            if (!string.IsNullOrWhiteSpace(savedEmail))
            {
                Email = savedEmail;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                    _loginCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                    _loginCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private async Task LoginAsync()
        {
            try
            {
                var result = await _userService.LoginAsync(new UserLoginDto
                {
                    Email = Email,
                    Password = Password

                });
                Properties.Settings.Default.LastEmail = Email;
                Properties.Settings.Default.Save();
                LoginSuccessful.Invoke(result);
            }
            catch (Exception)
            {
                MessageBox.Show("Невалидни данни за вход");
            }





        }
        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
        }
    }


}
