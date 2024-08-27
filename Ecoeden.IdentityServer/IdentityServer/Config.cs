using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new("userapi:read"),
            new("userapi:write"),
            new("catalogueapi:read"),
            new("catalogueapi:write"),
            new("searchapi:read"),
            new("searchapi:write"),
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
            },
            new("ecoeden.search.api", "Search api")
            {
                Scopes = { "searchapi:read", "searchapi:write" }
            }
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new(){
                ClientId = "ecoeden.management.ui",
                ClientName = "Management UI SPA",
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "http://localhost:4200"},
                PostLogoutRedirectUris = { "http://localhost:4200"},
                RequirePkce = true,
                RequireClientSecret = false,
                AccessTokenType = AccessTokenType.Jwt,
                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "email", "catalogueapi:read", "catalogueapi:write", "userapi:read", "userapi:write", "searchapi:read", "searchapi:write"},
                AccessTokenLifetime = 3600*24*30,
                AuthorizationCodeLifetime = 3600*24,
                AlwaysIncludeUserClaimsInIdToken = true
            },
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
                AllowedScopes = { "openid", "profile", "email", "userapi:read", "userapi:write", "catalogueapi:read", "catalogueapi:write", "searchapi:read", "searchapi:write" },
                RequireClientSecret = true,
                AccessTokenType = AccessTokenType.Jwt,
            },
        };
}
