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


        private AnimalsViewModel _animalViewModel;
        private EventsViewModel _eventViewModel;
        private TicketsViewModel _ticketViewModel;
        private ProfileViewModel _profileViewModel;

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

            ShowAddAnimalCommand = new DelegateCommand(ShowAddAnimalPage);
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
        public ICommand ShowAddAnimalCommand { get; }


        private void ShowAnimals()
        {
            if (_animalViewModel == null)
            {
                _animalViewModel = new AnimalsViewModel(_animalService);
                _animalViewModel.AddAnimalRequested += ShowAddAnimalPage;
            }
            CurrentViewModel = _animalViewModel;
            SelectedTab = "Animals";
        }

        private void ShowEvents()
        {
            if (_eventViewModel == null)
            {
                _eventViewModel = new EventsViewModel(_eventService, _animalService);
            }
            CurrentViewModel = _eventViewModel;
            SelectedTab = "Events";
        }
        private void ShowTickets()
        {
            if (_ticketViewModel == null)
                _ticketViewModel = new TicketsViewModel(_ticketService, _user.Id);

            CurrentViewModel = _ticketViewModel;
            SelectedTab = "Tickets";
        }
        private void ShowProfile()
        {
            //if (_profileViewModel == null)
                //LogoutRequested е събитие, декларирано в HomeViewModel
                _profileViewModel = new ProfileViewModel(_user, () => LogoutRequested?.Invoke(), _ticketService, _userService);

            CurrentViewModel = _profileViewModel;
            SelectedTab = "Profile";
        }


        private void ShowAddAnimalPage()
        {
            var addViewModel = new AddAnimalViewModel(_animalService, async () =>
            {
                // Презареждаме списъка с животни
                if (_animalViewModel == null)
                    _animalViewModel = new AnimalsViewModel(_animalService);
                else
                    await _animalViewModel.LoadAnimalsAsync(); 

                CurrentViewModel = _animalViewModel;
            });

            CurrentViewModel = addViewModel;
           
        }
    }
}
