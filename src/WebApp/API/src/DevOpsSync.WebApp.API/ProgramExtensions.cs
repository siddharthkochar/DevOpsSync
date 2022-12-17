using DevOpsSync.WebApp.API.Data;
using DevOpsSync.WebApp.Services;
using DevOpsSync.WebApp.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using RestSharp;

namespace DevOpsSync.WebApp.API;

public static class ProgramExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<Settings>(configuration.GetSection("Settings"));
        services.AddSingleton(typeof(IDataStore), typeof(DataStore));
        services.AddSingleton(typeof(IRestClient), typeof(RestClient));
        services.AddScoped(typeof(IGitHubService), typeof(GitHubService));
        services.AddScoped(typeof(ISlackService), typeof(SlackService));
        services.AddScoped(typeof(IDevOpsService), typeof(DevOpsService));
        services.AddDbContext<DevOpsSyncDbContext>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseSqlServer();
        });
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["Auth0:Authority"];
                options.Audience = configuration["Auth0:Audience"];
            });

        return services;
    }
}
