using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;


namespace Data.Repositories
{

    public class UserRepository : IUserRepository
    {
        private readonly ZooDbContext _context;

        public UserRepository(ZooDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

    }
}
