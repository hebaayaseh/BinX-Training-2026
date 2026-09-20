# CardioTrack Presentation Outline

## 1. Problem
- Cardiac-care workflows need a structured backend for patients, staff, appointments, medications, medical history, lab requests/results, and vital signs.
- The API must protect sensitive patient data through authentication, authorization, validation, and controlled access by role.
- The project also needs reliable testing, documentation, and CI for maintainability.

## 2. Architecture
- ASP.NET Core Web API
- Controllers → Services → EF Core / Identity
- MySQL for application data
- JWT authentication with role-based authorization
- FluentValidation for request validation
- Global exception handling
- Redis distributed caching for repeated patient-list queries
- xUnit + Moq + integration tests
- Swagger/OpenAPI, Postman, and GitHub Actions CI

## 3. Biggest Challenge
**Maintaining correctness as the API grew.**
- Multiple roles and protected medical-data workflows
- Integration tests exposing shared test-database and environment-configuration issues
- Deactivated users needed to be prevented from logging in
- Cache-dependent endpoints needed to behave correctly in the test environment

## 4. Performance Work
- Identified a repeated patient-list query as a performance opportunity.
- Added Redis distributed caching with a 5-minute absolute expiration.
- Cached the doctor’s patient dataset, then applied filtering, sorting, and pagination to the cached data.
- Avoided leaving temporary request-timing debug output in the final repository.

## 5. Testing & Quality
- Unit tests with xUnit and Moq
- Integration tests for authentication and role protection
- Regression coverage for the deactivated-user login bug
- CI workflow for restore, build, test, and test/coverage artifacts
- Swagger/OpenAPI and Postman documentation work

## 6. Outcome
- A structured ASP.NET Core backend with role-aware medical workflows
- Automated build/test pipeline
- Distributed caching for a high-use read path
- Documented API and testing workflow
- Clear remaining deployment/documentation follow-ups recorded rather than hidden

## 7. Closing
- What I learned technically
- What I would improve next
- Short demo: authentication → patient workflow → cached patient list → tests/CI
