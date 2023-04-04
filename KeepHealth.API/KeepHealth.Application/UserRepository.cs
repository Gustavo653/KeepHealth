using Common.DataAccess;
using KeepHealth.Application.Interface;
using KeepHealth.Domain.Identity;
using KeepHealth.Persistence;

namespace KeepHealth.Application
{
    public class UserRepository : BaseRepository<User, KeepHealthContext>, IUserRepository
    {
        public UserRepository(KeepHealthContext context) : base(context)
        {
        }
    }
}
