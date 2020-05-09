using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Shopping.Infrastructure
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Shopping.API", "Shopping API")
                {
                    // Scopes = {new Scope("Shopping.API")},
                    UserClaims = new[] {"email", "userid", "role", "permission"}
                },
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    RequireConsent = false,
                    ClientId = "angular_spa",
                    ClientName = "Angular SPA",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = {"openid", "profile", "email", "api.read"},
                    RedirectUris = {"http://localhost:4200/auth-callback"},
                    PostLogoutRedirectUris = {"http://localhost:4200/"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                },
                new Client()
                {
                    ClientId = "Shopping.ResourcePassword.Client",
                    AllowedGrantTypes = {GrantType.ResourceOwnerPassword},
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedScopes = {"Shopping.API", "openid", "profile"}
                },
                new Client {
                    ClientId = "demo_api_swagger",
                    ClientName = "Swagger UI for demo_api",
                    AllowedGrantTypes = new List<string>(){GrantType.Implicit,GrantType.ResourceOwnerPassword},
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = {"https://localhost:44304/swagger/oauth2-redirect.html"},
                    AllowedScopes = { "Shopping.API" }
                },

            };
        }
    }
}