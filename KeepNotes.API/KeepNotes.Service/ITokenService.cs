
using Common.DTO;

namespace KeepNotes.Service
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserDTO userDTO);
    }
}