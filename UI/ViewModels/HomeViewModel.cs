using BusinessLayer.DTOs;
using BusinessLayer.Services.Interfaces;
using System.Windows.Input;
using UI.Commands;

namespace UI.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IAnimalService _animalService;
        private readonly IEventService _eventService;

        public ICommand ShowAnimalsCommand { get; }
        public ICommand ShowEventsCommand { get; }

        private BaseViewModel _currentViewModel;
        private readonly UserDto _user;


        public HomeViewModel(IAnimalService animalService, IEventService eventService, UserDto user)
        {
            _animalService = animalService;
            _eventService = eventService;
            _user = user;

            ShowAnimalsCommand = new DelegateCommand(ShowAnimals);
            ShowEventsCommand = new DelegateCommand(ShowEvents);

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
            CurrentViewModel = new EventsViewModel(_eventService);
        }
    }
}
