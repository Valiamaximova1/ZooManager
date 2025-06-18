using BusinessLayer.DTOs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface IUserService
    {

        Task<UserDto> LoginAsync(UserLoginDto loginDto);
        Task RegisterAsync(UserRegisterDto registerDto);
        UserDto CurrentUser { get; }
        void SetCurrentUser(UserDto user);
        Task<bool> ChangePasswordAsync(String email, string currentPassword, string newPassword);
        
    }
}
