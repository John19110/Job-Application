namespace JobPlatform.Application.DTOs.Applications;

public record JobApplicationResponseDto(
    Guid Id,
    Guid JobId,
    string CandidateId,
    string Status,
    DateTime AppliedAt);
