using BusinessLayer.DTOs;
using BusinessLayer.Services.Interfaces;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using UI.Commands;
using Windows.UI.Xaml.Media.Imaging;

namespace UI.ViewModels
{
    public class AnimalsViewModel : BaseViewModel
    {
        private string _selectedCategory = "Всички";
        private AnimalDto _selectedAnimal;
        private bool _isPopupOpen;

        private readonly IAnimalService _animalService;

        public ObservableCollection<AnimalDto> Animals { get; } = new();
        public ObservableCollection<string> Categories { get; } =
      new ObservableCollection<string>(new[] { "Всички" }.Concat(Enum.GetNames(typeof(AnimalCategory))));

        public ICommand SearchCommand { get; }
        public ICommand PlaySoundCommand { get; }
        public ICommand ShowAnimalDetailsCommand { get; }
        public ICommand ClosePopupCommand { get; }

        public AnimalsViewModel(IAnimalService animalService)
        {
            _animalService = animalService;

            SearchCommand = new AsyncDelegateCommand(LoadAnimalsAsync);
            PlaySoundCommand = new DelegateCommand<AnimalDto>(PlaySound);
            ShowAnimalDetailsCommand = new DelegateCommand<AnimalDto>(ShowAnimalDetails);
            ClosePopupCommand = new DelegateCommand(() => IsPopupOpen = false);

            LoadAnimalsAsync();
        }


        public AnimalDto SelectedAnimal
        {
            get => _selectedAnimal;
            set
            {
                _selectedAnimal = value;
                OnPropertyChanged();
            }
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
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        private async Task<ObservableCollection<AnimalDto>> LoadAnimalsAsync()
        {
            Animals.Clear();

            if (SelectedCategory.Equals("Всички"))
            {
                var all = await _animalService.GetAllAsync();
                foreach (var a in all) Animals.Add(a);
            }
            else
            {
                if (Enum.TryParse<AnimalCategory>(SelectedCategory, out var category))
                {
                    var filtered = await _animalService.GetByCategoryAsync(category);
                    foreach (var a in filtered) Animals.Add(a);
                }
            }

            return Animals;
        }

        private void PlaySound(AnimalDto animal)
        {


            if (animal.SoundPath is null || !File.Exists(animal.SoundPath))
            {
                MessageBox.Show("Няма звук за това животно.");
                return;
            }

            var player = new MediaPlayer();
            player.Open(new Uri(animal.SoundPath, UriKind.RelativeOrAbsolute));

            player.Play();
        }

        private void ShowAnimalDetails(AnimalDto animal)
        {
            SelectedAnimal = animal;
            IsPopupOpen = true;
        }
    }

}
