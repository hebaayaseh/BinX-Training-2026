#  Day 2: Case Study & Profile Updates

## Project Case Study — CardioTrack API

### 1. Problem

CardioTrack is an ASP.NET Core Web API designed to support cardiac-care workflows, including patients, appointments, vital signs, medical data, staff management, authentication, and role-based access.

As the API grew, the main challenges were maintaining performance across frequently used list endpoints, protecting sensitive medical data, and keeping the growing API well-tested and documented.

---

## 2. Architecture

The backend was built with:

* ASP.NET Core Web API
* C#
* Entity Framework Core
* MySQL
* ASP.NET Core Identity
* JWT Authentication & Role-Based Authorization
* Redis / `IDistributedCache`
* Fluent API and EF Core Migrations
* xUnit integration and service tests
* Swagger / OpenAPI
* Postman
* GitHub Actions CI

The application follows a layered structure with Controllers, Services, Data Access, Identity, and supporting infrastructure.

---

## 3. Biggest Challenge

The biggest challenge was maintaining correctness and reliability as the API expanded.

During Sprint 4, a test-coverage audit was performed across **60 endpoints**. This exposed several issues in authentication, integration testing, Redis configuration, and test isolation.

One important security issue was found in `AuthService.LoginAsync`: inactive users were still able to log in because `user.IsActive` was not being checked.

The issue was fixed and covered with a regression test.

---

## 4. Performance Work — Sprint 3

Sprint 3 focused on diagnosing and improving the performance of the main list endpoints.

### Database and Query Analysis

Realistic test data was added:

* **55 patients**
* **150+ vital signs**
* **60+ appointments**

Three important list endpoints were analyzed for query behavior.

The investigation found no genuine N+1 query problem. Instead, an unnecessary `Include` was causing over-fetching.

After removing it:

* Query count remained **1 → 1**
* Columns fetched per row decreased from approximately **12 → 2**

### Redis Caching

Redis caching was implemented for the patients list endpoint using a cache-aside strategy.

Measured locally:

* Cache miss: **16 ms**
* Cache hit: **6 ms**
* Cache expiration: **5 minutes**

Cache invalidation was also implemented so patient changes were reflected immediately instead of waiting for cache expiration.

### Database Indexing

Three frequently filtered column combinations were indexed:

* `Patient.DoctorId`
* `VitalSignAlert.(PatientId, IsResolved)`
* `Appointment.(DoctorId, Status)`

Using MySQL `EXPLAIN`, the query plan changed from:

`type=ALL` → `type=ref`

for the three indexed query patterns, replacing full table scans with index lookups.

---

## 5. Testing & Quality — Sprint 4

Sprint 4 focused on closing testing gaps, improving API documentation, and establishing CI.

A complete audit covered **60 endpoints**, classified by test coverage and risk.

The test suite initially had:

**27 failing tests**

After resolving the underlying issues, it reached:

**0 failing tests**

One intentionally failing regression test was kept to document the inactive-user authentication bug.

Several testing problems were also identified and fixed, including:

* Shared integration-test database conflicts
* Redis being registered during testing
* Missing default values for `User.PhoneNumber`
* Incorrect exception-type assertions
* JWT collisions generated within the same second

GitHub Actions was also configured to automatically:

* Restore dependencies
* Build the project
* Run tests
* Upload `.trx` test results and coverage artifacts

---

## 6. Outcome

The project progressed from a growing backend into a more structured and measurable API.

Key outcomes included:

* **60 endpoints** audited for test coverage
* **27 failing tests → 0**
* **16 ms → 6 ms** measured cache miss/hit times
* Approximately **12 → 2** columns fetched per row in the optimized query
* Query plans improved from `ALL` to `ref` for the targeted indexes
* Redis caching implemented with invalidation
* Authentication security bug identified and fixed
* Automated build and test pipeline added
* API documentation and Postman testing guidelines prepared

The main remaining deployment work at the end of Sprint 4 was hosting the application publicly, along with completing the ERD and checking nullable compiler warnings.

---

# CV Bullet

**CardioTrack API — ASP.NET Core, C#, EF Core, MySQL, Redis, xUnit**

* Developed a cardiac-care backend with **60 API endpoints**, JWT authentication, role-based authorization, MySQL/EF Core, and Redis caching; optimized database access from approximately **12 to 2 columns per row**, reduced measured cache response time from **16 ms to 6 ms**, and improved test stability from **27 failing tests to 0**.

---

# LinkedIn Profile — Project Description

**CardioTrack API | ASP.NET Core Backend**

Built a cardiac-care backend using ASP.NET Core, C#, Entity Framework Core, MySQL, Redis, JWT authentication, and xUnit.

Worked on authentication, role-based authorization, patient and medical-data workflows, performance optimization, database indexing, caching, automated testing, API documentation, and GitHub Actions CI.

Key results included auditing 60 endpoints, resolving 27 failing tests, implementing Redis caching, and improving targeted MySQL query plans through indexing.

---

# LinkedIn Post

I’ve completed another stage of my backend development training by working on **CardioTrack**, an ASP.NET Core Web API for cardiac-care workflows.

During the project, I worked on:

* ASP.NET Core Web API and C#
* Entity Framework Core & MySQL
* JWT authentication and role-based authorization
* Redis distributed caching
* Database indexing and query analysis
* xUnit testing
* Swagger / Postman documentation
* GitHub Actions CI

One of the most valuable parts was measuring the impact of the work rather than only implementing features.

Some results from the project:

* Audited **60 API endpoints**
* Reduced failing tests from **27 to 0**
* Measured Redis cache performance at **16 ms cache miss vs. 6 ms cache hit**
* Reduced unnecessary columns fetched per row from approximately **12 to 2**
* Improved targeted MySQL query plans from full table scans to index lookups

This project helped me understand more deeply how backend development goes beyond writing endpoints — performance, testing, security, and maintainability are all part of building a reliable API.

🔗 GitHub: https://github.com/hebaayaseh/BinX-Training-2026


