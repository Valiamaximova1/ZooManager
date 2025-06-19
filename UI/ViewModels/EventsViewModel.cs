using BusinessLayer.DTOs;
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

        public ObservableCollection<AnimalDto> AllAnimals { get; set; } = new();
        public ObservableCollection<AnimalCheckboxDto> AnimalFilters { get; } = new();

        public ObservableCollection<AnimalDto> SelectedAnimals { get; } = new();

        private EventDto _selectedEvent;
        private EventDto _editingEvent;

        private bool _isDetailsVisible;
        private bool _isEditMode;


        public EventsViewModel(IEventService eventService, IAnimalService animalService)
        {
            _eventService = eventService;
            _animalService = animalService;


            ClearCommand = new AsyncDelegateCommand(ClearFilters);
            EditEventCommand = new DelegateCommand(OnEdit);
            DeleteEventCommand = new AsyncDelegateCommand(OnDeleteAsync);
            SaveEventCommand = new AsyncDelegateCommand(OnSaveAsync);
            LoadEventsCommand = new AsyncDelegateCommand(LoadEventsAsync);
            ExportCommand = new AsyncDelegateCommand(onExport);
            ImportCommand = new AsyncDelegateCommand(OnImport);

            LoadAllAnimalsAsync();
            LoadEventsAsync();
            StartAnimalAutoReload(1000);
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

        public void SetRightClickSelection()
        {
            _isRightClickSelection = true;
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

            // Изключваме делегатите временно и махаме отметките
            foreach (var animal in AnimalFilters)
            {
                animal.SelectionChanged = null;
                animal.IsSelected = false;
            }

            // Възстановяваме делегатите, но не извикваме LoadEventsAsync вътре!
            foreach (var animal in AnimalFilters)
            {
                animal.SelectionChanged = async () =>
                {
                    if (!_isClearingFilters)
                        await LoadEventsAsync();
                };
            }

            _isClearingFilters = false;

            // Еднократно зареждане след пълното възстановяване на делегатите
            await LoadEventsAsync();
        }

        private async Task LoadAllAnimalsAsync()
        {
            var animals = await _animalService.GetAllAsync();

            AllAnimals = new ObservableCollection<AnimalDto>(animals);
            OnPropertyChanged(nameof(AllAnimals));

            // Обнови AnimalFilters, като запазиш избора
            var previouslySelected = AnimalFilters.Where(a => a.IsSelected).Select(a => a.Id).ToHashSet();
            AnimalFilters.Clear();

            foreach (var animal in animals)
            {
                var filterItem = new AnimalCheckboxDto
                {
                    Id = animal.Id,
                    Name = animal.Name,
                    IsSelected = previouslySelected.Contains(animal.Id)
                };

                filterItem.SelectionChanged = async () =>
                {
                    if (!_isClearingFilters)
                        await LoadEventsAsync();
                };

                AnimalFilters.Add(filterItem);
            }

            OnPropertyChanged(nameof(AnimalFilters));
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

        private void OnEdit()
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

                LoadSelectableAnimalsAsync();
                //    // Това е важното
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

        private DispatcherTimer _animalReloadTimer;

        public void StartAnimalAutoReload(int intervalMilliseconds = 3000)
        {
            _animalReloadTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(intervalMilliseconds)
            };

            _animalReloadTimer.Tick += async (s, e) =>
            {
                await LoadAllAnimalsAsync();
            };

            _animalReloadTimer.Start();
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


    }
}
