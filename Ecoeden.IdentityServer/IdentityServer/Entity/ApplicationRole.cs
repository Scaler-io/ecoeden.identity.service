using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Entity
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(string name, string normalizedName)
        {
            Name = name;
            NormalizedName = normalizedName;
        }

        public ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
        public ICollection<RolePermission> RolePermissions { get; set ; } = new List<RolePermission>();
    }
}
