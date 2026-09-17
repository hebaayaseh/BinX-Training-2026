# Week 9 — Sprint 4: Testing, Docs & Deployment

**Project:** CardioTrack API (`Week9/Day2/CardioTrack/`)
**Program:** BinX Tech — Backend Development Internship (.NET), Phase 3, Sprint 4
**Overall status:**  Testing, Docs, and CI complete —  Deploy blocked (see last section)

---

## Day 1 — Sprint 4 Planning & Closing Test Coverage Gaps

### Files
- `docs/Sprint4-Plan.md` — sprint goal, backlog (S4-01 → S4-19), and the
  Sprint 3 retro action carried forward
- `docs/Test-Coverage-Audit.md` — a full inventory of **60 endpoints**,
  each classified (Both / Happy only / Error only / None), prioritized by
  risk (P0 Authentication → P1 sensitive medical data → P2 role-protected
  → P3 everything else)
- 5 new test files closing the highest-priority gaps:
  - `CardioTrack.Tests/Integration/RoleProtectionIntegrationTests.cs`
  - `CardioTrack.Tests/Integration/AuthEndpointsIntegrationTests.cs`
  - `CardioTrack.Tests/Services/ManageLabRequestServiceTests.cs`
  - `CardioTrack.Tests/Services/AddStaffServiceTests.cs`
  - `CardioTrack.Tests/Services/ActiveDeactiveActorServiceTests.cs`

### Key finding
A real security bug in `AuthService.LoginAsync` — it wasn't checking
`user.IsActive`, meaning an account deactivated by an admin could still log
in. Documented and fixed.

### Test suite outcome
Went from **27 failing tests → 0** (except one intentionally-failing test
that documents the bug above as a regression guard). Root causes resolved
along the way:
- `User.PhoneNumber` had no default value (broke 11 pre-existing tests)
- Integration test database was accidentally shared, because
  `Guid.NewGuid()` was evaluated inside the lambda instead of outside it
  (caused conflicts + random 500s)
- Redis was registered even in the Testing environment (caused 500s on
  any cache-using endpoint)
- Overly strict exact-type exception assertions
  (`Assert.ThrowsAsync<Exception>` instead of the real type) and JWTs that
  collided when generated within the same second

---

## Day 2 — Finalizing API Documentation

### Files
- `docs/Lab2-Documentation-Guide.md` — ready-to-paste code: enabling XML
  docs, doc comments on the 5 most important endpoints, request/response
  examples on 3 endpoints via an `ExampleSchemaFilter`
- `docs/Postman-Review-Checklist.md` — a full checklist for all 60
  endpoints, a suggested folder structure, ready-made test scripts (login,
  negative tests, user-enumeration check)
- `README.md` (at the root of the CardioTrack project) — setup from
  scratch, user-secrets, every environment variable, migrations, policies,
  project structure

---

## Day 3 — Building the CI/CD Pipeline

### Files
- `.github/workflows/ci.yml` — automatic build + test, NuGet caching,
  uploads test results (`.trx` + coverage) as an artifact even on failure

### Verification
-  Normal push → green
-  Deliberately broken test → clearly red
-  Fixed it → back to green
-  The README badge still needs the correct link: `hebaayaseh/BinX-Training-2026`

---

## Day 4 — Deploying to Azure App Service / Railway

### Files
- `lab4-deploy/Dockerfile`, `lab4-deploy/.dockerignore` — a multi-stage
  .NET 8 build, works on both Railway and Azure Web App for Containers with
  no changes
- `lab4-deploy/Lab4-Deploy-Guide.md` — full Railway steps (root directory,
  MySQL/Redis add-ons, secrets via Variables), a `deploy` job added to
  `.github/workflows/ci.yml` gated on `needs: build-and-test` and the
  `main` branch, plus a short Azure alternative

### Actual status: blocked

Railway is asking for a payment card to continue before deployment can
finish. **This is still unresolved.** Options being evaluated (documented
in detail in the Day 5 summary):

1. Add a card for verification on Railway (fastest, usually no real charge
   as long as usage stays within free-trial limits)
2. Switch to Render.com (a free web service with no card required, but
   needs MySQL hosted elsewhere)
3. Azure for Students, if eligible

**Result:** no live public URL yet. This is the biggest open item at the
end of the week.

---

## Day 5 — Definition of Done Audit, Sprint Review & Retrospective

### Files
- `docs/Definition-of-Done-Audit.md` — a full audit against the 6 official
  criteria, surfacing two new gaps found today (not covered in any prior
  lab):
  - **ERD** — the entity relationship diagram; hadn't been touched at all
    before today
  - **Nullable compiler warnings** — hadn't been checked before today
- `docs/Sprint4-Retrospective-and-Summary.md` — ready-made retrospective
  and summary templates, with intentional blanks for you to fill in from
  your actual experience

---

## Definition of Done status (at a glance)

| Item | Status |
|---|---|
| All sprint tasks + API runs end-to-end |  Needs actual verification |
| Swagger/OpenAPI + Postman (test per endpoint) |  Code is ready, actual application needs confirming |
| ERD + EF Core Migrations |  Migrations exist, ERD doesn't |
| Complete README |  |
| Live deploy + full CI/CD |  CI is ready, deploy blocked (Railway) |
| Zero compiler warnings |  Not yet checked |

**Bottom line:** the week is technically complete on the Testing, Docs,
and CI fronts, and there are three real items that still need closing
before final submission: **resolving hosting, creating the ERD, and
zeroing out nullable warnings**. The full path to closing each one is in
`docs/Definition-of-Done-Audit.md`.