﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using AutoMapper;
using Common.Redis;
using Common.Web.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using Shopping.Application;
using Shopping.Infrastructure;
using Shopping.Infrastructure.Persistence;

namespace Shopping
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IServiceCollection _services;
        protected IConfiguration Configuration { get; }
        protected IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            services.AddHttpContextAccessor();
            services.AddRedis();
            services.AddMiniProfiler().AddEntityFramework();
            services.AddAutoMapper(typeof(Startup));

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationIdentityDbContext>();

            // action filter for logging requests with serilog
            // services.AddControllersWithViews(opts =>
            // {
            //     opts.Filters.Add<SerilogMvcLoggingAttribute>();
            // });

            services.AddInfrastructure(Configuration, Environment);
            services.AddApplication();

            services.AddControllersWithViews()
                .AddNewtonsoftJson();

            services.AddRazorPages();
            services.AddOpenApiDocument(document =>
                    {
                        document.AddSecurity("bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                        {
                            Type = OpenApiSecuritySchemeType.OAuth2,
                            Description = "Authentication",
                            Flow = OpenApiOAuth2Flow.Implicit,
                            Flows = new OpenApiOAuthFlows()
                            {
                                Implicit = new OpenApiOAuthFlow()
                                {
                                    Scopes = new Dictionary<string, string>
                                    {
                                        {"Shopping.API", "Shopping API"}

                                    },
                                    TokenUrl = "http://localhost:8000/connect/token",
                                    AuthorizationUrl = "http://localhost:8000/connect/authorize",

                                },
                            }
                        });

                        document.OperationProcessors.Add(
                            new AspNetCoreOperationSecurityScopeProcessor("bearer"));
                    }
                );

            _services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseMiniProfiler();
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                RegisteredServicesPage(app);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseSerilogRequestLogging();

            app.UseCustomExceptionHandler();
            app.UseHealthChecks("/health");
            app.UseCors("CorsPolicy");
            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                settings.OAuth2Client = new OAuth2ClientSettings
                {
                    ClientId = "demo_api_swagger",

                    AppName = "Auth Server API - Swagger",

                };
            });

            #region Healthchecks

            var healthCheckOptions = new HealthCheckOptions();
            healthCheckOptions.ResultStatusCodes[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable;

            healthCheckOptions.ResponseWriter = async (ctx, rpt) =>
            {
                var result = JsonConvert.SerializeObject(new
                {
                    Status = rpt.Status.ToString(),
                    Errors = rpt.Entries.Select(e => new
                    { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
                }, Formatting.None, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                ctx.Response.ContentType = MediaTypeNames.Application.Json;
                await ctx.Response.WriteAsync(result);
            };

            app.UseHealthChecks("/health", healthCheckOptions);

            #endregion
        }

        private void RegisteredServicesPage(IApplicationBuilder app)
        {
            app.Map("/services", builder => builder.Run(async context =>
            {
                var sb = new StringBuilder();
                sb.Append("<h1>Registered Services</h1>");
                sb.Append("<table><thead>");
                sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
                sb.Append("</thead><tbody>");
                foreach (var svc in _services)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{svc.ServiceType.FullName}</td>");
                    sb.Append($"<td>{svc.Lifetime}</td>");
                    sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                    sb.Append("</tr>");
                }

                sb.Append("</tbody></table>");
                await context.Response.WriteAsync(sb.ToString());
            }));
        }
    }
}