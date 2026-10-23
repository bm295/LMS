# AI-Native LMS

This repository has been reorganized around Clean Architecture so the LMS domain and workflows are isolated from delivery and infrastructure concerns.

## Solution layout

```text
LMS.Domain/          Enterprise business rules: entities, value rules, invariants.
LMS.Application/     Legacy .NET configuration constants.
LMS.Infrastructure/  Legacy .NET library shell (API adapters removed).
python-api/          Python FastAPI REST API (course catalog and enrollments).
WebApplication/      React SPA and optional ASP.NET Core static presentation host.
docs/                Product, architecture, security, operations, and agent workflow docs.
```

## Runtime architecture

`python-api/` owns HTTP contracts, enrollment rules, and in-memory data for the
migrated APIs. It runs independently of .NET. React uses the same `/api` URLs
through a development or production reverse proxy. The .NET domain library is
retained for existing non-HTTP domain functionality and domain tests.

## Current runtime

Run the web application with:

```bash
dotnet run --project WebApplication/WebApplication.csproj
```

The REST API now runs independently in Python:

```bash
cd python-api
python -m venv .venv
. .venv/bin/activate
pip install -e '.[test]'
uvicorn lms_api.main:app --host 0.0.0.0 --port 8000
```

For frontend development, run `npm install` and `npm start` in
`WebApplication/ClientApp`; the development server proxies API requests to port 8000.
For deployment, route `/api/*` to Python and other paths to the frontend using a
reverse proxy. The optional .NET static host no longer serves LMS APIs.
See [Python API documentation](python-api/README.md) for testing and limitations.

## LMS goal

Continue evolving the system toward:

- Multi-tenant organizations
- Role-based administration
- Course authoring and publishing
- Assessments, grading, and progress tracking
- Reporting, notifications, and auditability

## Start here for product execution

Read the document execution order first:

- `docs/00_AI_EXECUTION_ORDER.md`

Then execute each document in sequence (`01` to `13`) to drive planning, architecture, implementation, and operations.

