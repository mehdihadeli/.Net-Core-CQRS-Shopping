using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shopping.Application.Services;
using Shopping.Core.Domains;
using Shopping.Core.Services;
using Shopping.Infrastructure.Persistence;

namespace Shopping.Infrastructure
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            var currentAssembly = typeof(Startup).Assembly;
            var migrationsAssembly = typeof(ConfigurationContextDesignTimeFactory).Assembly;
            byte[] certData;
            using (var resourceStream =
                currentAssembly.GetManifestResourceStream(
                    $"{Assembly.GetCallingAssembly().GetName().Name}.Keys.Shopping.pfx"))
            {
                using (var memoryStream = new MemoryStream())
                {
                    resourceStream.CopyTo(memoryStream);
                    memoryStream.Flush();
                    certData = memoryStream.ToArray();
                }
            }

            services.Configure<CookiePolicyOptions>(options =>
            {
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddScoped<IIdentityUserValidatorService, IdentityUserValidatorService>();

            services.AddDbContext<ApplicationIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    option => option.MigrationsAssembly("Shopping")));

            if (environment.IsDevelopment() || environment.IsEnvironment("Test"))
            {
                services.Configure<IdentityOptions>(options =>
                {
                    //Week password settings for only development
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                });
            }

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IIdentityDataContext>(sp => sp.GetRequiredService<ApplicationIdentityDbContext>());

            services.AddDbContext<ShoppingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    option => option.MigrationsAssembly("Shopping")));

            services.AddScoped<IShoppingDataContext>(provider => provider.GetService<ShoppingDbContext>());

            if (environment.IsEnvironment("Test"))
            {
                // services.AddIdentityServer()
                //     .AddApiAuthorization<ApplicationUser, DbContext>(options =>
                //     {
                //         options.Clients.Add(new Client
                //         {
                //             ClientId = "Northwind.IntegrationTests",
                //             AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                //             ClientSecrets = { new Secret("secret".Sha256()) },
                //             AllowedScopes = { "Northwind.WebUIAPI", "openid", "profile" }
                //         });
                //     }).AddTestUsers(new List<TestUser>
                //     {
                //         new TestUser
                //         {
                //             SubjectId = "f26da293-02fb-4c90-be75-e4aa51e0bb17",
                //             Username = "jason@northwind",
                //             Password = "Northwind1!",
                //             Claims = new List<Claim>
                //             {
                //                 new Claim(JwtClaimTypes.Email, "jason@northwind")
                //             }
                //         }
                //     });
                services.AddIdentityServer().AddDeveloperSigningCredential()
                    // this adds the operational data from DB (codes, tokens, consents)
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                        // this enables automatic token cleanup. this is optional.
                        options.EnableTokenCleanup = true;
                        options.TokenCleanupInterval = 30; // interval in seconds
                    })
                    .AddTestUsers(new List<TestUser>
                    {
                        new TestUser
                        {
                            SubjectId = "1",
                            Username = "Test",
                            Password = "Test",
                            Claims = new List<Claim>
                            {
                                new Claim(JwtClaimTypes.Email, "test@test.com")
                            }
                        }
                    })
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiResources(Config.GetApiResources())
                    .AddInMemoryClients(Config.GetClients())
                    .AddAspNetIdentity<ApplicationUser>();
            }
            else
            {
                services.AddIdentityServer()
                    .AddSigningCredential(new X509Certificate2(
                        rawData: certData,
                        password: configuration.GetValue<String>("OAuth:CertificatePassword")))
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                        {
                            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly.GetName().Name));
                        };
                    })
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                        {
                            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly.GetName().Name));
                        };
                    })
                    .AddAspNetIdentity<ApplicationUser>()
                    .AddProfileService<IdentityClaimsProfileService>();

                services.AddAuthentication(IdentityConstants.ApplicationScheme)
                    .AddOpenIdConnect(
                        authenticationScheme: "Google",
                        displayName: "Google",
                        configureOptions: options =>
                        {
                            configuration.Bind("Google", options);
                            options.Scope.Add("email");
                            options.Scope.Add("profile");
                        })
                    .AddMicrosoftAccount(
                        authenticationScheme: "Microsoft",
                        displayName: "Microsoft",
                        configureOptions: options =>
                        {
                            configuration.Bind("Microsoft", options);
                            options.CorrelationCookie.SameSite = SameSiteMode.None;
                        })
                    .AddFacebook(
                        authenticationScheme: "Facebook",
                        displayName: "Facebook",
                        configureOptions: options =>
                        {
                            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                            configuration.Bind("Facebook", options);
                            options.Scope.Add("email");
                        });
            }

            services.AddAuthentication().AddIdentityServerJwt();

            return services;
        }
    }
}