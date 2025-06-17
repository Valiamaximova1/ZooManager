using BusinessLayer.DTOs;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using DevExpress.Xpo.DB;
using Microsoft.Extensions.Logging;
using Models;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UI.Commands;

namespace UI.ViewModels
{

    public class EventsViewModel : BaseViewModel
    {
        private readonly IEventService _eventService;
        private readonly IAnimalService _animalService;

        private string _selectedType = "Всички";
        private DateTime? _selectedDate = null;

        public Array EventTypeValues => Enum.GetValues(typeof(EventType));


        public ObservableCollection<EventDto> Events { get; } = new();
        public ObservableCollection<string> EventTypes { get; } =
        new ObservableCollection<string>(new[] { "Всички" }.Concat(Enum.GetNames(typeof(EventType))));
        public ObservableCollection<EventType> EditableEventTypes { get; } =
      new ObservableCollection<EventType>((EventType[])Enum.GetValues(typeof(EventType)));


        public ObservableCollection<AnimalSelectableViewModel> SelectableAnimals { get; } = new();

        public ObservableCollection<AnimalDto> AllAnimals { get; } = new();
        public ObservableCollection<AnimalDto> SelectedAnimals { get; } = new();

        private EventDto _selectedEvent;
        private EventDto _editingEvent;

        private bool _isDetailsVisible;
        private bool _isEditMode;


        public EventsViewModel(IEventService eventService, IAnimalService animalService)
        {
            _eventService = eventService;
            _animalService = animalService;

            SearchCommand = new AsyncDelegateCommand(LoadEventsAsync);
            EditEventCommand = new DelegateCommand(OnEdit);
            DeleteEventCommand = new AsyncDelegateCommand(OnDeleteAsync);
            SaveEventCommand = new AsyncDelegateCommand(OnSaveAsync);
            LoadEventsCommand = new AsyncDelegateCommand(LoadEventsAsync);



            LoadEventsAsync();
        }

        public string SelectedType
        {
            get => _selectedType;
            set
            {

                _selectedType = value;
                OnPropertyChanged();
            }
        }
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        public EventDto SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                if (_selectedEvent != value)
                {
                    foreach (var ev in Events)
                        ev.IsEditMode = false;

                    _selectedEvent = value;
                    IsDetailsVisible = value != null;
                    OnPropertyChanged();

                    LoadSelectedAnimalsAsync();
                    LoadAllAnimalsAsync();
                }

            }
        }

        public EventDto EditingEvent
        {
            get => _editingEvent;
            set
            {
                _editingEvent = value;

                OnPropertyChanged();
            }
        }

        public bool IsDetailsVisible
        {
            get => _isDetailsVisible;
            set
            {
                _isDetailsVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand EditEventCommand { get; }
        public ICommand DeleteEventCommand { get; }
        public ICommand SaveEventCommand { get; }
        public ICommand LoadEventsCommand { get; }



        private async Task LoadEventsAsync()
        {

            Events.Clear();
            if (SelectedType.Equals("Всички"))
            {
                var all = await _eventService.GetAllAsync();
                foreach (var a in all)
                    Events.Add(a);
                SelectedDate = null;
            }
            else
            {
                if (Enum.TryParse<EventType>(SelectedType, out var type))
                {

                    var dateToUse = SelectedDate ?? DateTime.Today;
                    var results = await _eventService.GetFilteredAsync(type, SelectedDate);

                    foreach (var ev in results)
                        Events.Add(ev);
                }

            }

        }

        private async Task LoadSelectedAnimalsAsync()
        {
            if (SelectedEvent == null || SelectedEvent.AnimalIds == null)
                return;

            if (AllAnimals.Count == 0)
                await LoadAllAnimalsAsync();

            SelectedAnimals.Clear();

            foreach (var animal in AllAnimals.Where(a => SelectedEvent.AnimalIds.Contains(a.Id)))
                SelectedAnimals.Add(animal);
        }
        private async Task LoadAllAnimalsAsync()
        {
            AllAnimals.Clear();
            var animals = await _animalService.GetAllAsync();

            foreach (var animal in animals)
                AllAnimals.Add(animal);
        }

        private void OnEdit()
        {
            if (SelectedEvent != null)
            {
                foreach (var ev in Events)
                    ev.IsEditMode = false;

                SelectedEvent.IsEditMode = true;

                EditingEvent = new EventDto
                {
                    Id = SelectedEvent.Id,
                    Title = SelectedEvent.Title,
                    Description = SelectedEvent.Description,
                    Date = SelectedEvent.Date,
                    Type = SelectedEvent.Type,
                    AnimalIds = SelectedEvent.AnimalIds?.ToList() ?? new()
                };
                LoadSelectableAnimalsAsync();
                // Това е важното
                SelectedAnimals.Clear();
                foreach (var animal in AllAnimals)
                {
                    if (EditingEvent.AnimalIds.Contains(animal.Id))
                        SelectedAnimals.Add(animal);
                }


            }
        }

        private async Task OnDeleteAsync()
        {
            if (SelectedEvent == null) return;

            var result = MessageBox.Show("Сигурни ли сте, че искате да изтриете събитието?", "Изтриване", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _eventService.DeleteAsync(SelectedEvent.Id);
                await LoadEventsAsync();
                SelectedEvent = null;
                IsEditMode = false;
            }
        }

        private async Task OnSaveAsync()
        {
            if (EditingEvent == null) return;

            EditingEvent.AnimalIds = SelectableAnimals
                .Where(animal => animal.IsSelected)
                .Select(animal => animal.Animal.Id)
                .ToList();

            await _eventService.UpdateAsync(EditingEvent);
            await LoadEventsAsync();
            SelectedEvent = Events.FirstOrDefault(eventA => eventA.Id == EditingEvent.Id);
            IsEditMode = false;
            SelectedEvent.IsEditMode = false;

        }

       

        private async Task LoadSelectableAnimalsAsync()
        {
            SelectableAnimals.Clear();

            if (AllAnimals.Count == 0)
                await LoadAllAnimalsAsync();

            foreach (var animal in AllAnimals)
            {
                bool isSelected = EditingEvent.AnimalIds.Contains(animal.Id);
                SelectableAnimals.Add(new AnimalSelectableViewModel(animal, isSelected));
            }
        }
    }
}
