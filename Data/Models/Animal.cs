using Shared.Enums;

namespace Models
{

    public class Animal : BaseEntity
    {
        public string Name { get; set; } 
        public AnimalCategory Category { get; set; }  
        public string Description { get; set; } 
        public string ImagePath { get; set; }
        public string SoundPath { get; set; } 
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
