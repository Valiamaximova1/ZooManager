using BusinessLayer.DTOs;
using BusinessLayer.Services.Interfaces;
using CommunityToolkit.Mvvm.Input;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UI.Commands;

namespace UI.ViewModels
{
    public class TicketsViewModel : BaseViewModel
    {
        private readonly ITicketService _ticketService;
        private readonly Guid _currentUserId;

        public ObservableCollection<TicketTemplateDto> AllTemplates { get; set; } = new();
        public ObservableCollection<TicketTemplateDto> FilteredTemplates { get; set; } = new();
        public ObservableCollection<TicketSelection> Selections { get; set; } = new();

        public ObservableCollection<TicketType> TicketTypes { get; set; } =
            new(Enum.GetValues(typeof(TicketType)).Cast<TicketType>());

        private TicketType? _selectedType;
        public TicketType? SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public ICommand PurchaseCommand { get; }
        public ICommand ClearFilterCommand { get; }

        public TicketsViewModel(ITicketService ticketService, Guid currentUserId)
        {
            _ticketService = ticketService;
            _currentUserId = currentUserId;

            PurchaseCommand = new AsyncRelayCommand(PurchaseTicketsAsync);
            ClearFilterCommand = new RelayCommand(() => SelectedType = null);

            LoadTemplates();
        }

        private async void LoadTemplates()
        {
            var templates = await _ticketService.GetAllTemplatesAsync();
            AllTemplates = new ObservableCollection<TicketTemplateDto>(templates);
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var filtered = SelectedType.HasValue
                ? AllTemplates.Where(t => t.Type == SelectedType.Value)
                : AllTemplates;

            FilteredTemplates = new ObservableCollection<TicketTemplateDto>(filtered);
            OnPropertyChanged(nameof(FilteredTemplates));

            Selections = new ObservableCollection<TicketSelection>(
                FilteredTemplates.Select(t => new TicketSelection { Template = t, Quantity = 0 }));

            OnPropertyChanged(nameof(Selections));
        }

        private async Task PurchaseTicketsAsync()
        {
            try
            {
                foreach (var selection in Selections.Where(s => s.Quantity > 0))
                {
                    await _ticketService.PurchaseTicketAsync(
                        _currentUserId,
                        selection.Template.Id,
                        selection.Quantity
                    );
                }

                MessageBox.Show("Успешна покупка!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadTemplates(); // обнови наличностите
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show( "Недостатъчна наличност");
            }
            catch (Exception)
            {
                MessageBox.Show("Възникна неочаквана грешка.", "Грешка");
            }
        
        }
    }

    public class TicketSelection
    {
        public TicketTemplateDto Template { get; set; }
        public int Quantity { get; set; }
    }

}
