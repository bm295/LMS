# System Architecture — LMS Rewrite

## Implemented architecture

The repository now follows Clean Architecture as a modular monolith foundation for the LMS MVP.

```text
WebApplication -> LMS.Application -> LMS.Domain
WebApplication -> LMS.Infrastructure -> LMS.Application -> LMS.Domain
```

- `LMS.Domain`: enterprise business rules, entities, invariants, and vocabulary.
- `LMS.Application`: application use cases, DTOs, and ports that describe required persistence or integration capabilities.
- `LMS.Infrastructure`: adapters that implement application ports for persistence and external systems.
- `WebApplication`: ASP.NET Core composition root, HTTP API controllers, static assets, and React SPA.

## Dependency rule

Source dependencies must point inward toward the domain. The domain layer must not reference ASP.NET Core, React, databases, messaging providers, or infrastructure packages. Application code may reference domain code and define ports, but infrastructure owns concrete adapter implementations.

## Recommended runtime model

- Modular monolith for MVP with clear internal domain modules.
- API-first backend with REST endpoints initially; GraphQL can be evaluated later.
- React SPA presentation hosted by ASP.NET Core.
- Infrastructure adapters can evolve from in-memory repositories to a relational database, object storage, notifications, and audit pipelines without changing domain rules.

## Backend modules

1. Identity & Access
2. Course Authoring
3. Enrollment & Cohorts
4. Assessment & Grading
5. Progress & Analytics
6. Notifications
7. Audit & Compliance

## Cross-cutting concerns

- Authorization policy engine
- Validation and idempotency
- Audit log pipeline
- Observability (traces, metrics, logs)
- Background jobs for notifications and reporting

## Data and integration boundaries

- Single primary relational database for MVP
- Object storage abstraction for assignment attachments
- Email provider abstraction

## Architecture decisions (ADRs to create)

- ADR-001: Multi-tenancy model
- ADR-002: Grading event-sourcing or mutable ledger
- ADR-003: Notification delivery strategy

## Change log

- v1.1: Repository reorganized into Clean Architecture layers.
- v1.0: Initial architecture target.
