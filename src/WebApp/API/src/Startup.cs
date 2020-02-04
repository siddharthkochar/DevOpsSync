using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using DevOpsSync.WebApp.API.Code;
using DevOpsSync.WebApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DevOpsSync.WebApp.Services;
using DevOpsSync.WebApp.Utility;

namespace DevOpsSync.WebApp.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configurationSection = Configuration.GetSection("ConnectionStrings:DefaultConnection");
            services.AddDbContext<DevOpsSyncDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(configurationSection.Value);
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Auth0:Authority"];
                    options.Audience = Configuration["Auth0:Audience"];
                });

            services.AddControllers();
            services.Configure<Settings>(Configuration.GetSection("Settings"));
            services.AddScoped(typeof(ISlack), typeof(Slack));
            services.AddSingleton(typeof(IDataStore), typeof(DataStore));
            services.AddScoped(typeof(IGitHubService), typeof(GitHubService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(config =>
                {
                    config.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .SetIsOriginAllowed(x => true);
                });
            }
            
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
