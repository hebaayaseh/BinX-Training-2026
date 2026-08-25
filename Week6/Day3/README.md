# Day 2 — CardioTrack & Entity Framework Core Model

## Overview

Building the full Entity Framework Core data model for the CardioTrack project based on the Day 1 ERD.

## Steps Completed

* Implemented entity classes for all tables defined in the CardioTrack ERD.
* Added the required navigation properties between related entities.
* Configured relationships between entities using the Fluent API.
* Explicitly configured delete behavior for selected relationships.
* Added seed data for the `Medication` reference table using `HasData`.
* Generated the initial EF Core migration for the CardioTrack database.
* Reviewed the generated migration file before applying it to verify the schema changes.
* Applied the migration to the SQL Server database.
* Verified that the generated database schema matches the original ERD design.

## Tools

Entity Framework Core · SQL Server · Visual Studio · .NET SDK 
