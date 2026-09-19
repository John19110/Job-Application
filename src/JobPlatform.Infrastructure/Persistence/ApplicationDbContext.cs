using JobPlatform.Application.Common.Interfaces;
using JobPlatform.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobPlatform.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<JobApplication> Applications => Set<JobApplication>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Job>(entity =>
        {
            entity.HasKey(j => j.Id);

            entity.Property(j => j.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(j => j.Description)
                .IsRequired()
                .HasMaxLength(4000);

            entity.Property(j => j.RecruiterId)
                .IsRequired();

            entity.Property(j => j.CreatedAt)
                .IsRequired();

            // Recruiter -> Jobs relationship
            entity.HasOne(j => j.Recruiter)
                .WithMany(u => u.PostedJobs)
                .HasForeignKey(j => j.RecruiterId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.CandidateId)
                .IsRequired();

            entity.Property(a => a.AppliedAt)
                .IsRequired();

            entity.Property(a => a.Status)
                .IsRequired();

            // Job -> Applications relationship
            entity.HasOne(a => a.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            // Candidate -> Applications relationship
            entity.HasOne(a => a.Candidate)
                .WithMany(u => u.Applications)
                .HasForeignKey(a => a.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

            // Database-level unique constraint to prevent duplicate applications
            entity.HasIndex(a => new { a.CandidateId, a.JobId })
                .IsUnique();
        });
    }
}
