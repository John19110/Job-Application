using MediatR;
using JobPlatform.Application.DTOs.Applications;
using JobPlatform.Application.Interfaces;

namespace JobPlatform.Application.Features.Applications.Queries.GetMyApplications;

public class GetMyApplicationsQueryHandler : IRequestHandler<GetMyApplicationsQuery, IReadOnlyList<JobApplicationResponseDto>>
{
    private readonly IApplicationService _applicationService;

    public GetMyApplicationsQueryHandler(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    public async Task<IReadOnlyList<JobApplicationResponseDto>> Handle(GetMyApplicationsQuery request, CancellationToken cancellationToken)
    {
        return await _applicationService.GetMyApplicationsAsync(cancellationToken);
    }
}

