using LMS.Application.Courses;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers;

[ApiController]
[Route("api/courses")]
public sealed class CoursesController(IListPublishedCoursesUseCase listPublishedCoursesUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CourseSummary>>> Get(CancellationToken cancellationToken)
    {
        var courses = await listPublishedCoursesUseCase.ExecuteAsync(cancellationToken);

        return Ok(courses);
    }
}
