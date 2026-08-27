# Week 6 — Advanced Backend Development & CardioTrack Sprint

## Overview

During Week 6, I continued developing the **CardioTrack** backend project by applying more advanced backend development concepts and expanding the system with new features.

The main focus of this week was on database design, Entity Framework Core modeling, querying and pagination, audit logging, database transactions, appointment management, laboratory workflows, doctor scheduling, emergency contacts, role-based authorization, and data consistency.

Throughout the week, I worked on extending the existing CardioTrack application while applying real-world backend practices such as validation, authorization, transactional consistency, role-based data access, and safe audit logging.

---

## Week Objectives

The main objectives of this week were:

* Plan and organize the next development sprint for the CardioTrack project.
* Design and extend the database schema.
* Build and configure the Entity Framework Core data model.
* Implement filtering, sorting, and pagination.
* Add centralized audit logging.
* Apply database transactions to maintain data consistency.
* Improve appointment booking functionality.
* Implement laboratory request and result workflows.
* Add doctor scheduling functionality.
* Implement patient emergency contact management.
* Apply role-based authorization and data scoping.
* Use FluentValidation and business rule validation.
* Improve logging by avoiding Entity Framework Core serialization cycles.

---

# Day 1 — Planning & Full Schema Design

The first day focused on planning the next sprint and preparing the database design for new functionality.

### Completed Work

* Reviewed the existing CardioTrack database structure.
* Planned the next development sprint.
* Designed the `AuditLog` entity.
* Defined the main audit log fields:

  * `Id`
  * `EntityName`
  * `EntityId`
  * `Action`
  * `PerformedByUserId`
  * `Time`
* Defined the relationship between `AuditLog` and `User`.
* Planned audit logging for important CRUD operations.
* Identified upcoming entities and relationships.
* Organized the Sprint 1 development tasks.
* Prepared the project for a more structured and production-oriented database design.

---

# Day 2 — Entity Framework Core Model & Database Setup

On Day 2, the database design was translated into a complete Entity Framework Core model.

### Completed Work

* Implemented entity classes based on the CardioTrack ERD.
* Added navigation properties between related entities.
* Configured entity relationships using the Fluent API.
* Explicitly configured delete behavior for selected relationships.
* Added seed data for the `Medication` reference table using `HasData`.
* Generated EF Core migrations.
* Reviewed the generated migration files.
* Applied migrations to the database.
* Verified that the generated schema matched the intended ERD design.

---

# Day 3 — Querying, Filtering, Pagination & Audit Logging

Day 3 focused on improving data retrieval and implementing centralized audit logging.

### Patient Query Functionality

Implemented a `QueryService` for retrieving patients assigned to the authenticated doctor.

The functionality includes:

* Authentication and authorization validation.
* Filtering patients by:

  * `Gender`
  * `BloodType`
* Dynamic sorting by:

  * `FullName`
  * `DateOfBirth`
* Ascending and descending sorting.
* Pagination using:

  * `PageNumber`
  * `PageSize`
* Returning pagination metadata such as:

  * `TotalCount`
  * `TotalPages`
  * Current page number
  * Current page size

### Audit Logging

Implemented an `AuditLogService` to record important actions performed within the system.

The audit log stores:

* User ID
* Action
* Entity name
* Entity ID
* Old value
* New value
* Timestamp

Additional work included:

* Using `IHttpContextAccessor` to retrieve the authenticated user ID from JWT claims.
* Serializing old and new values using `JsonSerializer`.
* Creating an Admin-only endpoint for viewing audit logs.
* Protecting the audit log functionality using the `AdminOnly` authorization policy.

---

# Day 4 — Appointment Booking & Database Transactions

On Day 4, the existing appointment functionality was enhanced by applying validation, business rules, fee calculation, and database transactions.

### Appointment Validation

Implemented validation to ensure:

* The authenticated user exists.
* The user is active.
* The user has the appropriate role.
* The selected patient exists.
* The patient belongs to the requested doctor.
* The selected doctor does not already have an appointment at the same date and time.

This prevents double booking and scheduling conflicts.

### Appointment Fee Calculation

Added a `Fee` property to the `Appointment` entity.

The appointment fee is calculated dynamically based on the appointment reason using dedicated business logic.

### Related Vital Sign Alerts

Added support for an optional `RelatedAlertId`.

When an appointment is created in response to a Vital Sign Alert, the related alert can be resolved as part of the same operation.

### Database Transactions

Implemented database transactions to ensure data consistency.

The transaction handles:

1. Creating the appointment.
2. Resolving the related alert when applicable.
3. Saving both operations together.
4. Committing only when all operations succeed.
5. Rolling back all changes if an error occurs.

This ensures that the database does not contain partial or inconsistent data.

---

# Day 5 — Lab Management, Doctor Scheduling & Emergency Contacts

The final day focused on expanding CardioTrack with several major backend features.

## Lab Request Management

Implemented functionality for doctors to create one or more lab test requests for a patient.

The implementation includes:

* Doctor role validation.
* Active user validation.
* Patient validation.
* Optional appointment validation.
* Creating multiple lab requests in a single operation.

### Role-Based Data Access

Lab requests are filtered based on the authenticated user's role:

