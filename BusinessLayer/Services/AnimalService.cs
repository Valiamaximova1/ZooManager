using BusinessLayer.DTOs;
using BusinessLayer.Mappers;
using BusinessLayer.Services.Interfaces;
using Shared.Enums;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _repository;

        public AnimalService(IAnimalRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AnimalDto>> GetAllAsync()
        {
            var animals = await _repository.GetAllAsync();
            return animals.Select(animal => animal.ToDto());
        }

        public async Task<IEnumerable<AnimalDto>> GetByCategoryAsync(AnimalCategory category)
        {
            var animals = await _repository.GetByCategoryAsync(category);
            return animals.Select(animal => animal.ToDto());
        }

        public async Task<AnimalDto> GetByIdAsync(Guid id)
        {
            var animal = await _repository.GetByIdAsync(id);
            return animal.ToDto();
        }
    }

}
