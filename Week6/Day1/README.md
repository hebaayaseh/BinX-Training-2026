# Day 1 — Planning & Full Schema Design

## Overview

Planned Sprint 1 for the CardioTrack project and designed the database schema for the new audit logging functionality. The goal was to extend the existing data model with an `AuditLog` entity and prepare the project for a more structured and production-ready database design.

## Steps Completed

* Reviewed the current CardioTrack database entities and identified the entities planned for the next development phase.
* Designed the `AuditLog` entity to track important system actions.
* Defined the main fields for the audit log, including:

  * `Id`
  * `EntityName`
  * `EntityId`
  * `Action`
  * `PerformedByUserId`
  * `Time`
* Defined the relationship between `AuditLog` and the `User` entity using `PerformedByUserId` as a foreign key.
* Planned the implementation of audit logging for CRUD operations using middleware or an interceptor.
* Broke down Sprint 1 into tasks, including audit log implementation, CRUD operations for new entities, unit testing, and updating the ERD diagram.
* Planned the future database entities and relationships to keep the schema organized and normalized.

## Sprint 1 Planning

The main tasks planned for this sprint are:

* Design the AuditLog entity.
* Implement audit logging for CRUD operations.
* Create AuditLog controllers and services.
* Design new entities and their relationships.
* Implement CRUD operations for the new entities.
* Write unit tests for the audit logging functionality.
* Update the ERD diagram.
* Add documentation for Sprint 1.

## Tools

* ASP.NET Core
* Entity Framework Core
* MySQL
* EF Core Migrations
* Database Schema Design
* ERD Diagram
