using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class AnimalCheckboxDto : BaseViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
                SelectionChanged?.Invoke();
            }
        }

        public Action SelectionChanged { get; set; } 
    }
}
