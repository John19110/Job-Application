using JobPlatform.Application.DTOs.Applications;
using MediatR;
using JobPlatform.Application.Features.Applications.Commands.ApplyForJob;
using JobPlatform.Application.Features.Applications.Commands.CancelApplication;
using JobPlatform.Application.Features.Applications.Queries.GetApplicationsForJob;
using JobPlatform.Application.Features.Applications.Queries.GetMyApplications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPlatform.API.Controllers;

[ApiController]
public class JobApplicationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobApplicationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("api/jobs/{jobId:guid}/applications")]
    [Authorize(Roles = "Candidate")]
    [ProducesResponseType(typeof(JobApplicationResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Apply(Guid jobId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new ApplyForJobCommand(jobId), cancellationToken);
        return StatusCode(StatusCodes.Status201Created, application);
    }

    [HttpDelete("api/jobs/{jobId:guid}/applications")]
    [Authorize(Roles = "Candidate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CancelApplication(Guid jobId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelApplicationCommand(jobId), cancellationToken);
        return NoContent();
    }

    [HttpGet("api/jobs/{jobId:guid}/applications")]
    [Authorize(Roles = "Recruiter")]
    [ProducesResponseType(typeof(IReadOnlyList<JobApplicationResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetApplicationsForJob(Guid jobId, CancellationToken cancellationToken)
    {
        var applications = await _mediator.Send(new GetApplicationsForJobQuery(jobId), cancellationToken);
        return Ok(applications);
    }

    [HttpGet("api/applications/my")]
    [Authorize(Roles = "Candidate")]
    [ProducesResponseType(typeof(IReadOnlyList<JobApplicationResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMyApplications(CancellationToken cancellationToken)
    {
        var applications = await _mediator.Send(new GetMyApplicationsQuery(), cancellationToken);
        return Ok(applications);
    }
}
using JobPlatform.Application.DTOs.Applications;
using JobPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPlatform.API.Controllers;

[ApiController]
public class JobApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public JobApplicationsController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpPost("api/jobs/{jobId:guid}/applications")]
    [Authorize(Roles = "Candidate")]
    [ProducesResponseType(typeof(JobApplicationResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Apply(Guid jobId, CancellationToken cancellationToken)
    {
        var application = await _applicationService.ApplyAsync(jobId, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, application);
    }

    [HttpDelete("api/jobs/{jobId:guid}/applications")]
    [Authorize(Roles = "Candidate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CancelApplication(Guid jobId, CancellationToken cancellationToken)
    {
        await _applicationService.CancelApplicationAsync(jobId, cancellationToken);
        return NoContent();
    }

    [HttpGet("api/jobs/{jobId:guid}/applications")]
    [Authorize(Roles = "Recruiter")]
    [ProducesResponseType(typeof(IReadOnlyList<JobApplicationResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetApplicationsForJob(Guid jobId, CancellationToken cancellationToken)
    {
        var applications = await _applicationService.GetApplicationsForJobAsync(jobId, cancellationToken);
        return Ok(applications);
    }

    [HttpGet("api/applications/my")]
    [Authorize(Roles = "Candidate")]
    [ProducesResponseType(typeof(IReadOnlyList<JobApplicationResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMyApplications(CancellationToken cancellationToken)
    {
        var applications = await _applicationService.GetMyApplicationsAsync(cancellationToken);
        return Ok(applications);
    }
}
