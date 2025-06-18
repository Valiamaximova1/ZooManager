using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;


namespace BusinessLayer.DTOs
{
    public class AnimalDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string SoundPath { get; set; }
        public AnimalCategory Category { get; set; }

        public bool isCheckedAnimal;
        public string FullImagePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", ImagePath);
            }
        }
    }
}

