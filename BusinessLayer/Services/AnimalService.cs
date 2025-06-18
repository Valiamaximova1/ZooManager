using BusinessLayer.DTOs;
using BusinessLayer.Mappers;
using BusinessLayer.Services.Interfaces;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Models;
using Shared.Enums;
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

        public async Task UpdateAsync(AnimalDto animal)
        {
            var existingAnimal = await _repository.GetByIdAsync(animal.Id);
            if (existingAnimal != null)
            {
                existingAnimal.Name = animal.Name;
                existingAnimal.Description = animal.Description;
                existingAnimal.Category = animal.Category;
                existingAnimal.ImagePath = animal.ImagePath;
                existingAnimal.SoundPath = animal.SoundPath;

                await _repository.SaveChangesAsync(); 
            }
        }

        public async Task CreateAsync(AnimalDto animalDto)
        {
            var entity = animalDto.ToEntity(); // използваш вече съществуващия мапър
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }

    }

}
