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
        private string _imagePath;
        private string _soundPath;

        private readonly IAnimalService _animalService;
        private readonly Action _goBack;

        public AnimalDto NewAnimal { get; set; } = new AnimalDto();
        public ObservableCollection<AnimalCategory> Categories { get; set; }


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

        public ICommand SaveCommand { get; }
        public ICommand BrowseImageCommand { get; }
        public ICommand BrowseSoundCommand { get; }
        public ICommand GoBackCommand { get; }

        private void BrowseImage()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog { Filter = "Images|*.jpg;*.png" };
            if (dialog.ShowDialog() != true)
                return;

            var fileName = Path.GetFileName(dialog.FileName);
            string relativeDir = Path.Combine("Assets", "Images");
            string relativePath = Path.Combine(relativeDir, fileName);

            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            string sourcePath = Path.Combine(projectRoot, relativePath);
            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            if (File.Exists(sourcePath) || File.Exists(outputPath))
            {
                string uniqueName = Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid() + Path.GetExtension(fileName);
                relativePath = Path.Combine(relativeDir, uniqueName);
                sourcePath = Path.Combine(projectRoot, relativePath);
                outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(sourcePath));
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            File.Copy(dialog.FileName, sourcePath, true);
            File.Copy(dialog.FileName, outputPath, true);

            _imagePath = relativePath;
            NewAnimal.ImagePath = _imagePath;
            OnPropertyChanged(nameof(NewAnimal));
        }

        private void BrowseSound()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog { Filter = "Audio|*.mp3;*.wav" };
            if (dialog.ShowDialog() != true)
                return;

            var fileName = Path.GetFileName(dialog.FileName);
            string relativeDir = Path.Combine("Assets", "Sounds");
            string relativePath = Path.Combine(relativeDir, fileName);

            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            string sourcePath = Path.Combine(projectRoot, relativePath);
            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            if (File.Exists(sourcePath) || File.Exists(outputPath))
            {
                string uniqueName = Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid() + Path.GetExtension(fileName);
                relativePath = Path.Combine(relativeDir, uniqueName);
                sourcePath = Path.Combine(projectRoot, relativePath);
                outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(sourcePath));
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            File.Copy(dialog.FileName, sourcePath, true);
            File.Copy(dialog.FileName, outputPath, true);

            _soundPath = relativePath;
            NewAnimal.SoundPath = _soundPath;
            OnPropertyChanged(nameof(NewAnimal));
        }

        private async Task SaveAsync()
        {
            await _animalService.CreateAsync(NewAnimal);
            _goBack?.Invoke();
        }
    }
}
