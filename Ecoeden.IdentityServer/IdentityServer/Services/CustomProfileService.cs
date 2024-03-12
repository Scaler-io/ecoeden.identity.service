using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public CustomProfileService(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            var roles = await _userManager.GetRolesAsync(user);
            var permissions = new List<ApplicationPermission>();

            foreach (var roleName in roles)
            {
                var role = await _roleManager.Roles.Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
                        .FirstOrDefaultAsync(x => x.Name == roleName);
                if (role is not null)
                {
                    var rolePermissions = role.RolePermissions.Select(rp => rp.Permission).ToList();
                    permissions.AddRange(rolePermissions);
                }                
            }

            var existingClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim("username", user.UserName),
                new Claim("roles", JsonConvert.SerializeObject(roles)),
                new Claim("first name", user.FirstName),
                new Claim("last name", user.Lastname),
                new Claim("permissions", JsonConvert.SerializeObject(permissions.Distinct().Select(x => x.Name).ToList()))
            };
            context.IssuedClaims.AddRange(claims);
            context.IssuedClaims.Add(existingClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email));
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}
