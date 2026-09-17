# Day 2 — Finalizing API Documentation (Swagger/OpenAPI & Postman)

**Program:** BinX Tech — Backend Development Internship (.NET), Phase 3, Sprint 4, Week 9
**Project:** CardioTrack API
**Hours:** 8
**Tools used:** Swashbuckle.AspNetCore, Postman

---

## Objectives

- Enrich Swagger/OpenAPI documentation with meaningful XML doc comments
- Document realistic request and response examples for every endpoint
- Write a complete, professional README

---

## What was done

### 1. Enabled XML documentation output

Added to `CardioTrack/CardioTrack.csproj`:

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```

Wired the generated XML file into Swagger in `Program.cs` via
`options.IncludeXmlComments(...)`, with `includeControllerXmlComments: true`
so controller-level summaries show up as group descriptions, not just
action-level ones.

### 2. Added XML doc comments to the 5 highest-traffic endpoints

- `POST /api/login` — auth entry point
- `POST /api/token/refresh-token` — token rotation
- `POST /api/DoctorOrNurse/add-vitalsign` — vital signs + automatic alerting
- `POST /api/Doctor` (create lab request) — lab ordering workflow
- `POST /api/admin/add-doctor` — staff account creation

Each includes a `<summary>`, a `<remarks>` block with a sample JSON request,
and `<response>` tags for every status code the endpoint can actually
return (`200`, `400`, `401`, `403` as applicable).

### 3. Added realistic request/response examples in Swagger UI

Added `<example>` tags directly on DTO properties for simple cases
(`LoginRequestDto`, `AddDoctorRequestDto`, `CreateLabRequestDto`). For DTOs
where a full object-shaped example was needed (arrays, nested response
objects), added a custom `ISchemaFilter`
(`Swagger/ExampleSchemaFilter.cs`) covering:

- `LoginRequestDto` / `LoginResponseDto`
- `CreateLabRequestDto` / `LabRequestResponseDto`

### 4. Reviewed the Postman collection

Went through all 60 endpoints against a checklist covering:

- Every endpoint present and correctly grouped into folders
- Collection-level bearer auth (`{{accessToken}}`) with per-request
  "inherit from parent," except the 3 auth endpoints (no auth)
- Environment variables (`baseUrl`, `accessToken`, `refreshToken`,
  `patientId`, `labRequestId`)
- At least one test script per request — status code assertion as the
  baseline, plus specific assertions where meaningful (e.g. login stores
  tokens, patient list stores an ID for later requests)
- A dedicated "Negative Tests" folder: no-token → 401, wrong role → 403,
  wrong password → 403, logout-then-refresh → 401

### 5. Wrote the project README

Covers: prerequisites, setup from scratch, `dotnet user-secrets`
configuration for every required secret, the full environment variable
table, migrations commands, the authorization policy table, how to run the
tests, the CI badge, and the project's folder structure.

---

## Deliverables

| File | Description |
|---|---|
| `README.md` | Full project README (setup, secrets, migrations, policies, structure) |
| `docs/Lab2-Documentation-Guide.md` | Ready-to-paste XML doc comments, schema filter code, and the exact `.csproj`/`Program.cs` changes |
| `docs/Postman-Review-Checklist.md` | Full 60-endpoint checklist, folder structure, and test scripts |

---

## Status

| Item | Status |
|---|---|
| XML docs enabled + 5 endpoints documented |  Code provided — confirm applied to the actual project |
| Request/response examples on ≥3 endpoints |  Code provided — confirm applied to the actual project |
| Postman collection fully reviewed |  Checklist provided — confirm items actually checked off |
| README complete |  Done |

## Open items

- Confirm the XML doc comments and schema filter were actually pasted into
  the live codebase (the guide only provides the code — it doesn't apply
  itself)
- Finish the manual Postman review pass using the checklist
- Once deployment is live, add the deployed Swagger link (or an exported
  OpenAPI JSON) to the README's docs section