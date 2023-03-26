using Common.DataAccess;
using KeepNotes.Domain.Identity;
using KeepNotes.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KeepNotes.Application
{
    public class UserRepository : BaseRepository<User, KeepNotesContext>, IUserRepository
    {
        public UserRepository(KeepNotesContext context) : base(context)
        {
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await GetListAsync();
        }

        public async Task<User> GetUserByIdAsync(long id)
        {
            return await GetEntities().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await GetEntities().FirstOrDefaultAsync(x => x.UserName == userName);
        }
    }
}
