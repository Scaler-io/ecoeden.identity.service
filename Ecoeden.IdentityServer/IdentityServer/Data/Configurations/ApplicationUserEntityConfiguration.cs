using IdentityServer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Data.Configurations
{
    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasIndex(u => u.UserName).IsUnique();

            builder.HasMany(r => r.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(fk => fk.UserId)
                .IsRequired();

            builder.Property(u => u.Image).IsRequired(false);
            builder.Property(u => u.ImageId).IsRequired(false);
        }
    }
}
