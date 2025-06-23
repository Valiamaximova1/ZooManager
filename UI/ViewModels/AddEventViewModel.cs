using BusinessLayer.DTOs;
using BusinessLayer.Factories.Interfaces;
using BusinessLayer.Services.Interfaces;
using Shared.Enums;
using System.Windows.Input;
using UI.Commands;

namespace UI.ViewModels
{
    public class AddEventViewModel : BaseViewModel
    {
        public EventDto NewEvent { get; set; }
        public List<AnimalSelectableViewModel> Animals { get; set; }
        public List<string> EventTypes { get; }

        public ICommand CreateCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly IEventService _eventService;
        private readonly IAnimalService _animalService;
        private readonly Action _onBack;

        public AddEventViewModel(
            IEventService eventService,
            IAnimalService animalService,
            IFactory<EventDto> eventFactory, 
            Action onBack)
        {
            _eventService = eventService;
            _animalService = animalService;
            _onBack = onBack;

            EventTypes = Enum.GetNames(typeof(EventType)).ToList();

            NewEvent = eventFactory.Create(); 

            CreateCommand = new AsyncDelegateCommand(OnCreateAsync);
            CancelCommand = new DelegateCommand(() => _onBack?.Invoke());

            LoadAnimalsAsync();
        }

        private async void LoadAnimalsAsync()
        {
            var animals = await _animalService.GetAllAsync();
            Animals = animals.Select(a => new AnimalSelectableViewModel(a)).ToList();
            OnPropertyChanged(nameof(Animals));
        }

        private async Task OnCreateAsync()
        {
            if (Enum.TryParse(NewEvent.Type.ToString(), out EventType parsedType))
                NewEvent.Type = parsedType;

            NewEvent.AnimalIds = Animals
                .Where(a => a.IsSelected)
                .Select(a => a.Animal.Id)
                .ToList();

            await _eventService.CreateAsync(NewEvent);
            _onBack?.Invoke();
        }
    }
}

