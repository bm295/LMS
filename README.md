# AI-Native LMS

This repository has been reorganized around Clean Architecture so the LMS domain and workflows are isolated from delivery and infrastructure concerns.

## Solution layout

```text
LMS.Domain/          Enterprise business rules: entities, value rules, invariants.
LMS.Application/     Application business rules: use cases, DTOs, repository ports.
LMS.Infrastructure/  Adapters for external concerns such as persistence and integrations.
WebApplication/      ASP.NET Core API host and React SPA presentation layer.
docs/                Product, architecture, security, operations, and agent workflow docs.
```

## Dependency rule

Dependencies point inward only:

```text
WebApplication -> LMS.Application -> LMS.Domain
WebApplication -> LMS.Infrastructure -> LMS.Application -> LMS.Domain
```

The domain has no dependency on ASP.NET Core, React, persistence, or other external frameworks. Infrastructure implements application ports, and the web layer composes the application at startup.

## Current runtime

Run the web application with:

```bash
dotnet run --project WebApplication/WebApplication.csproj
```

The React app calls `GET /api/courses`, which is served by an application use case and an in-memory infrastructure repository.

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
