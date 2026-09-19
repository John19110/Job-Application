using JobPlatform.Application.Common.Interfaces;
using JobPlatform.Application.DTOs.Applications;
using JobPlatform.Application.Interfaces;
using JobPlatform.Domain.Entities;
using JobPlatform.Domain.Enums;
using JobPlatform.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace JobPlatform.Application.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ApplicationService(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<JobApplicationResponseDto> ApplyAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        var candidateId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(candidateId))
        {
            throw new ForbiddenException("Authenticated candidate ID is missing from user claims.");
        }

        // Verify the job exists
        var jobExists = await _context.Jobs
            .AnyAsync(j => j.Id == jobId, cancellationToken);

        if (!jobExists)
        {
            throw new NotFoundException("Job", jobId);
        }

        // Check for existing application
        var existingApplication = await _context.Applications
            .FirstOrDefaultAsync(a => a.JobId == jobId && a.CandidateId == candidateId, cancellationToken);

        if (existingApplication != null)
        {
            if (existingApplication.Status == ApplicationStatus.Applied)
            {
                throw new ConflictException("You have already applied for this job.");
            }

            // Re-activate previously cancelled application
            existingApplication.Status = ApplicationStatus.Applied;
            existingApplication.AppliedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new JobApplicationResponseDto(
                Id: existingApplication.Id,
                JobId: existingApplication.JobId,
                CandidateId: existingApplication.CandidateId,
                Status: existingApplication.Status.ToString(),
                AppliedAt: existingApplication.AppliedAt);
        }

        var application = new JobApplication
        {
            Id = Guid.NewGuid(),
            JobId = jobId,
            CandidateId = candidateId,
            AppliedAt = DateTime.UtcNow,
            Status = ApplicationStatus.Applied
        };

        _context.Applications.Add(application);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            // Catches any database-level unique constraint violation (CandidateId + JobId)
            throw new ConflictException("You have already applied for this job.");
        }

        return new JobApplicationResponseDto(
            Id: application.Id,
            JobId: application.JobId,
            CandidateId: application.CandidateId,
            Status: application.Status.ToString(),
            AppliedAt: application.AppliedAt);
    }

    public async Task CancelApplicationAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        var candidateId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(candidateId))
        {
            throw new ForbiddenException("Authenticated candidate ID is missing from user claims.");
        }

        var jobExists = await _context.Jobs
            .AnyAsync(j => j.Id == jobId, cancellationToken);

        if (!jobExists)
        {
            throw new NotFoundException("Job", jobId);
        }

        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.JobId == jobId && a.CandidateId == candidateId, cancellationToken);

        if (application is null)
        {
            throw new NotFoundException("Application not found for the specified job and candidate.");
        }

        // Enforce Candidate ownership check at the application layer
        if (application.CandidateId != candidateId)
        {
            throw new ForbiddenException("You can only cancel your own application.");
        }

        application.Status = ApplicationStatus.Cancelled;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<JobApplicationResponseDto>> GetMyApplicationsAsync(CancellationToken cancellationToken = default)
    {
        var candidateId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(candidateId))
        {
            throw new ForbiddenException("Authenticated candidate ID is missing.");
        }

        return await _context.Applications
            .AsNoTracking()
            .Where(a => a.CandidateId == candidateId)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new JobApplicationResponseDto(
                a.Id,
                a.JobId,
                a.CandidateId,
                a.Status.ToString(),
                a.AppliedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<JobApplicationResponseDto>> GetApplicationsForJobAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUserService.UserId;
        var job = await _context.Jobs
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.Id == jobId, cancellationToken);

        if (job is null)
        {
            throw new NotFoundException("Job", jobId);
        }

        // Only the recruiter who created the job can view its applications
        if (job.RecruiterId != currentUserId)
        {
            throw new ForbiddenException("You are not authorized to view applications for this job.");
        }

        return await _context.Applications
            .AsNoTracking()
            .Where(a => a.JobId == jobId)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new JobApplicationResponseDto(
                a.Id,
                a.JobId,
                a.CandidateId,
                a.Status.ToString(),
                a.AppliedAt))
            .ToListAsync(cancellationToken);
    }
}
