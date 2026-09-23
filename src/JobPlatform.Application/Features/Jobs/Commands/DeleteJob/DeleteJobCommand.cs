using MediatR;

namespace JobPlatform.Application.Features.Jobs.Commands.DeleteJob;

public record DeleteJobCommand(Guid Id) : IRequest;

