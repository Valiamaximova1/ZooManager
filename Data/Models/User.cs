

namespace Models
{

    public class User : BaseEntity
    {
        public string FirstName { get; set; } 

        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } 

    
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
