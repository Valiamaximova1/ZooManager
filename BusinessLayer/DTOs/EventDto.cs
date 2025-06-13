
using Models;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EventDto : INotifyPropertyChanged
    {
        public Guid Id { get; set; }

        private string _title;

        private string _description;

        private DateTime _date;

        private EventType _type; 
        
        private bool _isEditMode;

        private List<Guid> _animalIds = new();


        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public DateTime Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }


        public Shared.Enums.EventType Type
        {
            get => _type;
            set { _type = value; OnPropertyChanged(); }
        }


        public List<Guid> AnimalIds
        {
            get => _animalIds;
            set { _animalIds = value; OnPropertyChanged(); }
        }


        public bool IsEditMode
        {
            get => _isEditMode;
            set { _isEditMode = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}

