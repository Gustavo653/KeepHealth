using KeepNotes.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeepNotes.Persistence
{
    public class KeepNotesContext : IdentityDbContext<User, Role, long,
                                               IdentityUserClaim<long>, UserRole, IdentityUserLogin<long>,
                                               IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public KeepNotesContext(DbContextOptions<KeepNotesContext> options) : base(options) { }

        protected KeepNotesContext()
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            }
           );
        }
    }
}
