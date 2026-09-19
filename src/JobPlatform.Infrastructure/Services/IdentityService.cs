using JobPlatform.Application.Common.Interfaces;
using JobPlatform.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace JobPlatform.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<(bool Success, string[] Errors, ApplicationUser User)> CreateUserAsync(
        string email,
        string password,
        string role,
        CancellationToken cancellationToken = default)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return (false, new[] { "A user with this email already exists." }, existingUser);
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            var errors = createResult.Errors.Select(e => e.Description).ToArray();
            return (false, errors, user);
        }

        // Ensure the role exists in Identity
        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole(role));
        }

        var addRoleResult = await _userManager.AddToRoleAsync(user, role);
        if (!addRoleResult.Succeeded)
        {
            var errors = addRoleResult.Errors.Select(e => e.Description).ToArray();
            return (false, errors, user);
        }

        return (true, Array.Empty<string>(), user);
    }

    public async Task<(bool Success, ApplicationUser? User, IList<string> Roles)> CheckPasswordAndGetRolesAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return (false, null, Array.Empty<string>());
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
        {
            return (false, null, Array.Empty<string>());
        }

        var roles = await _userManager.GetRolesAsync(user);
        return (true, user, roles);
    }
}
