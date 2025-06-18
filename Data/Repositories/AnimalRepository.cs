using Shared.Enums;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly ZooDbContext _context;

        public AnimalRepository(ZooDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Animal>> GetAllAsync()
            => await _context.Animals.ToListAsync();

        public async Task<IEnumerable<Animal>> GetByCategoryAsync(AnimalCategory category)
            => await _context.Animals
                             .Where(animal => animal.Category == category)
                             .ToListAsync();

        public async Task<Animal> GetByIdAsync(Guid id)
        {
            return await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task AddAsync(Animal animal)
        {
            await _context.Animals.AddAsync(animal);
        }
        public async Task DeleteAsync(Animal animal)
        {
            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
        }
    }
}
