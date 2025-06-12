using BusinessLayer.DTOs;
using BusinessLayer.Mappers;
using BusinessLayer.Services.Interfaces;
using Data.Repositories.Interfaces;
using Models;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || loginDto.Password != user.PasswordHash)
                throw new ArgumentException("Невалидни данни!");

            return UserMapper.ToDto(user);
        }

        public async Task RegisterAsync(UserRegisterDto registerDto)
        {
            //тук връща грешка 
            if (registerDto.Password != registerDto.ConfirmPassword)
                throw new ArgumentException("Паролите не съвпаат");

            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);

            if (existingUser != null)
                throw new ArgumentException("Потребител с този имейл съществува!");


            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = registerDto.Password
            };

            await _userRepository.AddAsync(newUser);

        }
    }

}
