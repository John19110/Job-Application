using JobPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobPlatform.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Job> Jobs { get; }
    DbSet<JobApplication> Applications { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
