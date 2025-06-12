using BusinessLayer.DTOs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mappers
{
    public static class AnimalMapper
    {
        public static AnimalDto ToDto(this Animal animal)
        {
            return new AnimalDto
            {
                Id = animal.Id,
                Name = animal.Name,
                Description = animal.Description,
                ImagePath = animal.ImagePath,
                SoundPath = $"Assets/{animal.SoundPath}",
                Category = animal.Category
            };
        }

        public static Animal ToEntity(this AnimalDto dto)
        {
            return new Animal
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                ImagePath = dto.ImagePath,
                SoundPath = dto.SoundPath,
                Category = dto.Category
            };
        }
    }

}
