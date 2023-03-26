using Common.Infrastructure;
using KeepNotes.Domain.Identity;

namespace KeepNotes.Application
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(long id);
        Task<User> GetUserByUserNameAsync(string userName);
    }
}
