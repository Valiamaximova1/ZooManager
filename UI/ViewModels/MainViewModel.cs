using BusinessLayer.DTOs;
using BusinessLayer.Factories;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using Data;
using Data.Repositories;
using DevExpress.Internal;
using DevExpress.Xpo;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Windows;
using UI.ViewModels;
using UI.Views;



namespace UI.ViewModels
{

    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;

        private readonly IUserService _userService;
        private readonly IAnimalService _animalService;
        private readonly IEventService _eventService;
        private readonly ITicketService _ticketService;

        private UserDto _currentUser;
        private readonly FactoryProvider _factoryProvider = new FactoryProvider();



        public MainViewModel()
        {

            DbInitializer.Initialize();

            var factory = new ZooDbContextFactory();
            var context = factory.CreateDbContext(Array.Empty<string>());

            var userRepository = new UserRepository(context);
            var animalRepository = new AnimalRepository(context);
            var eventRepository = new EventRepository(context);
            var ticketTemplateRepo = new TicketTemplateRepository(context);
            var ticketPurchaseRepo = new TicketPurchaseRepository(context);

            _userService = new UserService(userRepository);
            _animalService = new AnimalService(animalRepository);
            _eventService = new EventService(eventRepository, animalRepository);
            _ticketService = new TicketService(ticketTemplateRepo, ticketPurchaseRepo);

            ShowLogin();
        }

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }


        private void ShowLogin()
        {
            var loginVM = new LoginViewModel(_userService);
            //тук му казвам абонирам се за събитие
            loginVM.LoginSuccessful += OnLoginSuccess;
            loginVM.NavigateToRegisterRequested += ShowRegister;
            CurrentViewModel = loginVM;
        }

        private void ShowRegister()
        {
            var registerVM = new RegisterViewModel(_userService);
            registerVM.NavigateToLoginRequested += ShowLogin;
            CurrentViewModel = registerVM;
        }

        private void OnLoginSuccess(UserDto user)
        {
            //CurrentViewModel = new HomeViewModel(_animalService,_eventService, user);
            _currentUser = user;
            _userService.SetCurrentUser(user);

            var homeVM = new HomeViewModel(_animalService, _eventService, _ticketService, user, _userService, _factoryProvider);

            //абонирам се за събитие
            homeVM.LogoutRequested += OnLogoutRequested;

            CurrentViewModel = homeVM;
        }

        private void OnLogoutRequested()
        {
            _currentUser = null;
            ShowLogin(); // връщаме се към login екрана
        }
    }


}
