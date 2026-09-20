# CardioTrack — Presentation Deck Outline

## 1. Title slide

- CardioTrack — [one-line tagline, e.g. "A role-based cardiac patient
  tracking API"]
- Heba Hesham Ayaseh, BinX Tech Backend .NET Internship, 2026/9/20

## 2. Problem

- Cardiac-care workflows need a structured backend for patients, staff, appointments, medications, medical history, lab requests/results, and vital signs.
- The API must protect sensitive patient data through authentication, authorization, validation, and controlled access by role.
- The project also needs reliable testing, documentation, and CI for maintainability.

## 3. Architecture

- ASP.NET Core Web API
- Controllers → Services → EF Core / Identity
- MySQL for application data
- JWT authentication with role-based authorization
- FluentValidation for request validation
- Global exception handling
- Redis distributed caching for repeated patient-list queries
- xUnit + Moq + integration tests
- Swagger/OpenAPI, Postman, and GitHub Actions CI

## 4. Biggest challenge


Candidates from the actual work on this project — pick whichever you can
tell most concretely, or use your own if a different one stands out more:

- **The JWT collision bug**: two tokens issued within the same second were
  byte-for-byte identical because the JWT had no unique claim (`jti`) —
  found only because a test asserted token rotation and failed on
  "expected not equal, got equal." Fixed by adding a `Jti` claim. Good
  story because it shows reading a *passing-looking* system critically,
  not just fixing failures.
- **The shared in-memory test database bug**: integration tests started
  returning random 500s once more test classes were added, traced back to
  `Guid.NewGuid()` being evaluated *inside* a DI lambda that runs twice
  (once for seeding, once for the real host), producing two different
  database names instead of one. Good story because the bug was invisible
  until scale (more test classes) exposed it.
- **The `IsActive` security gap**: writing the test-coverage audit
  surfaced that deactivated accounts could still log in and get a valid
  access token, because `LoginAsync` never checked `IsActive`. Good story
  because it's a real security finding from a systematic process
  (endpoint-by-endpoint audit), not luck.

For whichever you pick: state the symptom, the wrong assumption that hid
it, how you found the real cause, and the fix — in that order.

## 5. Performance work

*What did you measure, and what changed?*

- Identified a repeated patient-list query as a performance opportunity.
- Added Redis distributed caching with a 5-minute absolute expiration.
- Cached the doctor’s patient dataset, then applied filtering, sorting, and pagination to the cached data.
- Avoided leaving temporary request-timing debug output in the final repository.

## 6. Testing & quality

- Unit tests with xUnit and Moq
- Integration tests for authentication and role protection
- Regression coverage for the deactivated-user login bug
- CI workflow for restore, build, test, and test/coverage artifacts
- Swagger/OpenAPI and Postman documentation work

## 7. Outcome

- A structured ASP.NET Core backend with role-aware medical workflows
- Automated build/test pipeline
- Distributed caching for a high-use read path
- Documented API and testing workflow
- Clear remaining deployment/documentation follow-ups recorded rather than hidden

