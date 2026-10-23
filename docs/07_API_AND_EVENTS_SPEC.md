# API and Event Specifications

## Implemented runtime (Python)

The `python-api/` FastAPI project owns these implemented routes:

- `GET /api/courses`: published course summaries, ordered by title, with lessons ordered by sequence.
- `POST /api/courses/{courseId}/enrollments`: enroll a learner; returns 201 and a Location header.

The previous .NET controller, API application services, and in-memory adapters have been removed.
See `python-api/README.md` for contracts, validation, and runtime instructions.
The `/api/v1` endpoints below are planned contracts, not implemented routes.

## Planned API conventions

- Versioned base path: `/api/v1`
- JSON request/response contracts
- Problem Details error format
- Cursor pagination for list endpoints

## Core endpoints (MVP)

### Auth
- POST `/auth/register`
- POST `/auth/login`
- POST `/auth/forgot-password`

### Courses
- GET `/courses`
- POST `/courses`
- GET `/courses/{courseId}`
- PATCH `/courses/{courseId}`
- POST `/courses/{courseId}/publish`

### Enrollment
- POST `/courses/{courseId}/enrollments`
- GET `/courses/{courseId}/enrollments`

### Assessments
- POST `/courses/{courseId}/assessments`
- POST `/assessments/{assessmentId}/submissions`
- POST `/submissions/{submissionId}/grade`

### Progress/analytics
- GET `/courses/{courseId}/progress`
- GET `/courses/{courseId}/gradebook`

## Domain events (internal)

- CoursePublished
- EnrollmentCreated
- SubmissionCreated
- SubmissionGraded
- LearnerAtRiskDetected

## Event envelope

- event_id
- event_type
- organization_id
- occurred_at_utc
- actor_user_id
- payload (JSON)

## Change log

- v1.0: Initial API and event contract definitions.

