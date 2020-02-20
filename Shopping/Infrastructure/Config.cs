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
                new ApiResource("shopping.api", "Shopping API")
                {
                    Scopes = {new Scope("Shopping.API")},
                    UserClaims = new[] {"email", "userid", "role", "permission"}
                },
                new ApiResource("resourceapi", "Resource API")
                {
                    Scopes = {new Scope("api.read")}
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
                new Client
                {
                    ClientId = "shopping.app",
                    ClientName = "Shopping Backend Application",
                    Description = "Secure client using ResourceOwnerPassword flow",
                    ClientSecrets = new[] {new Secret("5aKtv2wvyP".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new[]
                    {
                        "shopping.api"
                    }
                },
                new Client
                {
                    ClientId = "shopping.js",
                    ClientName = "Shopping JavaScrip Web Application",
                    Description = "Un secure static content client",
                    ClientSecrets = new[] {new Secret("5aKtv2wvyP".Sha256())},
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new[]
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "shopping.api"
                    },
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new[] {"https://localhost:5004/signin-oidc"},
                    PostLogoutRedirectUris = {"https://localhost:5004/signout-callback-oidc"},
                },
                new Client
                {
                    ClientId = "shopping.web",
                    ClientName = "Shopping Web Application",
                    Description = "Web application client with back-channel",
                    ClientSecrets = new[] {new Secret("5aKtv2wvyP".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes = new[]
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "shopping.api"
                    },
                    AccessTokenLifetime = (int) TimeSpan.FromHours(1).TotalSeconds,
                    AuthorizationCodeLifetime = (int) TimeSpan.FromMinutes(5).TotalSeconds,
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = new[]
                    {
                        "https://localhost:5004/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:5004/signout-callback-oidc"
                    },
                }
            };
        }
    }
}