using BusinessLayer.DTOs;
using BusinessLayer.Events;
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

        public event EventHandler<AnimalChangedEventArgs> AnimalChanged;

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

        public async Task UpdateAsync(AnimalDto animalDto)
        {
            var existingAnimal = await _repository.GetByIdAsync(animalDto.Id);
            if (existingAnimal != null)
            {
                existingAnimal.Name = animalDto.Name;
                existingAnimal.Description = animalDto.Description;
                existingAnimal.Category = animalDto.Category;
                existingAnimal.ImagePath = animalDto.ImagePath;
                existingAnimal.SoundPath = animalDto.SoundPath;

                await _repository.SaveChangesAsync();
                AnimalChanged?.Invoke(this, new AnimalChangedEventArgs(animalDto, AnimalChangeType.Updated));
            }
        }

        public async Task CreateAsync(AnimalDto animalDto)
        {
            var entity = animalDto.ToEntity(); 
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            AnimalChanged?.Invoke(this, new AnimalChangedEventArgs(animalDto, AnimalChangeType.Added));
        }
        public async Task DeleteAsync(Guid id)
        {
            var animal = await _repository.GetByIdAsync(id);
            if (animal != null)
            {
                var animalDto = animal.ToDto();
                await _repository.DeleteAsync(animal);
                AnimalChanged?.Invoke(this, new AnimalChangedEventArgs(animalDto, AnimalChangeType.Deleted));
            }
        }
    }

}
