using Microsoft.AspNetCore.Identity;

namespace KeepHealth.Domain.Identity
{
    public class User : IdentityUser<long>
    {
        public string Name { get; set; }
        public virtual IEnumerable<UserRole> UserRoles { get; set; }
    }
}