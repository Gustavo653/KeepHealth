using Common.DTO;
using Microsoft.AspNetCore.Identity;

namespace KeepNotes.Service
{
    public interface IAccountService
    {
        Task<ResponseDTO> GetUserByUserNameAsync(string userName);
        Task<ResponseDTO> CreateAccountAsync(UserDTO userDto);
        Task<ResponseDTO> UpdateAccount(UserDTO userDTO);
        Task<ResponseDTO> Login(UserLoginDTO userDTO);
    }
}