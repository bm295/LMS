using LMS.Application.Courses;
using LMS.Application.Enrollments;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers;

[ApiController]
[Route("api/courses")]
public sealed class CoursesController(
    ICourseApplicationService courseApplicationService,
    IEnrollmentApplicationService enrollmentApplicationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CourseSummary>>> Get(CancellationToken cancellationToken)
    {
        var courses = await courseApplicationService.ListPublishedAsync(cancellationToken);

        return Ok(courses);
    }

    [HttpPost("{courseId:guid}/enrollments")]
    public async Task<ActionResult<EnrollLearnerResponse>> Enroll(
        Guid courseId,
        [FromBody] EnrollRequest request,
        CancellationToken cancellationToken)
    {
        var result = await enrollmentApplicationService.EnrollAsync(
            courseId,
            request.UserId,
            request.StartDate,
            request.StandardDays,
            request.HasExtension,
            cancellationToken);

        return Created($"/api/courses/{courseId}/enrollments/{result.EnrollmentId}", result);
    }

    public sealed record EnrollRequest(
        Guid UserId,
        DateTime StartDate,
        int StandardDays,
        bool HasExtension = false);
}
