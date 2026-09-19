namespace JobPlatform.Domain.Entities;

public class Job
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RecruiterId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ApplicationUser? Recruiter { get; set; }
    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
}
