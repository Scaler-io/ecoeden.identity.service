using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new("userapi:read"),
            new("userapi:write"),
            new("catalogueapi:read"),
            new("catalogueapi:write"),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new("ecoeden.user.api", "User manager api")
            {
                Scopes = { "userapi:read", "userapi:write" }
            },
            new("ecoeden.catalogue.api", "Catalogue api")
            {
                Scopes = { "catalogueapi:read", "catalogueapi:write" }
            }
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new()
            {
                ClientId = "ecoeden.user.api",
                ClientName = "User Api",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                RedirectUris = { "https://www.getpostmane.com/oauth2/callback" },
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256())},
                AllowedScopes = { "userapi:read", "userapi:write" },
                RequireClientSecret = true,
                AccessTokenType = AccessTokenType.Jwt
            },
            new()
            {
                ClientId = "ecoeden.catalogue.api",
                ClientName = "Catalogue Api",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                RedirectUris = { "https://www.getpostmane.com/oauth2/callback" }, // Not going to be used. nore redirection in postman testing 
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                AllowedScopes = { "catalogueapi:read", "catalogueapi:write" },
                RequireClientSecret = true,
                AccessTokenType = AccessTokenType.Jwt,
            },
            // m2m client credentials flow client
            new()
            {
                ClientId = "postman",
                ClientName = "Postman",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RedirectUris = { "https://www.getpostmane.com/oauth2/callback" }, // Not going to be used. nore redirection in postman testing 
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                AllowedScopes = { "openid", "profile", "email", "userapi:read", "userapi:write", "catalogueapi:read", "catalogueapi:write" },
                RequireClientSecret = true,
                AccessTokenType = AccessTokenType.Jwt,
            },
        };
}
