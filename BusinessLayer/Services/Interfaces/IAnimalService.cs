using BusinessLayer.DTOs;
using BusinessLayer.Events;
using Models;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface IAnimalService
    {
        event EventHandler<AnimalChangedEventArgs> AnimalChanged;
        Task<IEnumerable<AnimalDto>> GetAllAsync();
        Task<IEnumerable<AnimalDto>> GetByCategoryAsync(AnimalCategory category);
        Task<AnimalDto> GetByIdAsync(Guid id);
        Task UpdateAsync(AnimalDto animal);
        Task CreateAsync(AnimalDto animalDto);
        Task DeleteAsync(Guid id);
    }

}
