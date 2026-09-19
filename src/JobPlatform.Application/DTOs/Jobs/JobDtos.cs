namespace JobPlatform.Application.DTOs.Jobs;

public record CreateJobRequestDto(string Title, string Description);

public record JobResponseDto(
    Guid Id,
    string Title,
    string Description,
    string RecruiterId,
    DateTime CreatedAt);
