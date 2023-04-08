using Common.DTO;
using KeepHealth.Domain.Enum;
using KeepHealth.Domain.Identity;

namespace KeepHealth.Service.Interface
{
    public interface IAccountService
    {
        Task<ResponseDTO> CreateOrUpdateUser(long? id, UserDTO user);
        Task<ResponseDTO> AddUserInRole(User user, RoleName role);
        Task<ResponseDTO> GetUserByUserNameAsync(string userName);
        Task<ResponseDTO> Login(UserLoginDTO userDTO);
    }
}