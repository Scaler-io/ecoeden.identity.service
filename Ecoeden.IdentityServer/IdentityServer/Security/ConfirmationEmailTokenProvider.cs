using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityServer.Security
{
    public sealed class ConfirmationEmailTokenProvider<TUser> : DataProtectorTokenProvider<TUser>
        where TUser : IdentityUser
    {
        public ConfirmationEmailTokenProvider(IDataProtectionProvider dataProtectionProvider, 
            IOptions<DataProtectionTokenProviderOptions> options, 
            ILogger<DataProtectorTokenProvider<TUser>> logger) 
            : base(dataProtectionProvider, options, logger)
        {
            options.Value.TokenLifespan = TimeSpan.FromSeconds(3600);
        }
    }
}
