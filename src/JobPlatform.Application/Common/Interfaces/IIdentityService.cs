using JobPlatform.Domain.Entities;

namespace JobPlatform.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(bool Success, string[] Errors, ApplicationUser User)> CreateUserAsync(
        string email,
        string password,
        string role,
        CancellationToken cancellationToken = default);

    Task<(bool Success, ApplicationUser? User, IList<string> Roles)> CheckPasswordAndGetRolesAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}
