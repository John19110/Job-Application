using JobPlatform.Application.DTOs.Jobs;
using MediatR;
using JobPlatform.Application.Features.Jobs.Commands.CreateJob;
using JobPlatform.Application.Features.Jobs.Commands.DeleteJob;
using JobPlatform.Application.Features.Jobs.Queries.GetJobs;
using JobPlatform.Application.Features.Jobs.Queries.GetJobById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<JobResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var jobs = await _mediator.Send(new GetJobsQuery(), cancellationToken);
        return Ok(jobs);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(JobResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var job = await _mediator.Send(new GetJobByIdQuery(id), cancellationToken);
        return Ok(job);
    }

    [HttpPost]
    [Authorize(Roles = "Recruiter")]
    [ProducesResponseType(typeof(JobResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreateJobRequestDto request, CancellationToken cancellationToken)
    {
        var job = await _mediator.Send(new CreateJobCommand(request.Title, request.Description), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Recruiter")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteJobCommand(id), cancellationToken);
        return NoContent();
    }
}
using JobPlatform.Application.DTOs.Jobs;
using JobPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<JobResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var jobs = await _jobService.GetAllJobsAsync(cancellationToken);
        return Ok(jobs);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(JobResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var job = await _jobService.GetJobByIdAsync(id, cancellationToken);
        return Ok(job);
    }

    [HttpPost]
    [Authorize(Roles = "Recruiter")]
    [ProducesResponseType(typeof(JobResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreateJobRequestDto request, CancellationToken cancellationToken)
    {
        var job = await _jobService.CreateJobAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Recruiter")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _jobService.DeleteJobAsync(id, cancellationToken);
        return NoContent();
    }
}
