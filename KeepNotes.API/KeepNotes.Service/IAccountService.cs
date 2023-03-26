using Common.DTO;
using Microsoft.AspNetCore.Identity;

namespace KeepNotes.Service
{
    public interface IAccountService
    {
        Task<bool> UserExists(string username);
        Task<UserDTO> GetUserByUserNameAsync(string username);
        Task<SignInResult> CheckUserPasswordAsync(UserDTO userDTO, string password);
        Task<UserDTO> CreateAccountAsync(UserDTO userDto);
        Task<UserDTO> UpdateAccount(UserDTO userDTO);
    }
}