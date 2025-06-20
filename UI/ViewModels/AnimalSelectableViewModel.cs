using BusinessLayer.DTOs;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ViewModels
{
    public class AnimalSelectableViewModel : BaseViewModel
    {
        public AnimalDto Animal { get; }
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public AnimalSelectableViewModel(AnimalDto animal, bool isSelected = false)
        {
            Animal = animal;
            _isSelected = isSelected;
        }
        public void UpdateFrom(AnimalDto animal)
        {
            if (animal == null || animal.Id != Animal.Id) return;

            Animal.Name = animal.Name + (string.IsNullOrWhiteSpace(animal.Name) ? " (изтрито)" : "");
            OnPropertyChanged(nameof(Name));
        }

    }

}
