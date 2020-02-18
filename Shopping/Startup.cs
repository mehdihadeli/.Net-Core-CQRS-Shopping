using System;
using System.Linq;
using System.Net.Mime;
using System.Text;
using AutoMapper;
using Common.Authentication;
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
using Shopping.Application;
using Shopping.Infrastructure;
using Shopping.Infrastructure.Persistence.Identity;

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
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

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

            //Adding Jwt Authentication
            services.AddInfrastructure(Configuration, Environment);
            services.AddApplication();

            //services.AddJwtAuthentication();

            services.AddControllersWithViews()
                .AddNewtonsoftJson();

            services.AddRazorPages();
            services.AddOpenApiDocument(configure => { configure.Title = "Auth Server"; });

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
            app.UseHttpsRedirection();
            
            // app.UseAuthentication();
            // app.UseAuthorization();

            //app.UseAccessTokenMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
            app.UseOpenApi();
            app.UseSwaggerUi3();

            #region Healthchecks

            var healthCheckOptions = new HealthCheckOptions();
            healthCheckOptions.ResultStatusCodes[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable;

            healthCheckOptions.ResponseWriter = async (ctx, rpt) =>
            {
                var result = JsonConvert.SerializeObject(new
                {
                    Status = rpt.Status.ToString(),
                    Errors = rpt.Entries.Select(e => new
                        {key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status)})
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