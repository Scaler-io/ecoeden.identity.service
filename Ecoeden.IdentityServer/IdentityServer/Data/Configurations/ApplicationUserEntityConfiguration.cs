using IdentityServer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Data.Configurations
{
    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasMany(r => r.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(fk => fk.UserId)
                .IsRequired();

            builder.Property(r => r.Image).IsRequired(false);
        }
    }
}
