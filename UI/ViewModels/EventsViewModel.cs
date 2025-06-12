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

        public ObservableCollection<EventDto> Events { get; } = new();
        public ObservableCollection<AnimalDto> AllAnimals { get; } = new();
        public ObservableCollection<AnimalDto> SelectedAnimals { get; } = new();

        private EventDto _selectedEvent;
        private EventDto _editingEvent;

        private bool _isDetailsVisible;
        private bool _isEditMode;


        public ICommand EditEventCommand { get; }
        public ICommand DeleteEventCommand { get; }
        public ICommand SaveEventCommand { get; }
        public ICommand LoadEventsCommand { get; }
        public ICommand KeyDownCommand { get; }

        public EventsViewModel(IEventService eventService, IAnimalService animalService)
        {
            _eventService = eventService;
            _animalService = animalService;

            EditEventCommand = new DelegateCommand(OnEdit);
            DeleteEventCommand = new AsyncDelegateCommand(OnDeleteAsync);
            SaveEventCommand = new AsyncDelegateCommand(OnSaveAsync);
            LoadEventsCommand = new AsyncDelegateCommand(LoadEventsAsync);
            KeyDownCommand = new DelegateCommand<KeyEventArgs>(OnKeyDown);

            _ = LoadEventsAsync();
            _ = LoadAllAnimalsAsync();
        }
        public EventDto SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                OnPropertyChanged();
                IsDetailsVisible = value != null;
                LoadSelectedAnimals();
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

        private async Task LoadEventsAsync()
        {
            Events.Clear();
            var all = await _eventService.GetAllAsync();
            foreach (var e in all)
                Events.Add(e);
        }

        private async Task LoadAllAnimalsAsync()
        {
            AllAnimals.Clear();
            var animals = await _animalService.GetAllAsync();
            foreach (var animal in animals)
                AllAnimals.Add(animal);
        }

        private void LoadSelectedAnimals()
        {
            SelectedAnimals.Clear();
            if (SelectedEvent?.AnimalIds != null)
            {
                foreach (var animal in AllAnimals.Where(a => SelectedEvent.AnimalIds.Contains(a.Id)))
                {
                    SelectedAnimals.Add(animal);
                }
            }
        }

        private void OnEdit()
        {
            if (SelectedEvent != null)
            {
                EditingEvent = new EventDto
                {
                    Id = SelectedEvent.Id,
                    Title = SelectedEvent.Title,
                    Description = SelectedEvent.Description,
                    Date = SelectedEvent.Date,
                    Type = SelectedEvent.Type,
                    AnimalIds = SelectedEvent.AnimalIds?.ToList() ?? new()
                };
                IsEditMode = true;
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

            await _eventService.UpdateAsync(EditingEvent);
            await LoadEventsAsync();
            SelectedEvent = null;
            IsEditMode = false;
        }

        private void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Delete && SelectedEvent != null)
            {
                _ = OnDeleteAsync();
            }
        }
    }
































    //public class EventsViewModel : BaseViewModel
    //{
    //    private readonly IEventService _eventService;
    //    private EventDto _selectedEvent;
    //    private string _selectedType = "Всички";
    //    private DateTime? _selectedDate = null;
    //    private bool _isPopupOpen;

    //    public ObservableCollection<EventDto> Events { get; } = new();

    //    public ObservableCollection<string> EventTypes { get; } =
    //    new ObservableCollection<string>(new[] { "Всички" }.Concat(Enum.GetNames(typeof(EventType))));

    //    public ICommand SearchCommand { get; }
    //    public ICommand ShowEventDetailsCommand { get; }
    //    public ICommand DeleteEventCommand { get; }
    //    public ICommand ClosePopupCommand { get; }


    //    public EventsViewModel(IEventService eventService)
    //    {
    //        _eventService = eventService;

    //        SearchCommand = new AsyncDelegateCommand(LoadEventsAsync);
    //        //ShowEventDetailsCommand = new DelegateCommand<EventDto>(ShowEventDetails);
    //        DeleteEventCommand = new AsyncDelegateCommand(DeleteEventAsync);
    //        ClosePopupCommand = new DelegateCommand(() => IsPopupOpen = false);

    //        _ = LoadEventsAsync();
    //    }
    //    public string SelectedType
    //    {
    //        get => _selectedType;
    //        set
    //        {

    //            _selectedType = value;
    //            OnPropertyChanged();
    //        }
    //    }
    //    public DateTime? SelectedDate
    //    {
    //        get => _selectedDate;
    //        set
    //        {
    //            _selectedDate = value;
    //            OnPropertyChanged();
    //        }
    //    }
    //    public bool IsPopupOpen
    //    {
    //        get => _isPopupOpen;
    //        set
    //        {
    //            _isPopupOpen = value;
    //            OnPropertyChanged();
    //        }
    //    }
    //    public EventDto SelectedEvent
    //    {
    //        get => _selectedEvent;
    //        set
    //        {
    //            _selectedEvent = value;
    //            OnPropertyChanged();
    //        }
    //    }


    //    private async Task LoadEventsAsync()
    //    {

    //        Events.Clear();
    //        if (SelectedType.Equals("Всички"))
    //        {
    //            var all = await _eventService.GetAllAsync();
    //            foreach (var a in all)
    //                Events.Add(a);
    //            SelectedDate = null;
    //        }
    //        else
    //        {
    //            if (Enum.TryParse<EventType>(SelectedType, out var type))
    //            {

    //                var dateToUse = SelectedDate ?? DateTime.Today;
    //                var results = await _eventService.GetFilteredAsync(type, SelectedDate);

    //                foreach (var ev in results)
    //                    Events.Add(ev);
    //            }

    //        }


    //    }

    //    private async Task DeleteEventAsync()
    //    {
    //        if (SelectedEvent == null) return;

    //        var result = MessageBox.Show("Сигурни ли сте, че искате да изтриете това събитие?", "Изтриване", MessageBoxButton.YesNo);
    //        if (result == MessageBoxResult.Yes)
    //        {
    //            await _eventService.DeleteAsync(SelectedEvent.Id);
    //            await LoadEventsAsync();
    //        }
    //    }

    //    //private void ShowEventDetails(Events eventt)
    //    //{
    //    //    //SelectedAnimal = animal;
    //    //    //IsPopupOpen = true;
    //    //}

    //    }

}
