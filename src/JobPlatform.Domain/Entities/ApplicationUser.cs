using Microsoft.AspNetCore.Identity;

namespace JobPlatform.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public ICollection<Job> PostedJobs { get; set; } = new List<Job>();
    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
}
