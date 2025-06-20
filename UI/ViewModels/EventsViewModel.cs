using BusinessLayer.DTOs;
using BusinessLayer.Events;
using BusinessLayer.Services.Interfaces;
using Data;
using Shared;
using Shared.Enums;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using UI.Commands;

namespace UI.ViewModels
{

    public class EventsViewModel : BaseViewModel
    {
        private readonly IEventService _eventService;
        private readonly IAnimalService _animalService;

        private bool _isPopupOpen;

        private bool _isClearingFilters = false;
        private bool _isRightClickSelection = false;
        //private DispatcherTimer _animalsReloadTimer;

        private string _selectedType = "Всички";
        private DateTime? _selectedDate = null;

        public Array EventTypeValues => Enum.GetValues(typeof(EventType));


        public ObservableCollection<EventDto> Events { get; } = new();

        public ObservableCollection<string> EventTypes { get; } =
        new ObservableCollection<string>(new[] { "Всички" }.Concat(Enum.GetNames(typeof(EventType))));

        public ObservableCollection<EventType> EditableEventTypes { get; } =
         new ObservableCollection<EventType>((EventType[])Enum.GetValues(typeof(EventType)));

        public List<EventDto> FilteredEvents { get; private set; } = new();

        public ObservableCollection<AnimalSelectableViewModel> SelectableAnimals { get; } = new();

        private ObservableCollection<AnimalDto> _allAnimals;
        private ObservableCollection<AnimalCheckboxDto> _animalFilters = new();

        public ObservableCollection<AnimalDto> SelectedAnimals { get; } = new();

        private EventDto _selectedEvent;
        private EventDto _editingEvent;

        private bool _isDetailsVisible;
        private bool _isEditMode;


