using Microsoft.AspNetCore.Identity;

namespace KeepHealth.Domain.Identity
{
    public class Role : IdentityRole<long>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}