# CardioTrack API

[![CI](https://github.com/hebaayaseh/BinX-Training-2026/actions/workflows/ci.yml/badge.svg)](https://github.com/hebaayaseh/BinX-Training-2026/actions/workflows/ci.yml)

> Replace `<YOUR-USERNAME>/<YOUR-REPO>` with your actual GitHub path or the badge
> will render as "not found".

A cardiac patient tracking API for a clinic. It manages patients, appointments,
medications, medical history, vital signs with automatic alerting, and the lab
request / lab result workflow, across five roles: Admin, Doctor, Nurse,
Technician and Patient.

---

## Tech stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 (Web API) |
| ORM | Entity Framework Core 8 + Pomelo MySQL provider |
| Database | MySQL 8 |
| Cache | Redis (via `StackExchangeRedis` distributed cache) |
| Auth | JWT Bearer + refresh token rotation, BCrypt password hashing |
| Validation | FluentValidation |
| Logging | Serilog |
| API docs | Swashbuckle / Swagger |
| Testing | xUnit, Moq, WebApplicationFactory, EF Core InMemory |
| CI | GitHub Actions |

---


## Roles and authorization policies

| Policy | Roles allowed |
|---|---|
| `AdminOnly` | Admin |
| `DoctorOnly` | Doctor |
| `NurseOnly` | Nurse |
| `TechnicianOnly` | Technician |
| `DoctorOrNurse` | Doctor, Nurse |
| `PatientOnly` | Patient |
| `MedicalStaff` | Admin, Doctor, Nurse, Technician |
| `LabViewer` | Patient, Technician, Doctor |
| `AllActors` | Admin, Doctor, Nurse, Patient |

Auth flow: `POST /api/login` returns an access token (30 min) and a refresh
token (7 days). Use `POST /api/token/refresh-token` to rotate — the old refresh
token is revoked on use. `POST /api/token/logout` revokes it immediately.

---

## Documentation

- **Swagger UI:** run in Development and open `/swagger`
- **OpenAPI JSON:** `/swagger/v1/swagger.json`
- **Postman collection:** [`CardioTrack Copy.postman_collection.json`](./CardioTrack%20Copy.postman_collection.json)
- **Test coverage audit:** [`docs/Test-Coverage-Audit.md`](./docs/Test-Coverage-Audit.md)

### Using the Postman collection

1. Import the collection.
2. Create an environment with `baseUrl`, `accessToken` and `refreshToken` variables.
3. Run **Auth → Login** first; its test script stores the tokens automatically.
4. Every other request inherits the bearer token from the collection.

---

## Running the tests

```bash
dotnet test CardioTrack.sln
```

The test suite needs **no MySQL and no Redis** — unit tests use EF Core InMemory
and the integration tests use `WebApplicationFactory` with an in-memory database
seeded by `CustomWebApplicationFactory`.

With coverage:

```bash
dotnet test CardioTrack.sln --collect:"XPlat Code Coverage"
```

---

## CI

`.github/workflows/ci.yml` runs on every push and pull request to `main`,
`master` and `develop`. It checks out the code, sets up .NET 8, restores,
builds in Release, runs the full test suite, and uploads the `.trx` results and
coverage report as artifacts. A failing test fails the workflow and the badge
above turns red.

---

## Project structure

```
CardioTrack/
├── Controllers/       grouped by feature
├── Services/          business logic, one folder per area
├── Interfaces/        service contracts (used for mocking in tests)
├── DTOs/              request/response models
├── Models/            EF Core entities
├── Validators/        FluentValidation rules
├── Middleware/        global exception handling -> ProblemDetails
├── ExceptionService/  typed exceptions carrying HTTP status codes
├── VitalSignsAlert/   vital-sign threshold evaluation
├── Data/              DbContext + seeding
└── Migrations/

CardioTrack.Tests/
├── Services/          unit tests (xUnit + Moq + InMemory)
├── Controllers/       controller-level tests
└── Integration/       WebApplicationFactory end-to-end tests
```

---

## Error format

All unhandled errors pass through `ExceptionMiddleware` and come back as
RFC 7807 `application/problem+json`:

```json
{
  "type": "https://httpstatuses.com/400",
  "title": "Bad Request",
  "status": 400,
  "detail": "Patient not found",
  "instance": "/api/Doctor/add-Medication",
  "traceId": "0HN7A3F1K9LMN:00000001"
}
```

Internal exception details are never returned to the client — unexpected errors
become a generic 500 message and are written to the Serilog log with the trace id.