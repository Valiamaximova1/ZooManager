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

        public ICommand ShowAnimalsCommand { get; }
        public ICommand ShowEventsCommand { get; }
        public ICommand ShowTicketsCommand { get; }
        public ICommand ShowProfileCommand { get; }
        

        private BaseViewModel _currentViewModel;
        private readonly UserDto _user;

        public event Action LogoutRequested;



        public HomeViewModel(IAnimalService animalService, IEventService eventService, ITicketService ticketService, UserDto user)
        {
            _animalService = animalService;
            _eventService = eventService;
            _ticketService = ticketService;
            _user = user;

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
        private void ShowAnimals()
        {
            CurrentViewModel = new AnimalsViewModel(_animalService);
        }

        private void ShowEvents()
        {
            CurrentViewModel = new EventsViewModel(_eventService, _animalService);
        }
        private void ShowTickets()
        {
            CurrentViewModel = new TicketsViewModel(_ticketService, _user.Id);
        }
        private void ShowProfile()
        {
            //LogoutRequested е събитие, декларирано в HomeViewModel
            CurrentViewModel = new ProfileViewModel(_user, () => LogoutRequested?.Invoke(), _ticketService);
        }



    }
}
