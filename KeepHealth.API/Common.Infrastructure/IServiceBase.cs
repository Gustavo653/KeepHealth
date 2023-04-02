using Common.DTO;

namespace Common.Infrastructure
{
    public interface IServiceBase
    {
        Task<ResponseDTO> Compare(string user);
    }
}
