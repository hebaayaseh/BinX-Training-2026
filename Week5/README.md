# Week 5 – TESTING - ERROR HANDLING & PROJECT KICKOFF

### Scope
CardioTrack backend API — ASP.NET Core Web API supporting 4 roles (Admin, Doctor, 
Nurse, Patient), covering patient management, vital sign recording with automatic 
alert generation, medication and appointment management, and medical history tracking.

### Highest-Risk Logic Identified & Tested

1. **Vital Sign Alert Evaluation** — Incorrect threshold logic could mean a patient 
   in a critical condition receives no alert at all. Covered with unit tests for 
   every vital sign type (heart rate, blood pressure, oxygen saturation, 
   temperature), including exact boundary values.

2. **Doctor-Patient Ownership Authorization** — A flaw here would let a doctor 
   access or modify another doctor's patient records, a serious privacy violation 
   in a medical system. Covered with dedicated ownership tests for medication 
   management and medical history.

3. **Appointment Conflict Detection** — Real business logic (not simple CRUD) that 
   prevents double-booking the same doctor at the same time slot. Covered with 
   tests for both the conflicting and non-conflicting scheduling paths.

### Test Suite Structure

**Unit Tests** (`CardioTrack.Tests.Services`, `CardioTrack.Tests.Controllers`)
- `VitalSignAlertEvaluatorTests` — Pure-function tests for threshold logic across 
  all four vital sign types, covering normal, medium, and critical severities, 
  plus exact boundary cases.
- `AppointmentServiceTests` — Success and failure paths for appointment creation, 
  cancellation, and completion, including doctor-double-booking prevention.
- `AuthorizationBoundaryTests` — Confirms doctors cannot manage medications or 
  medical history for patients outside their assigned care.
- `MedicalHistoryControllerTests` — Controller-level tests using Moq to isolate 
  from the Service and Validator layers, covering both valid and invalid requests.

**Integration Tests** (`CardioTrack.Tests.Integration`)
- `PatientEndpointsIntegrationTests` — Full request pipeline tests using 
  `WebApplicationFactory` and an InMemory database, covering the authenticated 
  happy path, missing-token rejection, and not-found error path for the primary 
  patient resource.
- `ExceptionHandlingIntegrationTests` — Confirms the global exception middleware 
  intercepts unhandled exceptions and returns a `ProblemDetails` response without 
  leaking internal exception details to the client.

### Error Handling Setup
A centralized `ExceptionMiddleware` intercepts all unhandled exceptions across the 
application and converts them into standardized `ProblemDetails` (RFC 7807) 
responses. A custom exception hierarchy (`AppException` and derived types such as 
`NotFoundException`, `BadRequestException`, `ForbiddenException`) allows each 
failure type to carry its own appropriate HTTP status code. Full exception details 
are logged via `ILogger` with request path and method context, while the client 
only ever receives a safe, generic message for unexpected errors — no stack traces 
are exposed.





