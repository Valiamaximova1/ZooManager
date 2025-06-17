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
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
             _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateAsync(Event ev)
        //{
        //    _context.Events.Update(ev);
        //    await _context.SaveChangesAsync();
        //}

    }
}
