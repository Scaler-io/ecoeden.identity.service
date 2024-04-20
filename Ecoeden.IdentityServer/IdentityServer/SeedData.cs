using System.Data;
using System.Security.Claims;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Entity;
using IdentityServer.Extensions;
using IdentityServer.Models.Constants;
using IdentityServer.Models.Enums;
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
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            // Do migration if it is really required
            if (IsMigrationRequired(context))
            {
                context.Database.Migrate();
            }

            // begin the transactions
            context.Database.BeginTransaction();

            // persists permissions
            await SeedPermissions(context);

            // persists role
            await SeedRoles(context, roleMgr);

            // persists users
            await SeedUsers(context, userMgr);

            // commit transaction
            context.Database.CommitTransaction();
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

    private static List<ApplicationRole> GetAppRoles()
    {
        return new List<ApplicationRole>
        {
            new ApplicationRole(Roles.Admin.ToString(), Roles.Admin.GetEnumMemberAttributeValue()),
            new ApplicationRole(Roles.Operator.ToString(), Roles.Operator.GetEnumMemberAttributeValue()),
            new ApplicationRole(Roles.Auditor.ToString(), Roles.Auditor.GetEnumMemberAttributeValue())
        };
    }

    private static List<ApplicationPermission> GetPermissions()
    {
        return new List<ApplicationPermission>
        {
            new ApplicationPermission("role:read"),
            new ApplicationPermission("role:write"),
            new ApplicationPermission("user:read"),
            new ApplicationPermission("user:write"),
            new ApplicationPermission("inventory:read"),
            new ApplicationPermission("inventory:write"),
            new ApplicationPermission("report:read"),
            new ApplicationPermission("report:write"),
            new ApplicationPermission("user_management:read"),
            new ApplicationPermission("user_management:write"),
            new ApplicationPermission("settings:read"),
            new ApplicationPermission("settings:write")
        };
    }

    private static async Task SeedPermissions(ApplicationDbContext context)
    {
        var permissions = GetPermissions();
        if (!context.Permissions.Any())
        {
            foreach (var permission in permissions)
                context.Permissions.Add(permission);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedRoles(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
    {
        var roles = GetAppRoles();
       
        if (context.Roles.Any())
        {
            return;
        }

        foreach(var role in roles)
        {
            var rolePermissions = GetRolePermissions(role.Name);
            foreach(var permission in rolePermissions)
            {
                role.RolePermissions.Add(new RolePermission { Permission = permission });
            }
            await roleManager.CreateAsync(role);
        }
    }

    private static List<ApplicationPermission> GetRolePermissions(string roleName)
    {
        return roleName switch
        {
           "Admin" => RolePermissionsMap.AdminPermissions,
           "Operator" => RolePermissionsMap.OperatorPermissions,
           "Auditor" => RolePermissionsMap.AuditorPermissions,
            _ => new List<ApplicationPermission>()
        };
    }

    private static async Task SeedUsers(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        if (context.Users.Any())
        {
            return;
        }

        var roles = GetAppRoles();
        var adminUser = new ApplicationUser("sharthak123", "Sharthak", "Mallik", "sharthak@ecoeden.com", true, true);
        var operatorUser = new ApplicationUser("john123", "John", "Doe", "john@ecoeden.com");
        var auditorUser = new ApplicationUser("david100", "David", "Warn", "david@ecoeden.com");

        try
        {
            await userManager.CreateAsync(adminUser, "P@ssw0rd");
            //await userManager.CreateAsync(operatorUser, "P@ssw0rd");
            //await userManager.CreateAsync(auditorUser, "P@ssw0rd");

            await AddToClaim(adminUser, userManager,
                roles.Where(x => x.Name == Roles.Admin.ToString()).ToList(),
                RolePermissionsMap.AdminPermissions);

            //await AddToClaim(operatorUser, userManager,
            //    roles.Where(x => x.Name == Roles.Operator.ToString()).ToList(),
            //    RolePermissionsMap.AdminPermissions);

            //await AddToClaim(auditorUser, userManager,
            //    roles.Where(x => x.Name == Roles.Auditor.ToString()).ToList(),
            //    RolePermissionsMap.AdminPermissions);

            await userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
            //await userManager.AddToRoleAsync(operatorUser, Roles.Operator.ToString());
            //await userManager.AddToRoleAsync(auditorUser, Roles.Auditor.ToString());
        }
        catch (Exception)
        {
            // exception handling
        }
    }

    private static async Task AddToClaim(ApplicationUser user, UserManager<ApplicationUser> userManager, 
        List<ApplicationRole> roles,
        List<ApplicationPermission> permissions)
    {
        await userManager.AddClaimsAsync(user, new Claim[]{
            new Claim(JwtClaimTypes.Name, user.UserName),
            new Claim(JwtClaimTypes.GivenName, user.FirstName),
            new Claim(JwtClaimTypes.FamilyName, user.Lastname),
            new Claim(JwtClaimTypes.Email, user.Email),
            new Claim(JwtClaimTypes.Role, JsonConvert.SerializeObject(string.Join(",", roles.Select(r => r.Name).ToList()))),
            new Claim("Permissions", JsonConvert.SerializeObject(string.Join(",", permissions.Select(x => x.Name).ToList())))
        });
    }
}
