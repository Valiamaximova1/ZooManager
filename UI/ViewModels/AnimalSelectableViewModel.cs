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
        private bool _isSelected;
        public AnimalDto Animal { get; }

        public AnimalSelectableViewModel(AnimalDto animal, bool isSelected = false)
        {
            Animal = animal;
            _isSelected = isSelected;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

    }

}
