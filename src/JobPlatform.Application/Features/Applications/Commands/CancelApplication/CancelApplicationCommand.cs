using MediatR;

namespace JobPlatform.Application.Features.Applications.Commands.CancelApplication;

public record CancelApplicationCommand(Guid JobId) : IRequest;

