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
    }
}
