
using System.Globalization;
using LoanTrack.Application.Common.Identity;
using LoanTrack.Identity.Database;
using LoanTrack.Identity.Models;
using LoanTrack.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoanTrack.Identity;

public static class IdentityServiceConfiguration
{
    public static IServiceCollection AddIdentityService(
        this IServiceCollection services,
        IConfiguration configuration,
        string dbConnection
    )
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        if (!jwtSettings.Exists())
        {
            throw new Exception("JwtSettings section is missing in configuration.");
        }

        services.Configure<JwtSettings>(x =>
        {
            x.Audience = jwtSettings["Audience"]!;
            x.Issuer = jwtSettings["Issuer"]!;
            x.Key = jwtSettings["Key"]!;
            x.DurationInMinutes = int.Parse(jwtSettings["DurationInMinutes"]!, CultureInfo.CurrentCulture);
        });
        
        services.AddDbContext<LoanTrackIdentityDbContext>((sp, options) => 
            options.UseNpgsql(
                    dbConnection,
                    sqlOptions => sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Identity")
                )
                .UseSnakeCaseNamingConvention()
        );
        
        services.AddIdentity<AppIdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<LoanTrackIdentityDbContext>()
            .AddDefaultTokenProviders();
        
        /*services.AddIdentityCore<AppIdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<LoanTrackIdentityDbContext>();*/

        services.AddTransient<IIdentityProviderService, IdentityProviderService>();

        return services;
    }

    public static IServiceCollection AddLoanTrackIdentityService(this IServiceCollection services, string dbConnection)
    {
        services.AddDbContext<LoanTrackIdentityDbContext>((sp, options) =>
            options.UseNpgsql(
                    dbConnection,
                    sqlOptions => sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Identity")
                )
                .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
                .UseSnakeCaseNamingConvention()
        );
        
        services.AddTransient<IIdentityProviderService, IdentityProviderService>();
        services.AddIdentityCore<AppIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<LoanTrackIdentityDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
        return services;
    }
}
