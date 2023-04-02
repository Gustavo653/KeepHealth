using KeepHealth.Domain;
using KeepHealth.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeepHealth.Persistence
{
    public class KeepHealthContext : IdentityDbContext<User, Role, long,
                                               IdentityUserClaim<long>, UserRole, IdentityUserLogin<long>,
                                               IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public KeepHealthContext(DbContextOptions<KeepHealthContext> options) : base(options) { }

        protected KeepHealthContext()
        {
        }

        public Doctor Doctor { get; set; }
        public MedicalSpeciality MedicalSpeciality { get; set; }
        public MedicalCondition MedicalCondition { get; set; }
        public Patient Patient { get; set; }

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
