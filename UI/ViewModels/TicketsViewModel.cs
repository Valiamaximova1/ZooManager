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
using UI.Helpers;

namespace UI.ViewModels
{
    public class TicketsViewModel : BaseViewModel
    {
        private readonly ITicketService _ticketService;
        private readonly Guid _currentUserId;
        private TicketType? _selectedType;

        public ObservableCollection<TicketTemplateDto> AllTemplates { get; set; } = new();
        public ObservableCollection<TicketTemplateDto> FilteredTemplates { get; set; } = new();
        public ObservableCollection<TicketSelection> Selections { get; set; } = new();

        public ObservableCollection<TicketType> TicketTypes { get; set; } =
            new(Enum.GetValues(typeof(TicketType)).Cast<TicketType>());

        public AsyncDelegateCommand PurchaseCommand { get; }
        public DelegateCommand ClearFilterCommand { get; }

        public TicketsViewModel(ITicketService ticketService, Guid currentUserId)
        {
            _ticketService = ticketService;
            _currentUserId = currentUserId;

            PurchaseCommand = new AsyncDelegateCommand(PurchaseTicketsAsync, CanPurchase);
            ClearFilterCommand = new DelegateCommand(() => SelectedType = null);

            LoadTemplates();
        }

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

        private async void LoadTemplates()
        {
            var templates = await _ticketService.GetAllTemplatesAsync();
            AllTemplates = new ObservableCollection<TicketTemplateDto>(templates);
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var filteredTickets = SelectedType.HasValue
                ? AllTemplates.Where(ticket => ticket.Type == SelectedType.Value)
                : AllTemplates;

            FilteredTemplates = new ObservableCollection<TicketTemplateDto>(filteredTickets);
            OnPropertyChanged(nameof(FilteredTemplates));

            Selections = new ObservableCollection<TicketSelection>(
                FilteredTemplates.Select(t => new TicketSelection { Template = t, Quantity = 0 }));

            foreach (var selection in Selections)
            {
                selection.QuantityChanged += () =>
                {
                    (PurchaseCommand as AsyncDelegateCommand)?.RaiseCanExecuteChanged();
                };
            }

            OnPropertyChanged(nameof(Selections));
            PurchaseCommand.RaiseCanExecuteChanged();
        }

        private bool CanPurchase()
        {
            return Selections.Any(selectElement => selectElement.Quantity > 0);
        }

        private async Task PurchaseTicketsAsync()
        {
            try
            {
                var invalidSelections = Selections
                   .Where(s =>
                       s.Quantity < 0 ||
                       s.Quantity > s.Template.AvailableQuantity
                   )
                   .ToList();

                if (invalidSelections.Any())
                {
                    foreach (var s in Selections)
                        s.Quantity = 0;

                    throw new InvalidOperationException();
                }
                foreach (var selection in Selections.Where(s => s.Quantity > 0))
                {
                    await _ticketService.PurchaseTicketAsync(
                        _currentUserId,
                        selection.Template.Id,
                        selection.Quantity
                    );
                }

                MessageBox.Show("Успешна покупка!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadTemplates();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Невалидно количество",
                    "Грешка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show("Възникна неочаквана грешка.", "Грешка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

}
