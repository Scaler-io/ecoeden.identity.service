using System.Security.Claims;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IdentityServer;

public class SeedData
{
    public async static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Do migration if it is really required
            if (IsMigrationRequired(context))
            {
                context.Database.Migrate();
            }

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var roles = new List<ApplicationRole>
            {
                new ApplicationRole { Name = "admin", NormalizedName = "ADMIN" }
            };
            var adminPermissions = new List<ApplicationPermission>
            {
                new ApplicationPermission { Name = "role:read" },
                new ApplicationPermission { Name = "role:write" },
                new ApplicationPermission { Name = "user:read" },
                new ApplicationPermission { Name = "user:write" },
                new ApplicationPermission { Name = "inventory:read" },
                new ApplicationPermission { Name = "inventory:write" },
                new ApplicationPermission { Name = "report:read" },
                new ApplicationPermission { Name = "report:write" },
                new ApplicationPermission { Name = "user_management:read" },
                new ApplicationPermission { Name = "user_management:write" },
                new ApplicationPermission { Name = "settings:read" },
                new ApplicationPermission { Name = "settings:write" }
            };

            if (!context.Permissions.Any())
            {       
                foreach(var permission in adminPermissions)
                    context.Permissions.Add(permission);

                await context.SaveChangesAsync();
            }

            if (!roleMgr.Roles.Any())
            {
                     
                foreach(var permission in adminPermissions)
                    roles.First()
                        .RolePermissions
                        .Add(new RolePermission { Permission = permission });

                await roleMgr.CreateAsync(roles.First());
            }

            var admin = userMgr.FindByNameAsync("sharthak123").Result;

            if (admin == null)
            {

                admin = new ApplicationUser
                {
                    UserName = "sharthak123",
                    Email = "admin@ecoeden.com",
                    EmailConfirmed = true,
                    FirstName = "Sharthak",
                    Lastname = "Mallik"
                };
                var result = userMgr.CreateAsync(admin, "P@ssw0rd").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                await userMgr.AddClaimsAsync(admin, new Claim[]{
                    new Claim(JwtClaimTypes.Name, admin.UserName),
                    new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, admin.Lastname),
                    new Claim(JwtClaimTypes.Email, admin.Email),
                    new Claim(JwtClaimTypes.Role, JsonConvert.SerializeObject(string.Join(",", roles.Select(r => r.Name).ToList()))),
                    new Claim("Permissions", JsonConvert.SerializeObject(string.Join(",", adminPermissions.Select(x => x.Name).ToList())))
                });
            }

            await userMgr.AddToRoleAsync(admin, "admin");
        }
    }

    private static bool IsMigrationRequired(ApplicationDbContext context)
    {
        if (!context.Database.GetAppliedMigrations().Any())
        {
            return true;
        }

        return false;
    }
}
