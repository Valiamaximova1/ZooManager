using System.ComponentModel;

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
                    SelectionChanged?.Invoke();
                }
            }
        }

        public Action SelectionChanged { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
