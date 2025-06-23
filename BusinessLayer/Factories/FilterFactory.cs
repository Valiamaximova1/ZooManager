using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Factories
{
    public static class FilterFactory
    {
        public static ObservableCollection<AnimalCheckboxDto> CreateAnimalFilters(
            IEnumerable<AnimalDto> animals,
            Action<Guid> onChanged)
        {
            var filters = new ObservableCollection<AnimalCheckboxDto>();

            foreach (var animal in animals)
            {
                filters.Add(new AnimalCheckboxDto
                {
                    Id = animal.Id,
                    Name = animal.Name,
                    SelectionChanged = () => onChanged(animal.Id)
                });
            }

            return filters;
        }
    }

}
