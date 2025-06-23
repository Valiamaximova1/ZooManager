using BusinessLayer.DTOs;
using BusinessLayer.Services.Interfaces;
using BusinessLayer.DTOs;
using BusinessLayer.Services.Interfaces;
using Shared.Enums;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UI.Commands;

namespace UI.ViewModels
{
    public class AnimalEditViewModel : BaseViewModel
    {
        private string _newImagePath;
        private string _newSoundPath;

        private readonly IAnimalService _animalService;
        public ObservableCollection<AnimalCategory> Categories { get; set; }

        public AnimalDto EditingAnimal { get; set; }

        public event Action ClosePopupRequested;
        public event Action ReloadAnimalsRequested;

        public AnimalEditViewModel(IAnimalService animalService, AnimalDto animalToEdit)
        {
            _animalService = animalService;
            EditingAnimal = animalToEdit;

            Categories = new ObservableCollection<AnimalCategory>((AnimalCategory[])Enum.GetValues(typeof(AnimalCategory)));

            SaveCommand = new AsyncDelegateCommand(SaveAsync);
            CancelCommand = new DelegateCommand(ClosePopup);
            BrowseImageCommand = new DelegateCommand(OnBrowseImage);
            BrowseSoundCommand = new DelegateCommand(OnBrowseSound);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BrowseImageCommand { get; }
        public ICommand BrowseSoundCommand { get; }

        private async Task SaveAsync()
        {
            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\")); // корен на проекта

            if (!string.IsNullOrEmpty(_newImagePath))
            {
                var fileName = Path.GetFileName(_newImagePath);
                var destDir = Path.Combine(projectRoot, "Assets", "Images");
                Directory.CreateDirectory(destDir);
                var destPath = Path.Combine(destDir, fileName);
                File.Copy(_newImagePath, destPath, true);
                EditingAnimal.ImagePath = Path.Combine("Images", fileName);
            }

            if (!string.IsNullOrEmpty(_newSoundPath))
            {
                var fileName = Path.GetFileName(_newSoundPath);
                var destDir = Path.Combine(projectRoot, "Assets", "Sounds");
                Directory.CreateDirectory(destDir);
                var destPath = Path.Combine(destDir, fileName);
                File.Copy(_newSoundPath, destPath, true);
                EditingAnimal.SoundPath = Path.Combine("Sounds", fileName);
            }

            await _animalService.UpdateAsync(EditingAnimal);
            ReloadAnimalsRequested?.Invoke();
            ClosePopupRequested?.Invoke();
        }

        private void OnBrowseImage()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() != true)
                return;

            if (!string.IsNullOrWhiteSpace(EditingAnimal.ImagePath))
            {
                string realImgPath = Path.Combine("Assets", EditingAnimal.ImagePath);
                string oldRelativePath = realImgPath.Replace('/', '\\');
                string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
                string oldProjectPath = Path.Combine(projectRoot, oldRelativePath);
                string oldBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, oldRelativePath);

                try
                {
                    if (File.Exists(oldProjectPath))
                        File.Delete(oldProjectPath);
                    if (File.Exists(oldBinPath))
                        File.Delete(oldBinPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Грешка при изтриване на старата снимка:\n{ex.Message}", "Грешка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            string fileName = Path.GetFileName(dialog.FileName);
            string relativePath = Path.Combine("Assets", "Images", fileName);
            string databasePath = Path.Combine("Images", fileName);
            string newProjectPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..", relativePath));
            string newBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            if (File.Exists(newProjectPath) || File.Exists(newBinPath))
            {
                string uniqueName = Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid() + Path.GetExtension(fileName);
                relativePath = Path.Combine("Assets", "Images", uniqueName);
                databasePath = Path.Combine("Images", uniqueName);
                newProjectPath = Path.Combine(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..")), relativePath);
                newBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(newProjectPath)!);
            Directory.CreateDirectory(Path.GetDirectoryName(newBinPath)!);

            File.Copy(dialog.FileName, newProjectPath, true);
            File.Copy(dialog.FileName, newBinPath, true);

            EditingAnimal.ImagePath = databasePath;
            _newImagePath = newProjectPath;

            OnPropertyChanged(nameof(EditingAnimal));
            SaveImmediatelyAsync();
        }

        private void OnBrowseSound()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Audio files (*.mp3; *.wav)|*.mp3;*.wav"
            };

            if (dialog.ShowDialog() == true)
            {
                var fileName = Path.GetFileName(dialog.FileName);
                string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
                var destDir = Path.Combine(projectRoot, "Assets", "Sounds");
                Directory.CreateDirectory(destDir);

                var destPath = Path.Combine(destDir, fileName);
                File.Copy(dialog.FileName, destPath, true);

                EditingAnimal.SoundPath = Path.Combine("Sounds", fileName);
                _newSoundPath = destPath;

                OnPropertyChanged(nameof(EditingAnimal));


                SaveImmediatelyAsync();
            }
        }

        private async Task SaveImmediatelyAsync()
        {
            try
            {
                await _animalService.UpdateAsync(EditingAnimal);
                ReloadAnimalsRequested?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Грешка при автоматично запазване: {ex.Message}");
            }
        }

        private void ClosePopup()
        {
            ClosePopupRequested?.Invoke();
        }
    }
}
