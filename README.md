# BinX Training 2026

Backend Development Internship — .NET / ASP.NET Core

This repository documents my 10-week backend development training at BinX Tech, covering C#, ASP.NET Core, databases, authentication, authorization, testing, performance optimization, CI, documentation, deployment, and technical presentation.

The training progressed from C# fundamentals and backend concepts to building and improving a complete backend project.

---

# Training Journey

## Week 1 — C# Fundamentals & Git

Built the foundation for .NET backend development.

Topics included:

* C# fundamentals
* Types and variables
* Control flow
* Nullable reference types
* Object-Oriented Programming
* Classes and records
* Interfaces
* Encapsulation
* Collections
* LINQ
* `async` / `await`
* Git and GitHub

---

## Week 2 — Advanced C# & API Development

Focused on backend programming and ASP.NET Core fundamentals.

Topics included:

* Generics
* Generic repositories
* Advanced LINQ
* `GroupBy`
* `Join`
* `SelectMany`
* Deferred execution
* `Task`
* `Task.WhenAll`
* `CancellationToken`
* ASP.NET Core Web API
* Controllers
* Minimal APIs
* Middleware
* Dependency Injection
* Swagger
* Postman

---

## Week 3 — REST APIs & Database Design

Focused on API design, database modeling, and API testing.

Topics included:

* REST API design
* API resources and endpoints
* Database normalization
* 1NF, 2NF, and 3NF
* Entity Relationship Diagrams
* Primary and foreign keys
* Postman collections
* Postman environments
* API test scripts
* Swagger documentation

---

## Week 4 — Authentication, Authorization & Validation

Focused on securing APIs and validating incoming requests.

Topics included:

* ASP.NET Core Identity
* User registration and login
* JWT authentication
* JWT Bearer Authentication
* Role-Based Access Control
* Claims-based authorization
* Policy-based authorization
* FluentValidation
* Rate limiting
* CORS
* Security headers
* HTTPS / HSTS
* Postman authentication testing

---

## Week 5 — Testing, Error Handling & CardioTrack Kickoff

Started the CardioTrack backend project and focused on testing high-risk business logic.

Key work included:

* Vital-sign alert testing
* Appointment conflict testing
* Doctor-patient ownership testing
* Unit testing with xUnit
* Mocking with Moq
* Integration testing with `WebApplicationFactory`
* Global exception middleware
* RFC 7807 `ProblemDetails`
* Safe exception logging

---

## Week 6 — Advanced Backend Development

Expanded CardioTrack with more complex backend workflows.

Key work included:

* Entity Framework Core modeling
* Fluent API configuration
* Database migrations
* Filtering
* Sorting
* Pagination
* Audit logging
* Database transactions
* Appointment booking
* Appointment fee calculation
* Laboratory requests
* Laboratory results
* File uploads
* Doctor scheduling
* Schedule conflict detection
* Emergency contacts
* Role-based data access
* Business-rule validation

---

## Week 7 — Identity, Authorization & Middleware

Focused on authentication architecture and secure API access through a separate Identity-based backend sprint.

Key work included:

* ASP.NET Core Identity
* `IdentityUser`
* `IdentityRole`
* Registration and login
* JWT claims
* Role-based authorization
* Resource ownership
* Database-level authorization checks
* Transactions
* Custom exception middleware
* RFC 7807 `ProblemDetails`

---

# Main Project — CardioTrack

## Cardiac Patient Monitoring System

CardioTrack is the main backend project developed during the training.

It is an ASP.NET Core Web API designed to support cardiac patient-care workflows and secure access to medical information.

The system covers workflows including:

* Authentication
* Role-based authorization
* Patient management
* Doctor management
* Nurse workflows
* Appointments
* Vital signs
* Vital-sign alerts
* Medications
* Medical history
* Laboratory requests
* Laboratory results
* Doctor schedules
* Emergency contacts
* Audit logging

The project evolved throughout the training as new requirements, testing scenarios, and performance concerns were introduced.

---

# Architecture

The backend follows a layered architecture:

```text
Client
  │
  ▼
ASP.NET Core Web API
  │
  ▼
Application / Services
  │
  ▼
Domain
  │
  ▼
Infrastructure
  │
  ▼
Entity Framework Core
  │
  ▼
MySQL
```

Supporting components include:

* JWT Authentication
* Role-Based Authorization
* FluentValidation
* Redis Distributed Cache
* Global Exception Handling
* Swagger / OpenAPI
* Postman
* GitHub Actions

---

# Security

Security was a major part of the project.

Implemented concepts include:

* JWT authentication
* Role-based authorization
* Claims and policies
* Resource ownership checks
* User activity checks
* Request validation
* Protected medical-data access
* Centralized exception handling
* Safe error responses
* Audit logging

The project also used testing to identify security issues that were not immediately visible during normal development.

---

# Sprint 3 — Performance Optimization

Performance work was performed using realistic test data:

* **55 patients**
* **150+ vital signs**
* **60+ appointments**

The investigation started with EF Core query logging and actual query analysis.

### Over-fetching

A patient/appointment query contained an unnecessary navigation `Include`.

The query count remained:

`1 → 1`

But the amount of selected data was reduced from approximately:

`~12 columns → 2 columns`

This reduced unnecessary data retrieval without claiming an artificial reduction in query count.

### Redis Caching

Redis was introduced using `IDistributedCache`.

The patient-list endpoint uses:

* Cache-aside strategy
* Doctor-specific cache keys
* 5-minute expiration
* Explicit cache invalidation after patient create/update

Measured results:

```text
Cache Miss: 16 ms
Cache Hit:   6 ms
```

