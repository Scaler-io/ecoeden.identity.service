using IdentityServer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Data.Configurations
{
    public class ApplicationRoleEntityConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasMany(ur => ur.UserRoles)
                .WithOne(r => r.Role)
                .HasForeignKey(fk => fk.RoleId)
                .IsRequired();

            builder.HasMany(rp => rp.RolePermissions)
                .WithOne(r => r.Role)
                .HasForeignKey(fk => fk.RoleId)
                .IsRequired();
        }
    }
}