* Patients can view their own requests.
* Doctors can view requests they created.
* Technicians and administrators can view all relevant requests.

### Status Management

Implemented controlled status transitions such as:

* `Seen`
* `Collected`
* `Cancelled`

The `Completed` status cannot be manually assigned because it is automatically set when a lab result is successfully uploaded.

---

## Lab Result Management

Implemented functionality for technicians to upload laboratory result files.

The implementation includes:

* File upload handling.
* Saving the file path or relative URL.
* Linking a result to an optional lab request.
* Preventing multiple results from being uploaded for the same request.
* Role-based access to lab results.

### Automatic Request Completion

When a lab result is successfully created and linked to a lab request, the related request is automatically marked as `Completed`.

Both operations are handled together to maintain database consistency.

---

## Doctor Schedule Management

Implemented weekly doctor scheduling functionality.

Features include:

* Adding weekly schedule slots.
* Detecting overlapping time ranges.
* Preventing conflicting schedules for the same doctor.
* Updating schedules by deactivating the old slot.
* Creating a replacement schedule slot.
* Automatically identifying appointments affected by the old schedule and marking them as postponed when necessary.

---

## Emergency Contact Management

Implemented full CRUD operations for patient emergency contacts.

The functionality includes:

* Adding emergency contacts.
* Updating emergency contacts.
* Deleting emergency contacts.
* Retrieving patient emergency contacts.

---

# Audit Log Integration

The existing `IAuditLog` service was integrated into the newly developed features, including:

* Lab Requests
* Lab Results
* Doctor Schedules
* Emergency Contacts
* Status changes
* Create and update operations

Each important action records information such as:

* Acting user
* Action type
* Entity name
* Entity ID
* Previous values
* New values
* Timestamp

---

# Key Concepts Applied

## Database Transactions

Transactions were used when multiple database operations needed to succeed together.

Examples include:

* Creating an appointment and resolving a related alert.
* Creating a lab result and automatically completing its related lab request.

If one operation fails, all related changes are rolled back.

---

## Role-Based Authorization

Different endpoints and data access rules were applied depending on the authenticated user's role.

Examples include:

* Doctor-only operations.
* Technician/Admin operations.
* Patient-specific data access.
* Admin-only audit log access.

This ensures that users only access the functionality and data they are authorized to use.

---

## Data Scoping

Data is filtered at the database query level based on the authenticated user's role and relationship to the data.

Examples:

* Patients only see their own records.
* Doctors only see data related to their patients or requests.
* Technicians and administrators have broader access where appropriate.

---

## Safe Audit Logging

While integrating audit logging, a circular reference problem occurred when Entity Framework Core tracked entities with navigation properties were serialized directly.

To solve this issue, lightweight anonymous objects containing only the necessary primitive fields are used when creating audit logs.

Instead of logging full tracked entities, the system logs safe snapshots of the relevant data.

This prevents:

* Circular reference exceptions.
* Infinite serialization loops.
* Unnecessary data from being stored in audit logs.

---

## Validation & Business Rules

Validation was applied at multiple levels to ensure correct system behavior.

Examples include:

* User and role validation.
* Active account validation.
* Appointment availability checks.
* Schedule conflict detection.
* Lab request validation.
* File upload validation.
* Preventing invalid status transitions.
* Preventing duplicate lab results.

---

# Technologies & Tools

The following technologies and tools were used throughout Week 6:

* ASP.NET Core Web API
* C#
* .NET
* Entity Framework Core
* MySQL
* Pomelo Entity Framework Provider
* LINQ
* FluentValidation
* JWT Authentication
* Role-Based Authorization
* EF Core Migrations
* Database Transactions
* File Upload Handling
* Visual Studio

---

# Week 6 Summary

Week 6 focused on moving the CardioTrack project toward a more complete and production-oriented backend application.

Throughout the week, I worked on:

* Database and sprint planning.
* Entity Framework Core modeling.
* Database migrations and schema management.
* Advanced querying with filtering, sorting, and pagination.
* Centralized audit logging.
* Appointment availability validation.
* Dynamic appointment fee calculation.
* Database transactions.
* Lab request management.
* Lab result file uploads.
* Automatic status transitions.
* Doctor scheduling and conflict detection.
* Appointment postponement handling.
* Patient emergency contact management.
* Role-based authorization and data access.
* Safe entity serialization for audit logs.

By the end of the week, the CardioTrack backend had been significantly extended with more complex business logic, stronger validation, improved data consistency, and better control over system activity and user access.

---

## Repository Structure

```text
Week6
│
├── Day1
│   ├── Planning & Full Schema Design
│   └── AuditLog Entity Design
│
├── Day2
│   ├── EF Core Model
│   ├── Fluent API
│   ├── Seed Data
│   └── Migrations
│
├── Day3
│   ├── Filtering
│   ├── Sorting
│   ├── Pagination
│   └── Audit Logging
│
├── Day4
│   ├── Appointment Validation
│   ├── Availability Check
│   ├── Fee Calculation
│   └── Database Transactions
│
└── Day5
    ├── Lab Requests
    ├── Lab Results
    ├── File Uploads
    ├── Doctor Scheduling
    ├── Emergency Contacts
    └── Audit Log Integration
```
