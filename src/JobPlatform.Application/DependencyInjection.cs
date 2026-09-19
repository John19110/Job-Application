using JobPlatform.Application.Interfaces;
using JobPlatform.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JobPlatform.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJobService, JobService>();
        services.AddScoped<IApplicationService, ApplicationService>();

        return services;
    }
}
