namespace JobPlatform.Application.DTOs.Auth;

public record RegisterRequestDto(string Email, string Password, string Role);

public record LoginRequestDto(string Email, string Password);

public record AuthResponseDto(
    string Token,
    string UserId,
    string Email,
    string Role,
    DateTime ExpiresAt);
