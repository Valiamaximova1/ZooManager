using BusinessLayer.DTOs;
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
        Task<IEnumerable<AnimalDto>> GetAllAsync();
        Task<IEnumerable<AnimalDto>> GetByCategoryAsync(AnimalCategory category);
        Task<AnimalDto> GetByIdAsync(Guid id);
    }

}
