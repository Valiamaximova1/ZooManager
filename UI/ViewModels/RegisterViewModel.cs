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
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IUserService _userService;


        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;

        private string _confirmPassword = string.Empty;

        private readonly AsyncDelegateCommand _registerCommand;
        private readonly AsyncDelegateCommand _navigateToLoginCommand;

        public ICommand RegisterCommand => _registerCommand;


        public ICommand NavigateToLoginCommand => _navigateToLoginCommand;

        public event Action NavigateToLoginRequested;

        public RegisterViewModel(IUserService userService)
        {
            _userService = userService;

            _registerCommand = new AsyncDelegateCommand(RegisterAsync, CanRegister);
            _navigateToLoginCommand = new AsyncDelegateCommand(() => Task.Run(() => NavigateToLoginRequested.Invoke()));

        }
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged();
                    _registerCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged();
                    _registerCommand.RaiseCanExecuteChanged();
                }
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
                    _registerCommand.RaiseCanExecuteChanged();
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
                    _registerCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    OnPropertyChanged();
                    _registerCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private async Task RegisterAsync()
        {
            try
            {
                await _userService.RegisterAsync(new UserRegisterDto
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Password = Password,
                    ConfirmPassword = ConfirmPassword
                });
                MessageBox.Show("Успешна регистрация!");
                Properties.Settings.Default.LastEmail = string.Empty;
                Properties.Settings.Default.Save();
                NavigateToLoginRequested.Invoke();
            }
            catch (Exception)
            {
                MessageBox.Show("Невалидни данни");
            }
        }

        private bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(Email)
                && !string.IsNullOrWhiteSpace(Password)
                && Password == ConfirmPassword;
        }
    }
}
