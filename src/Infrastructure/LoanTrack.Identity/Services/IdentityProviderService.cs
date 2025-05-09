using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoanTrack.Application.Common.Identity;
using LoanTrack.Application.Employees.Queries.Auth;
using LoanTrack.Domain.Common;
using LoanTrack.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LoanTrack.Identity.Services;

public class IdentityProviderService(
    UserManager<AppIdentityUser> userManager,
    SignInManager<AppIdentityUser> signInManager,
    IUserStore<AppIdentityUser> userStore,
    IOptions<JwtSettings> jwtSettings
) : IIdentityProviderService
{
    public async Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var appUser = new AppIdentityUser
        {
            UserName = user.Email,
            Email = user.Email,
            Firstname = user.FirstName,
            Lastname = user.LastName,
            EmailConfirmed = true
        };
        
       // var appUser = CreateUser();

        await userStore.SetUserNameAsync(appUser, user.Email, CancellationToken.None);
        var emailStore = GetEmailStore();
        await emailStore.SetEmailAsync(appUser, user.Email, CancellationToken.None);
        
        var identityResult = await userManager.CreateAsync(appUser, user.Password);
        if (identityResult.Succeeded)
        {
            var role = string.IsNullOrEmpty(user.UserRole) ? "Employee" : user.UserRole;
            await userManager.AddToRoleAsync(appUser, role);
            return appUser.Id;
        }
        
        StringBuilder str = new StringBuilder();
        foreach (IdentityError error in identityResult.Errors)
        {
            str.Append(CultureInfo.InvariantCulture, $".{error.Description}\n");
        }
        
        var code = identityResult.Errors.First().Code;
        return Result.Failure<string>(Error.Failure(code, $"Failed to register user: {str}"));
    }

    public async Task UpdateUserRoleAsync(string userId, string role, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }

    public async Task<Result<AuthResponse>>  LoginAsync(
        AuthRequest auth,
        (Guid id, string name) employee,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(auth.Email);
            if (user == null)
            {
                return Result.Failure<AuthResponse>(Error.Failure("401", $"User {auth.Email} does not exist."));
            }
            
            /*var isAuthenticated = await userManager.CheckPasswordAsync(user, auth.Password);
            if (!isAuthenticated)
            {
                return Result.Failure<AuthResponse>(Error.Failure("401", "Invalid credentials."));
            }*/
            var result = await signInManager.CheckPasswordSignInAsync(user, auth.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return Result.Failure<AuthResponse>(Error.Failure("401", "Invalid credentials."));
            }

            JwtSecurityToken securityToken = await GenerateToken(user, employee);
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new AuthResponse(user.Id, employee.name, user.Email!, token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Failure<AuthResponse>(Error.Failure("401", $"Invalid credentials."));
        }
    }
    private async Task<JwtSecurityToken> GenerateToken(AppIdentityUser user, (Guid id, string name) employee)
    {
        var _jwtSettings = jwtSettings.Value;
        var userClaims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("uid", user.Id),
            new("EmployeeId", employee.id.ToString()),
            new("FullName", employee.name)
        }
        .Union(userClaims)
        .Union(roleClaims);
        
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: credentials
        );
        return jwtSecurityToken;
    }
    
    private AppIdentityUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<AppIdentityUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(AppIdentityUser)}'. " +
                                                $"Ensure that '{nameof(AppIdentityUser)}' is not an abstract class and has a parameterless constructor.");
        }
    }

    private IUserEmailStore<AppIdentityUser> GetEmailStore()
    {
        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }

        return (IUserEmailStore<AppIdentityUser>)userStore;
    }
}
