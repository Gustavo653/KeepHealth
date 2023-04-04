
using Common.DTO;

namespace KeepHealth.Service.Interface
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserDTO userDTO);
    }
}