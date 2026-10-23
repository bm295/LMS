from threading import Lock
from uuid import UUID, uuid4

from .domain import calculate_deadline
from .models import CourseSummary, EnrollRequest, EnrollResponse


class AlreadyEnrolled(Exception):
    pass


class EnrollmentRepository:
    """Process-local storage, matching the previous .NET in-memory repository."""

    def __init__(self):
        self._items: dict[tuple[UUID, UUID], EnrollResponse] = {}
        self._lock = Lock()

    def enroll(self, course_id: UUID, request: EnrollRequest) -> EnrollResponse:
        with self._lock:
            key = (request.user_id, course_id)
            if key in self._items:
                raise AlreadyEnrolled("The learner is already enrolled in this course.")
            result = EnrollResponse(
                enrollment_id=uuid4(), user_id=request.user_id, course_id=course_id,
                start_date=request.start_date,
                deadline=calculate_deadline(request.start_date, request.standard_days, request.has_extension),
                has_extension=request.has_extension,
            )
            self._items[key] = result
            return result


def published_courses() -> list[CourseSummary]:
    organization = "b1f50a35-ef8f-4ab7-a615-6979076dc97b"
    courses = [
        CourseSummary(id="75ed4c5d-113d-46a1-bf84-5322b8c82aa1", organization_id=organization,
            title="LMS Foundations",
            description="A guided introduction to the platform, roles, and learning workflows.",
            status="Published", lessons=[
                {"id": "57035ea6-0997-495a-86e9-2d89cb124cd7", "title": "Navigate the learner workspace", "sequence": 1},
                {"id": "a595fda1-2654-41a5-98f3-5ac53433542c", "title": "Complete a course module", "sequence": 2},
            ]),
        CourseSummary(id="af09f31e-a3ff-4da8-a52d-80e5d4e58f25", organization_id=organization,
            title="Course Authoring Essentials",
            description="Learn how instructors structure content, publish lessons, and prepare assessments.",
            status="Published", lessons=[
                {"id": "4a06931c-62a1-423e-aa87-149557a0c2a1", "title": "Design measurable learning outcomes", "sequence": 1},
                {"id": "78e8920e-e3ce-4956-9f62-6bfadf00a915", "title": "Publish a course safely", "sequence": 2},
            ]),
    ]
    return sorted(courses, key=lambda course: course.title)
