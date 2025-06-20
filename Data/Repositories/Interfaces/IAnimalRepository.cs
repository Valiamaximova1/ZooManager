using Shared.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IAnimalRepository
    {
        IQueryable<Animal> GetAll();
        Task<IEnumerable<Animal>> GetAllAsync();
        Task<IEnumerable<Animal>> GetByCategoryAsync(AnimalCategory category);
        Task<Animal> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
        Task AddAsync(Animal animal);
        Task DeleteAsync(Animal animal);
    }
}
