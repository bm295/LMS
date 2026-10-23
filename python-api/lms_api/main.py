from uuid import UUID

from fastapi import FastAPI, HTTPException, Response

from .models import CourseSummary, EnrollRequest, EnrollResponse
from .repository import AlreadyEnrolled, EnrollmentRepository, published_courses


def create_app() -> FastAPI:
    app = FastAPI(title="LMS REST API", version="0.1.0")
    repository = EnrollmentRepository()

    @app.get("/api/courses", response_model=list[CourseSummary])
    async def list_courses():
        return published_courses()

    @app.post("/api/courses/{course_id}/enrollments", response_model=EnrollResponse, status_code=201)
    async def enroll(course_id: UUID, request: EnrollRequest, response: Response):
        if course_id.int == 0:
            raise HTTPException(400, "Course id must be provided.")
        try:
            result = repository.enroll(course_id, request)
        except AlreadyEnrolled as error:
            raise HTTPException(409, str(error)) from error
        except (OverflowError, ValueError) as error:
            raise HTTPException(400, "Deadline is outside the supported date range.") from error
        response.headers["Location"] = f"/api/courses/{course_id}/enrollments/{result.enrollment_id}"
        return result

    return app


app = create_app()
