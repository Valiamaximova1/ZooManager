using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Helpers
{
    public class TicketSelection : INotifyPropertyChanged
    {
        public TicketTemplateDto Template { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
                    QuantityChanged?.Invoke(); 
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action QuantityChanged; 
    }

}
