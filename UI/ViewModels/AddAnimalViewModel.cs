using BusinessLayer.DTOs;
using BusinessLayer.Services.Interfaces;
using Shared.Enums;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Commands;

namespace UI.ViewModels
{
    public class AddAnimalViewModel : BaseViewModel
    {
        private readonly IAnimalService _animalService;
        private readonly Action _goBack;

        public AnimalDto NewAnimal { get; set; } = new AnimalDto();
        public ObservableCollection<AnimalCategory> Categories { get; set; }

        private string _imagePath;
        private string _soundPath;

        public ICommand SaveCommand { get; }
        public ICommand BrowseImageCommand { get; }
        public ICommand BrowseSoundCommand { get; }
        public ICommand GoBackCommand { get; }

        public AddAnimalViewModel(IAnimalService animalService, Action goBack)
        {
            _animalService = animalService;
            _goBack = goBack;

            Categories = new ObservableCollection<AnimalCategory>((AnimalCategory[])Enum.GetValues(typeof(AnimalCategory)));

            SaveCommand = new AsyncDelegateCommand(SaveAsync);
            BrowseImageCommand = new DelegateCommand(BrowseImage);
            BrowseSoundCommand = new DelegateCommand(BrowseSound);
            GoBackCommand = new DelegateCommand(() => _goBack?.Invoke());
        }

        private void BrowseImage()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog { Filter = "Images|*.jpg;*.png" };
            if (dialog.ShowDialog() == true)
            {
                var fileName = Path.GetFileName(dialog.FileName);
                string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
                string destDir = Path.Combine(projectRoot, "Assets", "Images");
                Directory.CreateDirectory(destDir);

                string destPath = Path.Combine(destDir, fileName);
                File.Copy(dialog.FileName, destPath, true);

                _imagePath = Path.Combine("Images", fileName);
                NewAnimal.ImagePath = _imagePath;
                OnPropertyChanged(nameof(NewAnimal));
            }
        }

        private void BrowseSound()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog { Filter = "Audio|*.mp3;*.wav" };
            if (dialog.ShowDialog() == true)
            {
                var fileName = Path.GetFileName(dialog.FileName);
                string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
                string destDir = Path.Combine(projectRoot, "Assets", "Sounds");
                Directory.CreateDirectory(destDir);

                string destPath = Path.Combine(destDir, fileName);
                File.Copy(dialog.FileName, destPath, true);

                _soundPath = Path.Combine("Sounds", fileName);
                NewAnimal.SoundPath = _soundPath;
                OnPropertyChanged(nameof(NewAnimal));
            }
        }

        private async Task SaveAsync()
        {
            await _animalService.CreateAsync(NewAnimal);
            _goBack?.Invoke();
        }
    }
}
