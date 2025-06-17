using BusinessLayer.DTOs;
using BusinessLayer.Mappers;
using BusinessLayer.Services.Interfaces;
using Shared.Utils;
using Data.Repositories.Interfaces;
using Models;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private User _currentUser;

        public UserDto CurrentUser => _currentUser != null ? UserMapper.ToDto(_currentUser) : null;


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void SetCurrentUser(UserDto userDto)
        {
            _currentUser = UserMapper.ToEntity(userDto);
        }

        public async Task<UserDto> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || user.PasswordHash != PasswordHasher.Hash(loginDto.Password))
                   throw new ArgumentException("Невалидни данни!");

            return UserMapper.ToDto(user);
        }

        public async Task RegisterAsync(UserRegisterDto registerDto)
        {
            //тук връща грешка 
            if (registerDto.Password != registerDto.ConfirmPassword)
                throw new ArgumentException("Паролите не съвпадат");

            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);

            if (existingUser != null)
                throw new ArgumentException("Потребител с този имейл съществува!");


            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = PasswordHasher.Hash(registerDto.Password)
            };

            await _userRepository.AddAsync(newUser);

        }

        public async Task<bool> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.PasswordHash != PasswordHasher.Hash(currentPassword))
                return false;

            user.PasswordHash = PasswordHasher.Hash(newPassword);
            await _userRepository.UpdateAsync(user);
            return true;
        }
    }

}
