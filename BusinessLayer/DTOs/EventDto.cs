using Models;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public EventType Type { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        private List<Guid> _animalIds = new();
        public List<Guid> AnimalIds
        {
            get => _animalIds;
            set => _animalIds = value?.Distinct().ToList() ?? new();
        }

    }
}
