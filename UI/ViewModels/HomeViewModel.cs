using BusinessLayer.DTOs;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using Data;
using Data.Repositories;
using System.Windows.Input;
using UI.Commands;

namespace UI.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {

        private readonly IAnimalService _animalService;
        private readonly IEventService _eventService;
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;

        private string _selectedTab;

        private BaseViewModel _currentViewModel;
        private readonly UserDto _user;
        public event Action LogoutRequested;



        public HomeViewModel(IAnimalService animalService, IEventService eventService, ITicketService ticketService, UserDto user, IUserService userService)
        {
            _animalService = animalService;
            _eventService = eventService;
            _ticketService = ticketService;
            _user = user;
            _userService = userService;

            ShowAnimalsCommand = new DelegateCommand(ShowAnimals);
            ShowEventsCommand = new DelegateCommand(ShowEvents);
            ShowTicketsCommand = new DelegateCommand(ShowTickets);
            ShowProfileCommand = new DelegateCommand(ShowProfile);

            // Зареждаме начално екрана с животни
            ShowAnimals();
         
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

        public string SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowAnimalsCommand { get; }
        public ICommand ShowEventsCommand { get; }
        public ICommand ShowTicketsCommand { get; }
        public ICommand ShowProfileCommand { get; }

        
        private void ShowAnimals()
        {
            CurrentViewModel = new AnimalsViewModel(_animalService);
            SelectedTab = "Animals";
        }

        private void ShowEvents()
        {
            CurrentViewModel = new EventsViewModel(_eventService, _animalService);
            SelectedTab = "Events";
        }
        private void ShowTickets()
        {
            CurrentViewModel = new TicketsViewModel(_ticketService, _user.Id);
            SelectedTab = "Tickets";
        }
        private void ShowProfile()
        {
            //LogoutRequested е събитие, декларирано в HomeViewModel
            CurrentViewModel = new ProfileViewModel(_user, () => LogoutRequested?.Invoke(), _ticketService, _userService);
            SelectedTab = "Profile";
        }



    }
}
