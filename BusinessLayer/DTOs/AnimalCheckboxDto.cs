using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class AnimalCheckboxDto : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));

                    // Извиква презареждане, ако има делегат
                    SelectionChanged?.Invoke();
                }
            }
        }
        public void UpdateFrom(AnimalDto animal)
        {
            if (animal == null || animal.Id != Id) return;

            Name = animal.Name + (string.IsNullOrWhiteSpace(animal.Name) ? " (изтрито)" : "");
        }

        public Action? SelectionChanged { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
