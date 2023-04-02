
using Common.DTO;

namespace KeepHealth.Service
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserDTO userDTO);
    }
}