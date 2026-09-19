using JobPlatform.Application.Common.Interfaces;
using JobPlatform.Application.DTOs.Jobs;
using JobPlatform.Application.Interfaces;
using JobPlatform.Domain.Entities;
using JobPlatform.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace JobPlatform.Application.Services;

public class JobService : IJobService
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public JobService(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<JobResponseDto> CreateJobAsync(CreateJobRequestDto request, CancellationToken cancellationToken = default)
    {
        var recruiterId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(recruiterId))
        {
            throw new ForbiddenException("Authenticated recruiter ID is missing from user claims.");
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new BadRequestException("Job title is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            throw new BadRequestException("Job description is required.");
        }

        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            RecruiterId = recruiterId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Jobs.Add(job);
        await _context.SaveChangesAsync(cancellationToken);

        return new JobResponseDto(
            Id: job.Id,
            Title: job.Title,
            Description: job.Description,
            RecruiterId: job.RecruiterId,
            CreatedAt: job.CreatedAt);
    }

    public async Task DeleteJobAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            throw new ForbiddenException("Authenticated user ID is missing.");
        }

        var job = await _context.Jobs
            .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);

        if (job is null)
        {
            throw new NotFoundException("Job", id);
        }

        // Enforce Recruiter ownership check at the application layer
        if (job.RecruiterId != currentUserId)
        {
            throw new ForbiddenException("You can only delete jobs that you have created.");
        }

        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<JobResponseDto> GetJobByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var job = await _context.Jobs
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);

        if (job is null)
        {
            throw new NotFoundException("Job", id);
        }

        return new JobResponseDto(
            Id: job.Id,
            Title: job.Title,
            Description: job.Description,
            RecruiterId: job.RecruiterId,
            CreatedAt: job.CreatedAt);
    }

    public async Task<IReadOnlyList<JobResponseDto>> GetAllJobsAsync(CancellationToken cancellationToken = default)
    {
        var jobs = await _context.Jobs
            .AsNoTracking()
            .OrderByDescending(j => j.CreatedAt)
            .Select(j => new JobResponseDto(
                j.Id,
                j.Title,
                j.Description,
                j.RecruiterId,
                j.CreatedAt))
            .ToListAsync(cancellationToken);

        return jobs;
    }
}