        public EventsViewModel(IEventService eventService, IAnimalService animalService)
        {
            _eventService = eventService;
            _animalService = animalService;
            _animalService.AnimalChanged += async (sender, args) => await HandleAnimalChangeAsync(args);


            ClearCommand = new AsyncDelegateCommand(ClearFilters);
            EditEventCommand = new AsyncDelegateCommand(OnEdit);
            DeleteEventCommand = new AsyncDelegateCommand(OnDeleteAsync);
            SaveEventCommand = new AsyncDelegateCommand(OnSaveAsync);
            LoadEventsCommand = new AsyncDelegateCommand(LoadEventsAsync);
            ExportCommand = new AsyncDelegateCommand(onExport);
            ImportCommand = new AsyncDelegateCommand(OnImport);

            Task.Run(LoadAllAnimalsAsync);
            LoadEventsAsync();
        }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged();
            }
        }
        public string SelectedType
        {
            get => _selectedType;
            set
            {

                _selectedType = value;
                OnPropertyChanged();
                if (_isClearingFilters == false)
                {
                    LoadEventsAsync();
                }

            }
        }
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
                if (_isClearingFilters == false)
                {
                    LoadEventsAsync();
                }
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

                    // показвай детайли само ако не е от десен клик
                    IsDetailsVisible = !_isRightClickSelection && value != null;

                    OnPropertyChanged();

                    if (!_isRightClickSelection)
                        LoadSelectedAnimalsAsync();

                    _isRightClickSelection = false; // винаги нулираме след това
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

        public ObservableCollection<AnimalDto> AllAnimals
        {
            get => _allAnimals;
            set
            {
                _allAnimals = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AnimalCheckboxDto> AnimalFilters
        {
            get => _animalFilters;
            set
            {
                _animalFilters = value;
                OnPropertyChanged();
            }
        }



        public ICommand ClearCommand { get; }
        public ICommand EditEventCommand { get; }
        public ICommand DeleteEventCommand { get; }
        public ICommand SaveEventCommand { get; }
        public ICommand LoadEventsCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand ImportCommand { get; }


        private async Task LoadEventsAsync()
        {
            Events.Clear();

            var selectedAnimalIds = AnimalFilters
                .Where(x => x.IsSelected)
                .Select(x => x.Id)
                .ToList();

            var events = await _eventService.GetAllAsync();

            if (_selectedType != "Всички" && Enum.TryParse<EventType>(_selectedType, out var type))
            {
                events = events.Where(ev => ev.Type == type).ToList();
            }

            if (_selectedDate.HasValue)
            {
                events = events.Where(ev => ev.Date.Date == _selectedDate.Value.Date).ToList();
            }

            if (selectedAnimalIds.Any())
            {
                events = events.Where(ev => selectedAnimalIds.All(id => ev.AnimalIds.Contains(id))).ToList();
            }


            FilteredEvents = events.ToList();


            foreach (var ev in events)
                Events.Add(ev);
        }

        private async Task ClearFilters()
        {
            _isClearingFilters = true;

            SelectedType = "Всички";
            SelectedDate = null;

            foreach (var animal in AnimalFilters)
            {
                animal.IsSelected = false;
            }

            _isClearingFilters = false;

            await LoadEventsAsync();
        }


        private async Task LoadAllAnimalsAsync()
        {
            var animals = await _animalService.GetAllAsync();
            AllAnimals = new ObservableCollection<AnimalDto>(animals);

            var previouslySelected = AnimalFilters.Where(a => a.IsSelected).Select(a => a.Id).ToHashSet();
            var filters = new ObservableCollection<AnimalCheckboxDto>();

            foreach (var animal in animals)
            {
                var filterItem = new AnimalCheckboxDto
                {
                    Id = animal.Id,
                    Name = animal.Name
                };

                filterItem.SelectionChanged = async () =>
                {
                    if (!_isClearingFilters)
                        await LoadEventsAsync();
                };

                // след като е зададен SelectionChanged — сетваме IsSelected
                filterItem.IsSelected = previouslySelected.Contains(animal.Id);

                filters.Add(filterItem);
            }


            AnimalFilters = filters;
            //UpdateAnimalLookup();
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
            //EventDto.AnimalLookup = SelectedAnimals.ToDictionary(a => a.Id, a => a.Name);
        }

        private async Task OnEdit()
        {
            if (_selectedEvent != null)
            {
                foreach (var ev in Events)
                    ev.IsEditMode = false;

                _selectedEvent.IsEditMode = true;

                EditingEvent = new EventDto
                {
                    Id = _selectedEvent.Id,
                    Title = _selectedEvent.Title,
                    Description = _selectedEvent.Description,
                    Date = _selectedEvent.Date,
                    Type = _selectedEvent.Type,
                    AnimalIds = _selectedEvent.AnimalIds?.ToList() ?? new()
                };

                await LoadSelectableAnimalsAsync();
             
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
             .Where(a => a.IsSelected)
             .Select(a => a.Animal.Id)
             .ToList();


            await _eventService.UpdateAsync(EditingEvent);
            await LoadEventsAsync();
           
 SelectedEvent = Events.FirstOrDefault(eventA => eventA.Id == EditingEvent.Id);
            IsEditMode = false;
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


        private async Task onExport()
        {
            try
            {
                var saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Title = "Избери място за запис на Excel файл",
                    Filter = "Excel файлове (*.xlsx)|*.xlsx",
                    FileName = "EventsExport.xlsx"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    var exporter = new ExcelExporter();

                    //string startupPath = AppDomain.CurrentDomain.BaseDirectory;
                    //string solutionPath = Path.GetFullPath(Path.Combine(startupPath, @"..\..\..\.."));
                    //string filePath = Path.Combine(solutionPath, "EventsExport.xlsx");

                    await exporter.ExportEventsToExcel(FilteredEvents, saveDialog.FileName);

                    MessageBox.Show("Успешен експорт на събития!", "Excel", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Грешка при експортиране: {ex.Message}", "Грешка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task OnImport()
        {

            var openDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Избери XML файл за импортиране",
                Filter = "XML файлове (*.xml)|*.xml"
            };

            if (openDialog.ShowDialog() == true)
            {
                var importer = new XmlImporter();
                string result = await Task.Run(() => importer.ImportEventsFromXml(openDialog.FileName, FilteredEvents));

                if (result == "OK")
                {
                    LoadEventsAsync();
                    MessageBox.Show("Импортирането от XML завърши успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(result, "Грешка при импортиране", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }





            //string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            //string solutionPath = Path.GetFullPath(Path.Combine(startupPath, @"..\..\..\.."));
            //string xmlPath = Path.Combine(solutionPath, "EventsImport.xml");


        }

        private async Task HandleAnimalChangeAsync(AnimalChangedEventArgs args)
        {
            var animal = args.Animal;

            switch (args.ChangeType)
            {
                case AnimalChangeType.Added:
                    AllAnimals.Add(animal);
                    EventDto.AnimalLookup[animal.Id] = animal.Name;

                    AnimalFilters.Add(new AnimalCheckboxDto
                    {
                        Id = animal.Id,
                        Name = animal.Name,
                        IsSelected = false,
                        SelectionChanged = async () => await LoadEventsAsync()
                    });

                    SelectableAnimals.Add(new AnimalSelectableViewModel(animal, false));
                    break;

                case AnimalChangeType.Updated:
                    var index = AllAnimals.ToList().FindIndex(a => a.Id == animal.Id);
                    if (index >= 0)
                        AllAnimals[index] = animal;

                    EventDto.AnimalLookup[animal.Id] = animal.Name;

                    var filter = AnimalFilters.FirstOrDefault(f => f.Id == animal.Id);
                    if (filter != null)
                        filter.Name = animal.Name;

                    foreach (var selectable in SelectableAnimals.Where(s => s.Animal.Id == animal.Id))
                    {
                        selectable.Animal.Name = animal.Name;
                    }

                    break;

                case AnimalChangeType.Deleted:
                    // 1. Премахване от AllAnimals
                    var existing = AllAnimals.FirstOrDefault(a => a.Id == animal.Id);
                    if (existing != null)
                        AllAnimals.Remove(existing);

                    // 2. Премахване от AnimalLookup
                    EventDto.AnimalLookup.Remove(animal.Id);

                    // 3. Премахване от филтри
                    var deletedFilter = AnimalFilters.FirstOrDefault(f => f.Id == animal.Id);
                    if (deletedFilter != null)
                        AnimalFilters.Remove(deletedFilter);

                    // 4. Премахване от SelectableAnimals
                    var toRemove = SelectableAnimals
                        .Where(s => s.Animal.Id == animal.Id)
                        .ToList(); // избягваме модифициране по време на итерация

                    foreach (var s in toRemove)
                        SelectableAnimals.Remove(s);

                    foreach (var ev in Events)
                    {
                        int indexx = ev.AnimalIds.IndexOf(animal.Id);
                        if (indexx >= 0)
                        {
                            ev.AnimalIds.RemoveAt(indexx);
                            // не е нужно да триеш AnimalNames – те се пресмятат динамично от AnimalIds
                        }
                    }
                    break;


            }
        }






    }
}
