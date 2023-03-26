using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace KeepNotes.Domain.Identity
{
    public class User : IdentityUser<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual IEnumerable<UserRole> UserRoles { get; set; }
    }
}