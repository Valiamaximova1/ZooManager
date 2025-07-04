﻿using BusinessLayer.DTOs;
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

        private string _selectedCategory = "Всички";
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

            PlaySoundCommand = new DelegateCommand<AnimalDto>(PlaySound);
            ShowAnimalDetailsCommand = new DelegateCommand<AnimalDto>(ShowAnimalDetails);
            ClosePopupCommand = new DelegateCommand(() => IsPopupOpen = false);
            EditAnimalCommand = new DelegateCommand<AnimalDto>(ShowEditPopup);
            OpenAddAnimalCommand = new DelegateCommand(() => AddAnimalRequested?.Invoke());
            DeleteAnimalCommand = new DelegateCommand<AnimalDto>(async (animal) => await OnDeleteAnimalAsync(animal));
            CloseEditPopupCommand = new DelegateCommand(() => IsEditPopupOpen = false);

            LoadAnimalsAsync();
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged();
                    LoadAnimalsAsync(); 
                }
            }
        }

        
        public ICommand PlaySoundCommand { get; }
        public ICommand ShowAnimalDetailsCommand { get; }
        public ICommand ClosePopupCommand { get; }
        public ICommand EditAnimalCommand { get; }
        public ICommand AddAnimalPageCommand { get; }
        public ICommand OpenAddAnimalCommand { get; }
        public ICommand CloseEditPopupCommand { get; }
        public ICommand DeleteAnimalCommand { get; }
        public ICommand ClearFilterCommand => new DelegateCommand(() =>
        {
            SelectedCategory = "Всички";
        });

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
        //public string SelectedCategory { get; set; } = "Всички";
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
            if (IsEditPopupOpen) return;

            var editViewModel = new AnimalEditViewModel(_animalService, animal);
            editViewModel.ClosePopupRequested += () => IsEditPopupOpen = false;
            editViewModel.ReloadAnimalsRequested += async () => await LoadAnimalsAsync();
            EditViewModel = editViewModel;
            IsEditPopupOpen = true;
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
    }


}
