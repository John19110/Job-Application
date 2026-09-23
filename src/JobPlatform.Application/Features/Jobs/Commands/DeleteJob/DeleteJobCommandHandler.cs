using MediatR;
using JobPlatform.Application.Interfaces;

namespace JobPlatform.Application.Features.Jobs.Commands.DeleteJob;

public class DeleteJobCommandHandler : IRequestHandler<DeleteJobCommand, Unit>
{
    private readonly IJobService _jobService;

    public DeleteJobCommandHandler(IJobService jobService)
    {
        _jobService = jobService;
    }

    public async Task<Unit> Handle(DeleteJobCommand request, CancellationToken cancellationToken)
    {
        await _jobService.DeleteJobAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}

