# Week 7 — Identity, Authorization & Custom Middleware

## Overview

Week 7 focused on integrating ASP.NET Core Identity into the BookNest library reservation API, implementing authentication and JWT-based authorization, applying role-based access control and ownership checks, and centralizing exception handling through custom middleware.

The week was structured as a complete security and API architecture sprint, starting from Identity integration and ending with centralized error handling.

---

## Sprint Goal

Build a secure and maintainable authentication and authorization system for BookNest by:

* Integrating ASP.NET Core Identity.
* Implementing registration and login.
* Linking Identity users with domain entities.
* Issuing JWT tokens with custom claims.
* Applying role-based authorization.
* Enforcing ownership of member-specific resources.
* Centralizing exception handling with custom middleware.

---

## Day 1 — Sprint 2 Planning & Identity Integration

Integrated ASP.NET Core Identity into BookNest and replaced manual user and role management with Identity's built-in infrastructure.

### Key Tasks

* Created `ApplicationUser : IdentityUser<int>`.
* Added a custom `FullName` property.
* Configured `BookNestDbContext` using `IdentityDbContext`.
* Seeded `Admin` and `Member` roles.
* Implemented registration and login endpoints.
* Added JWT authentication with role claims.
* Created `AdminOnly` and `MemberOnly` authorization policies.
* Protected endpoints according to their required role.

### Main Endpoints

| Endpoint                  | Method | Access |
| ------------------------- | ------ | ------ |
| `/api/auth/register`      | POST   | Public |
| `/api/auth/login`         | POST   | Public |
| `/api/books`              | GET    | Public |
| `/api/books`              | POST   | Admin  |
| `/api/books/{id}/reserve` | POST   | Member |

The reservation endpoint also checks available copies and returns `409 Conflict` when no copies remain.

---

## Day 2 — Registration & Login with Domain Entity

Extended the Identity setup by introducing a separate `MemberProfile` domain entity.

### Key Tasks

* Created `MemberProfile`.
* Linked `MemberProfile` to `ApplicationUser` using a one-to-one relationship.
* Added a unique foreign key.
* Implemented transactional registration.
* Created both `ApplicationUser` and `MemberProfile` in one transaction.
* Added `memberProfileId` as a custom JWT claim.
* Tested registration and login using Postman.
* Verified JWT claims using jwt.io.

### Why Separate `MemberProfile`?

`ApplicationUser` is responsible for authentication and security-related information, while `MemberProfile` contains business-specific information such as:

* `FullName`
* `JoinedAt`
* `MaxBooksAllowed`

This keeps Identity concerns separate from domain concerns.

### Why Use a Transaction?

The transaction guarantees that the `ApplicationUser` and `MemberProfile` are created together. If one operation fails, the other is rolled back, preventing incomplete or orphaned accounts.

---

## Day 3 — RBAC & Ownership Checks

Audited the API endpoints and applied the appropriate authorization requirements.

### Key Tasks

* Applied role-based authorization to endpoints.
* Added `my-reservations` endpoints for members.
* Used the `memberProfileId` JWT claim to identify the authenticated member.
* Added ownership checks for individual reservations.
* Added an Admin-only endpoint for retrieving all reservations.
* Tested multiple users and roles using Postman.

### Security Behavior

A member can only access their own reservation data.

When requesting another member's reservation, the API returns:

`404 Not Found`

instead of:

`403 Forbidden`

This prevents revealing whether a resource belonging to another user exists. Ownership is enforced directly in the database query so unauthorized data is not loaded into memory.

---

## Day 4 — Custom Exception Middleware

Implemented centralized exception handling as a cross-cutting concern.

### Key Tasks

* Created `ExceptionMiddleware`.
* Registered the middleware in the ASP.NET Core pipeline.
* Converted exceptions into RFC 7807 `ProblemDetails` responses.
* Created a custom exception hierarchy:

  * `AppException`
  * `NotFoundException`
  * `BadRequestException`
  * `ConflictException`
* Refactored controllers to throw exceptions instead of manually returning error responses.
* Added server-side logging using `ILogger<T>`.
* Tested exception handling across multiple endpoints.

### Benefits

Centralized exception handling:

* Removes repeated `try/catch` blocks.
* Keeps controllers cleaner.
* Provides consistent API error responses.
* Prevents internal stack traces from being exposed to clients.
* Makes error handling reusable across the entire API.

---

## Day 5 — Sprint Summary

The final day consolidated the work completed throughout Sprint 2.

### Complete Authentication Flow

1. `POST /api/auth/register`

   * Creates an `ApplicationUser`.
   * Creates a `MemberProfile`.
   * Performs both operations inside a transaction.
   * Assigns the `Member` role by default.

2. `POST /api/auth/login`

   * Validates user credentials.
   * Retrieves the user's roles.
   * Adds the appropriate JWT claims.
   * Adds `memberProfileId` for members.

3. JWT contains:

   * `NameIdentifier`
   * `Email`
   * `Name`
   * `Role`
   * `memberProfileId` when applicable.

### Final RBAC Matrix

| Endpoint                          | Method | Access Level             |
| --------------------------------- | ------ | ------------------------ |
| `/api/auth/register`              | POST   | Public                   |
| `/api/auth/login`                 | POST   | Public                   |
| `/api/books`                      | GET    | Public                   |
| `/api/books`                      | POST   | Admin only               |
| `/api/books/{id}/reserve`         | POST   | Member only              |
| `/api/books/my-reservations`      | GET    | Member only              |
| `/api/books/my-reservations/{id}` | GET    | Member + Ownership Check |
| `/api/books/all-reservations`     | GET    | Admin only               |

---

## Key Bug Fixed

During the sprint, an issue was discovered where Admin login failed because the login process always expected a `MemberProfile`.

The issue was fixed by checking the user's role before looking up the profile.

* Members → have a `MemberProfile` and receive `memberProfileId`.
* Admins → do not require a `MemberProfile`.

This allowed both roles to authenticate correctly.

---

## Tech Stack

* ASP.NET Core Web API
* .NET 8
* ASP.NET Core Identity
* Entity Framework Core
* SQLite
* JWT Bearer Authentication
* Role-Based Authorization
* Custom Middleware
* RFC 7807 `ProblemDetails`
* Postman
* jwt.io

---

## Key Concepts Learned

* ASP.NET Core Identity
* `IdentityUser`
* `IdentityRole`
* `IdentityDbContext`
* `UserManager`
* JWT Authentication
* JWT Claims
* Role-Based Access Control (RBAC)
* Resource Ownership
* Database-level authorization checks
* Transactions
* Custom Middleware
* Global Exception Handling
* `ProblemDetails`
* Cross-Cutting Concerns
* Secure API design

---

## Sprint Outcome

By the end of Week 7, BookNest had a complete authentication and authorization foundation with:

* Identity-based user management.
* Secure registration and login.
* JWT-based authentication.
* Role-based access control.
* Member ownership enforcement.
* Centralized exception handling.
* Consistent API error responses.
* Proper separation between Identity and domain data.
