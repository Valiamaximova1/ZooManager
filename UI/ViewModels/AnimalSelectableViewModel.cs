using BusinessLayer.DTOs;
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
    }

}
