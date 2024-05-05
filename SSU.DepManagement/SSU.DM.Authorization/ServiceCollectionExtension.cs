using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SSU.DM.Authorization.Db;
using SSU.DM.Authorization.Models;

namespace SSU.DM.Authorization;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AuthorizationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            //.AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();
        
        services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.RequireUniqueEmail = false;
        });
        
        return services;
    }
}