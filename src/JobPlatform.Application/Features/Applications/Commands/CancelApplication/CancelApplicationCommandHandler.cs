using MediatR;
using JobPlatform.Application.Interfaces;

namespace JobPlatform.Application.Features.Applications.Commands.CancelApplication;

public class CancelApplicationCommandHandler : IRequestHandler<CancelApplicationCommand, Unit>
{
    private readonly IApplicationService _applicationService;

    public CancelApplicationCommandHandler(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    public async Task<Unit> Handle(CancelApplicationCommand request, CancellationToken cancellationToken)
    {
        await _applicationService.CancelApplicationAsync(request.JobId, cancellationToken);
        return Unit.Value;
    }
}

