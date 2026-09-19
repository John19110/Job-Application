using JobPlatform.Application.Common.Interfaces;
using JobPlatform.Application.DTOs.Auth;
using JobPlatform.Application.Interfaces;
using JobPlatform.Domain.Enums;
using JobPlatform.Domain.Exceptions;

namespace JobPlatform.Application.Services;

public class AuthService : IAuthService
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IIdentityService identityService, IJwtTokenGenerator jwtTokenGenerator)
    {
        _identityService = identityService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new BadRequestException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new BadRequestException("Password is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Role) || !Roles.IsValidRole(request.Role))
        {
            throw new BadRequestException($"Role is invalid. Allowed roles are: {string.Join(", ", Roles.All)}.");
        }

        var (success, errors, user) = await _identityService.CreateUserAsync(
            request.Email.Trim(),
            request.Password,
            request.Role,
            cancellationToken);

        if (!success)
        {
            throw new BadRequestException(string.Join("; ", errors));
        }

        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user, new[] { request.Role });

        return new AuthResponseDto(
            Token: token,
            UserId: user.Id,
            Email: user.Email ?? request.Email,
            Role: request.Role,
            ExpiresAt: expiresAt);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new BadRequestException("Email and password are required.");
        }

        var (success, user, roles) = await _identityService.CheckPasswordAndGetRolesAsync(
            request.Email.Trim(),
            request.Password,
            cancellationToken);

        if (!success || user is null)
        {
            throw new BadRequestException("Invalid email or password.");
        }

        var primaryRole = roles.FirstOrDefault() ?? Roles.Candidate;
        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user, roles);

        return new AuthResponseDto(
            Token: token,
            UserId: user.Id,
            Email: user.Email ?? request.Email,
            Role: primaryRole,
            ExpiresAt: expiresAt);
    }
}
