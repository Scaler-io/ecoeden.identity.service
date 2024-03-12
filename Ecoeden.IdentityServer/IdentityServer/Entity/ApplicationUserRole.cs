using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Entity
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public ApplicationUser User { get; set; }
        public ApplicationRole Role { get; set; }
    }
}
