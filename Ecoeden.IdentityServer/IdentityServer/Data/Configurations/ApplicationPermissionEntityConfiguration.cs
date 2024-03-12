using IdentityServer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Data.Configurations
{
    public class ApplicationPermissionEntityConfiguration : IEntityTypeConfiguration<ApplicationPermission>
    {
        public void Configure(EntityTypeBuilder<ApplicationPermission> builder)
        {
            builder.HasMany(rp => rp.RolePermissions)
                .WithOne(p => p.Permission)
                .HasForeignKey(fk => fk.PermissionId)
                .IsRequired();
        }
    }
}
