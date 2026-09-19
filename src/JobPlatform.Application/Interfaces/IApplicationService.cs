using JobPlatform.Application.DTOs.Applications;

namespace JobPlatform.Application.Interfaces;

public interface IApplicationService
{
    Task<JobApplicationResponseDto> ApplyAsync(Guid jobId, CancellationToken cancellationToken = default);
    Task CancelApplicationAsync(Guid jobId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobApplicationResponseDto>> GetMyApplicationsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobApplicationResponseDto>> GetApplicationsForJobAsync(Guid jobId, CancellationToken cancellationToken = default);
}
