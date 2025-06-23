using BusinessLayer.DTOs;
using BusinessLayer.Factories.Interfaces;
using DocumentFormat.OpenXml.Presentation;
using Models;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Factories
{
    public class AnimalFactory : IFactory<AnimalDto>
    {
        public AnimalDto Create()
        {
            return new AnimalDto
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Category = AnimalCategory.Неопределено,
                ImagePath = string.Empty,
                SoundPath = string.Empty,
                Description = string.Empty
            };
        }
    }

}


