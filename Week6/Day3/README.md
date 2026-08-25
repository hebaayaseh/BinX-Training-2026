# Day 3 — CardioTrack Query, Filtering & Audit Logging

## Overview

On Day 3, I implemented query functionality for retrieving patients with filtering, sorting, and pagination. I also implemented an Audit Log system to track actions performed by authenticated users within the CardioTrack application.

## Steps Completed

* Implemented a `QueryService` for retrieving patients assigned to the authenticated doctor.
* Added authorization validation to ensure that the requesting user exists, has the `Doctor` role, and is active.
* Implemented filtering functionality for patients based on:

  * `Gender`
  * `BloodType`
* Added dynamic sorting functionality based on the requested `SortBy` field.
* Implemented sorting by:

  * `FullName`
  * `DateOfBirth`
* Added support for ascending and descending sorting using the `SortDescending` option.
* Implemented pagination using `PageNumber` and `PageSize`.
* Added calculation for:

  * `TotalCount`
  * `TotalPages`
  * Current `PageNumber`
  * Current `PageSize`
* Created DTOs to return paginated patient data in a structured response.
* Implemented an `AuditLogService` to record important actions performed within the system.
* Used `IHttpContextAccessor` to retrieve the currently authenticated user's ID from JWT claims.
* Stored the following audit log information:

  * User ID
  * Action
  * Entity Name
  * Entity ID
  * Old Value
  * New Value
  * Timestamp
* Used `JsonSerializer` to serialize old and new entity values before storing them in the database.
* Created an Admin-only endpoint for retrieving audit logs.
* Protected the Audit Log endpoint using the `AdminOnly` authorization policy.

## Migration Note

> **Note:** The `HasData` seed data configuration has been temporarily commented out inside the project to avoid migration-related errors. It can be enabled again when the migration and seed data configuration are ready to be applied correctly.

## Tools

Entity Framework Core · ASP.NET Core Web API · SQL Server · JWT Authentication · LINQ · Visual Studio · .NET SDK
