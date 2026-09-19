namespace JobPlatform.Infrastructure.Authentication;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public string Secret { get; set; } = "DefaultDevelopmentSuperSecretKeyForJobApplicationPlatform12345!";
    public string Issuer { get; set; } = "JobPlatformAPI";
    public string Audience { get; set; } = "JobPlatformClients";
    public int ExpiryMinutes { get; set; } = 120;
}
