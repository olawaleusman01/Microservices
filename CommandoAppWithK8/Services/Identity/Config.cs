using Duende.IdentityServer.Models;

namespace Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };


    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("command", "Command Service API")
            {
                Scopes = { "command.fullaccess" }
            },
            new ApiResource("platform", "Platform Service API")
            {
                Scopes = { "platform.fullaccess", "platform.readaccess", "platform.writeaccess" }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
                new ApiScope("command.fullaccess"),
                new ApiScope("platform.readaccess"),
                new ApiScope("platform.fullaccess"),
                new ApiScope("platform.writeaccess")
        };


    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "platformm2m",
                ClientName = "Platform m2m to Command Service",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                AllowedScopes = { "command.writeaccess" }
            },
            new Client
            {
                ClientId = "commandm2m",
                ClientName = "Command m2m to Platform Service",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("E4520DAD-D7C4-4B66-AA40-9ABEDC4D7589".Sha256()) },
                AllowedScopes = { "platform.readaccess" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "commandoAngularinteractive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "http://localhost:4200" },
                //FrontChannelLogoutUri = "https://localhost:4200/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
                AllowOfflineAccess = true,
                AllowedCorsOrigins={"http://localhost:4200"},
                AllowAccessTokensViaBrowser = true,
                AllowedScopes = { "openid", "profile", "command.fullaccess","platform.fullaccess" }
            },
        };
}
