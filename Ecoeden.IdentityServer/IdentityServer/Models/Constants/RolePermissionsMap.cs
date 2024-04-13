using IdentityServer.Entity;

namespace IdentityServer.Models.Constants
{
    public static class RolePermissionsMap
    {
        public static List<ApplicationPermission> AdminPermissions = new()
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

        public static List<ApplicationPermission> OperatorPermissions = new()
        {
            new ApplicationPermission("role:read"),
            new ApplicationPermission("role:write"),
            new ApplicationPermission("user:read"),
            new ApplicationPermission("user:write"),
            new ApplicationPermission("inventory:read"),
            new ApplicationPermission("inventory:write"),
        };

        public static List<ApplicationPermission> AuditorPermissions = new()
        {
            new ApplicationPermission("role:read"),
            new ApplicationPermission("user:read"),
            new ApplicationPermission("inventory:read"),
            new ApplicationPermission("report:read")
        };
    }
}
