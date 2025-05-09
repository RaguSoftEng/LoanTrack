using Blazored.Toast;
using LoanTrack.Application;
using LoanTrack.Identity;
using LoanTrack.Identity.Models;
using LoanTrack.Persistence;
using LoanTrack.Web.Common;
using LoanTrack.Web.Components.Account;
using LoanTrack.Web.Shared.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LoanTrack.Web;

internal static class WebAppConfigurations
{
    public static IServiceCollection AddLoanTrack(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("DefaultConnection") ??
                           throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        
        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        services.AddApplication()
            .AddPersistence(dbConnection);
        
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddLoanTrackIdentityService(dbConnection);
        
        services.AddSingleton<IEmailSender<AppIdentityUser>, IdentityNoOpEmailSender>();

        services.AddScoped<AppSettingState>();
        services.AddScoped<CurrentUserService>();
        services.AddBlazoredToast();
        
        return services;
    }
}
