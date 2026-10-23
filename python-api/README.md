# LMS Python REST API

Requires Python 3.11+. From this directory:

```bash
python -m venv .venv
. .venv/bin/activate
pip install -e '.[test]'
uvicorn lms_api.main:app --host 0.0.0.0 --port 8000
```

Interactive API documentation: http://localhost:8000/docs
OpenAPI schema: http://localhost:8000/openapi.json

## Migrated endpoints

- `GET /api/courses`: returns the original two published seed courses, sorted by
  title, with lessons sorted by sequence. All fields remain camelCase.
- `POST /api/courses/{courseId}/enrollments`: returns HTTP 201 with `Location`
  and `enrollmentId`, `userId`, `courseId`, `startDate`, `deadline`, `hasExtension`.

Example body:

```json
{
  "userId": "478ecfe1-7da4-4c88-b892-99913f42b810",
  "startDate": "2026-10-23T09:30:00",
  "standardDays": 5,
  "hasExtension": false
}
```

The deadline counts weekdays after the start date, preserving the time and any
supplied UTC offset. An extension adds seven weekdays. IDs must be nonzero UUIDs;
standardDays must be a positive 32-bit integer. Duplicate learner/course pairs
return 409; invalid request fields return FastAPI's 422 validation response;
an empty course UUID or unrepresentable deadline returns 400. These explicit
errors replace unhandled .NET domain exceptions. No authentication is required,
as with the original controller. Course existence is not checked by enrollment,
matching the original use case.

Storage remains process-local and in memory. Restarting clears enrollments;
use one worker. Durable storage and cross-worker coordination are future work.

## Frontend

The React development server proxies `/api` to `http://localhost:8000`.
For production use a reverse proxy with `/api/*` routed to Python and all other
paths to the built SPA/static host. Keep requests on the same origin.
The .NET host returns 404 for `/api/*` to prevent SPA fallback from masking a
missing Python routing rule. The .NET domain library and its domain tests remain;
the old controller, API use cases, repositories, and corresponding tests are removed.

## Tests

```bash
python -m unittest discover -s tests -v
```

Tests cover response contracts, seed order, creation and Location, duplicate
handling, validation, date overflow, and weekday/extension parity with the
original algorithm across all seven start weekdays.
