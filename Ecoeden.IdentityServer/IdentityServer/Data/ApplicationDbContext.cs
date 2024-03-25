using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Entity;
using IdentityServer.Data.Configurations;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationPermissionEntityConfiguration).Assembly);
        builder.Entity<RolePermission>()
            .HasKey(ck => new { ck.RoleId, ck.PermissionId });
    }

    public DbSet<ApplicationPermission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
}
