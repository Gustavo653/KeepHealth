using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace KeepNotes.Domain.Identity
{
    public class UserRole : IdentityUserRole<long>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}