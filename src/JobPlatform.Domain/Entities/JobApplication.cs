using JobPlatform.Domain.Enums;

namespace JobPlatform.Domain.Entities;

public class JobApplication
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid JobId { get; set; }
    public string CandidateId { get; set; } = string.Empty;
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

    public Job? Job { get; set; }
    public ApplicationUser? Candidate { get; set; }
}