### Database Indexing

Indexes were added for frequently filtered data:

```text
Patient.DoctorId

VitalSignAlert.(PatientId, IsResolved)

Appointment.(DoctorId, Status)
```

MySQL `EXPLAIN` was used to verify the query plans.

The observed query-plan change was:

```text
type=ALL
     ↓
type=ref
```

---

# Sprint 4 — Testing, Documentation & CI

The testing sprint included an audit of **60 API endpoints**.

The endpoints were reviewed according to testing coverage and risk, with priority given to:

* Authentication
* Sensitive medical data
* Role-protected endpoints
* Remaining API functionality

### Testing Results

The test suite initially had:

**27 failing tests**

After investigating and fixing the underlying issues:

**0 failing tests**

One intentionally failing regression test was retained to document the specific security-bug scenario.

Issues investigated included:

* Inactive-user login
* Integration-test database isolation
* Redis configuration in the testing environment
* Missing `User.PhoneNumber` default behavior
* Exception assertions
* JWT collisions during tests

A real authentication issue was also discovered: inactive users could still log in because `user.IsActive` was not checked during login. The issue was fixed and covered with regression testing.

---

# CI & Documentation

GitHub Actions was configured to automate:

```text
Push
  ↓
Restore
  ↓
Build
  ↓
Test
  ↓
Test / Coverage Artifacts
```

The project also includes work around:

* Swagger / OpenAPI
* XML documentation
* API examples
* Postman endpoint checklist
* Project setup documentation
* Environment variables
* User Secrets
* Database migrations
* CI test artifacts
* Docker configuration

---

# Deployment

The project includes containerization and deployment preparation using Docker and CI/CD workflows.

Deployment configuration includes:

* Multi-stage .NET 8 Docker build
* `.dockerignore`
* Deployment documentation
* CI deployment workflow
* Environment configuration

Deployment status and hosting details are documented in the relevant Week 9 files.

---

# Week 10 — Finalization & Presentation

The final week focused on preparing the project and the presentation for completion.

Activities included:

### Repository Cleanup

* Reviewed the repository
* Removed leftover debug code
* Cleaned obsolete commented experiments
* Reviewed project documentation
* Organized final project materials

### Case Study

Prepared a case study covering:

* Problem
* Architecture
* Biggest technical challenge
* Performance work
* Testing
* Results
* Outcome

### Presentation

Prepared a technical presentation covering:

* Project problem
* Architecture
* Security
* Performance optimization
* Redis caching
* Database indexing
* Testing
* CI
* Live API demonstration
* Technical Q&A

### Final Rehearsal

Prepared:

* Full presentation rehearsal
* Live Postman demo
* Backup demo
* Mock technical Q&A
* Timing checklist

### Final Review

The final stage focuses on:

* Code review
* Mentor feedback
* Final evaluation
* Program outcome
* Personal reflection

---

# Technology Stack

### Backend

* C#
* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* LINQ

### Database

* MySQL
* MariaDB
* EF Core Migrations

### Security

* ASP.NET Core Identity
* JWT
* Role-Based Authorization
* Claims
* Policies
* FluentValidation

### Testing

* xUnit
* Moq
* `WebApplicationFactory`
* Integration Testing

### Performance

* Redis
* `IDistributedCache`
* EF Core Query Logging
* MySQL `EXPLAIN`
* Database Indexing

### API & Documentation

* Swagger / OpenAPI
* Postman

### DevOps

* Git
* GitHub
* GitHub Actions
* Docker

### Development

* Visual Studio
* .NET CLI

---

# Repository Structure

```text
BinX-Training-2026
│
├── Week1
│   └── C# Fundamentals & Git
│
├── Week2
│   └── Advanced C# & API Development
│
├── Week3
│   └── REST APIs & Database Design
│
├── Week4
│   └── Authentication & Validation
│
├── Week5
│   └── Testing, Error Handling & CardioTrack Kickoff
│
├── Week6
│   └── Advanced CardioTrack Development
│
├── Week7
│   └── Identity, Authorization & Middleware
│
├── Week8
│   └── Sprint 3 — Performance Optimization
│
├── Week9
│   └── Sprint 4 — Testing, Docs & Deployment
│
└── Week10
    └── Finalization, Presentation & Wrap-Up
```

---

# Key Outcomes

Throughout the 10-week program, I progressed from C# fundamentals to building, testing, optimizing, documenting, and presenting a backend system.

Key outcomes included:

* Built and extended a real ASP.NET Core backend project
* Applied layered architecture
* Implemented JWT authentication and role-based authorization
* Designed database relationships using EF Core
* Implemented transactions and audit logging
* Added business-rule validation
* Built unit and integration tests
* Audited 60 API endpoints
* Investigated real database performance issues
* Implemented Redis distributed caching
* Added database indexes and verified query plans
* Built a GitHub Actions CI pipeline
* Prepared Docker deployment configuration
* Documented the API with Swagger and Postman
* Prepared and presented the project as a technical case study

---

# What I Learned

The training helped me move beyond implementing individual features and understand the full backend development process.

I learned how to:

* Design backend systems
* Make authorization decisions based on real data relationships
* Test business logic and complete API workflows
* Investigate performance using actual measurements
* Use distributed caching appropriately
* Handle errors consistently
* Maintain data consistency with transactions
* Document APIs and technical decisions
* Use CI to validate changes automatically
* Explain technical decisions clearly

---

# Repository Purpose

This repository serves as a record of my 10-week backend development training and the progression of the CardioTrack project from its initial foundation to a more complete, tested, optimized, documented, and presentation-ready backend system.
