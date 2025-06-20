using BusinessLayer.DTOs;
using BusinessLayer.Services.Interfaces;
using Shared.Enums;

using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using UI.Commands;
using UI.Helpers;

namespace UI.ViewModels
{
    public class AnimalsViewModel : BaseViewModel
    {
        private readonly IAnimalService _animalService;

        private AnimalEditViewModel _animalEditViewModel;

        public ObservableCollection<AnimalDto> Animals { get; } = new();
        public ObservableCollection<string> Categories { get; } =
            new ObservableCollection<string>(new[] { "Всички" }.Concat(Enum.GetNames(typeof(AnimalCategory))));

        private AnimalDto _selectedAnimal;
        private bool _isPopupOpen;
        private bool _isEditPopupOpen;
        private BaseViewModel _editViewModel;

        public event Action AddAnimalRequested;

        public AnimalsViewModel(IAnimalService animalService)
        {
            _animalService = animalService;

            SearchCommand = new AsyncDelegateCommand(LoadAnimalsAsync);
            PlaySoundCommand = new DelegateCommand<AnimalDto>(PlaySound);
            ShowAnimalDetailsCommand = new DelegateCommand<AnimalDto>(ShowAnimalDetails);
            ClosePopupCommand = new DelegateCommand(() => IsPopupOpen = false);

            EditAnimalCommand = new DelegateCommand<AnimalDto>(ShowEditPopup);

            OpenAddAnimalCommand = new DelegateCommand(() => AddAnimalRequested?.Invoke());
            DeleteAnimalCommand = new DelegateCommand<AnimalDto>(async (animal) => await OnDeleteAnimalAsync(animal));

            CloseEditPopupCommand = new DelegateCommand(() => IsEditPopupOpen = false);

            LoadAnimalsAsync();
        }

        public ICommand SearchCommand { get; }
        public ICommand PlaySoundCommand { get; }
        public ICommand ShowAnimalDetailsCommand { get; }
        public ICommand ClosePopupCommand { get; }
        public ICommand EditAnimalCommand { get; }
        public ICommand AddAnimalPageCommand { get; }
        public ICommand OpenAddAnimalCommand { get; }
        public ICommand CloseEditPopupCommand { get; }
        public ICommand DeleteAnimalCommand { get; }

        public AnimalDto SelectedAnimal
        {
            get => _selectedAnimal;
            set { _selectedAnimal = value; OnPropertyChanged(); }
        }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set { _isPopupOpen = value; OnPropertyChanged(); }
        }
        public string SelectedCategory { get; set; } = "Всички";

        public bool IsEditPopupOpen
        {
            get => _isEditPopupOpen;
            set
            {
                _isEditPopupOpen = value;
                OnPropertyChanged();
            }
        }
        public BaseViewModel EditViewModel
        {
            get => _editViewModel;
            set
            {
                _editViewModel = value;
                OnPropertyChanged();
            }
        }
        public AnimalEditViewModel AnimalEditViewModel
        {
            get => _animalEditViewModel;
            set
            {
                _animalEditViewModel = value;
                OnPropertyChanged();
            }
        }
       

        private async Task OnDeleteAnimalAsync(AnimalDto animal)
        {
            if (animal == null) return;

            var result = MessageBox.Show(
                $"Сигурни ли сте, че искате да изтриете {animal.Name}?",
                "Изтриване",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            if (!string.IsNullOrWhiteSpace(animal.ImagePath))
                ImageHelper.DeleteFileFromAllLocations(animal.ImagePath, animal.FullImagePath);

            if (!string.IsNullOrWhiteSpace(animal.SoundPath))
                ImageHelper.DeleteFileFromAllLocations(animal.SoundPath, animal.SoundPath);

            await _animalService.DeleteAsync(animal.Id);
            Animals.Remove(animal);
        }

        public async Task<ObservableCollection<AnimalDto>> LoadAnimalsAsync()
        {
            Animals.Clear();

            if (SelectedCategory == "Всички")
            {
                var all = await _animalService.GetAllAsync();
                foreach (var animal in all)
                    Animals.Add(animal);
            }
            else if (Enum.TryParse<AnimalCategory>(SelectedCategory, out var category))
            {
                var filtered = await _animalService.GetByCategoryAsync(category);
                foreach (var animal in filtered)
                    Animals.Add(animal);
            }

            return Animals;
        }

        private void PlaySound(AnimalDto animal)
        {
            try
            {
                var soundFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", animal.SoundPath);

                if (!File.Exists(soundFullPath))
                {
                    MessageBox.Show("Няма звук за това животно.");
                    return;
                }

                var player = new MediaPlayer();
                player.Open(new Uri(soundFullPath, UriKind.Absolute));
                player.Play();
            }
            catch (Exception)
            {
                MessageBox.Show("Няма звук за това животно.");

            }


        }

        private void ShowAnimalDetails(AnimalDto animal)
        {
            SelectedAnimal = animal;
            IsPopupOpen = true;
        }

        public void ShowEditPopup(AnimalDto animal)
        {
            if (IsEditPopupOpen) return; // вече е отворен

            var editViewModel = new AnimalEditViewModel(_animalService, animal);
            editViewModel.ClosePopupRequested += () => IsEditPopupOpen = false;
            editViewModel.ReloadAnimalsRequested += async () => await LoadAnimalsAsync();
            EditViewModel = editViewModel;
            IsEditPopupOpen = true;
        }



        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T t)
                    yield return t;

                foreach (var childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }

     
    }


}
