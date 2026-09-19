using JobPlatform.Application.DTOs.Jobs;

namespace JobPlatform.Application.Interfaces;

public interface IJobService
{
    Task<JobResponseDto> CreateJobAsync(CreateJobRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteJobAsync(Guid id, CancellationToken cancellationToken = default);
    Task<JobResponseDto> GetJobByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobResponseDto>> GetAllJobsAsync(CancellationToken cancellationToken = default);
}
