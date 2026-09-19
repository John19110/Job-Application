namespace JobPlatform.Domain.Enums;

public static class Roles
{
    public const string Recruiter = "Recruiter";
    public const string Candidate = "Candidate";

    public static readonly IReadOnlyList<string> All = new[] { Recruiter, Candidate };

    public static bool IsValidRole(string? role) =>
        !string.IsNullOrWhiteSpace(role) && All.Contains(role);
}
