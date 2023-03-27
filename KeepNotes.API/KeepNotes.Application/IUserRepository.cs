using Common.Infrastructure;
using KeepNotes.Domain.Identity;

namespace KeepNotes.Application
{
    public interface IUserRepository : IRepositoryBase<User>
    {
    }
}
