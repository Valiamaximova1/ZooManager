using Shared.Enums;


namespace Models
{
    public class Event : BaseEntity
    {
        public string Title { get; set; } 
        public EventType Type { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }


        public ICollection<Animal> Animals { get; set; } = new List<Animal>();


        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
